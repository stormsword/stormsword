/*!
 * X-UniTMX: A tiled map editor file importer for Unity3d
 * https://bitbucket.org/Chaoseiro/x-unitmx
 * 
 * Copyright 2013 Guilherme "Chaoseiro" Maia
 * Released under the MIT license
 * Check LICENSE.MIT for more details.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using UnityEngine;
//using UnityEditor;

namespace X_UniTMX
{
	/// <summary>
	/// Defines the possible orientations for a Map.
	/// </summary>
	public enum Orientation : byte
	{
		/// <summary>
		/// The tiles of the map are orthogonal.
		/// </summary>
		Orthogonal,

		/// <summary>
		/// The tiles of the map are isometric.
		/// </summary>
		Isometric,

		/// <summary>
		/// The tiles of the map are isometric (staggered).
		/// </summary>
		Staggered
	}

	/// <summary>
	/// A delegate used for searching for map objects.
	/// </summary>
	/// <param name="layer">The current layer.</param>
	/// <param name="mapObj">The current object.</param>
	/// <returns>True if this is the map object desired, false otherwise.</returns>
	public delegate bool MapObjectFinder(MapObjectLayer layer, MapObject mapObj);

	/// <summary>
	/// A full map from Tiled.
	/// </summary>
	public class Map
	{
		/// <summary>
		/// The difference in layer depth between layers.
		/// </summary>
		/// <remarks>
		/// The algorithm for creating the LayerDepth for each layer when enumerating from
		/// back to front is:
		/// float layerDepth = 1f - (LayerDepthSpacing * x);</remarks>
		public const int LayerDepthSpacing = 1;

		private readonly Dictionary<string, Layer> namedLayers = new Dictionary<string, Layer>();

		/// <summary>
		/// Gets the version of Tiled used to create the Map.
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		/// Gets the orientation of the map.
		/// </summary>
		public Orientation Orientation { get; private set; }

		/// <summary>
		/// Gets the width (in tiles) of the map.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the height (in tiles) of the map.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Gets the width of a tile in the map.
		/// </summary>
		public int TileWidth { get; private set; }

		/// <summary>
		/// Gets the height of a tile in the map.
		/// </summary>
		public int TileHeight { get; private set; }

		/// <summary>
		/// Gets a list of the map's properties.
		/// </summary>
		public PropertyCollection Properties { get; private set; }

		/// <summary>
		/// Gets a collection of all of the tiles in the map.
		/// </summary>
		public Dictionary<int, Tile> Tiles { get; private set; }

		/// <summary>
		/// Gets a collection of all of the layers in the map.
		/// </summary>
		public List<Layer> Layers { get; private set; }

		/// <summary>
		/// Gets a collection of all of the tile sets in the map.
		/// </summary>
		public List<TileSet> TileSets { get; private set; }

		/// <summary>
		/// Gets this map's Game Object Parent
		/// </summary>
		public GameObject Parent { get; private set; }

		/// <summary>
		/// Gets this map's Game Object
		/// </summary>
		public GameObject MapObject { get; private set; }

		/// <summary>
		/// Map's Tile Layers' initial Sorting Order
		/// </summary>
		public int DefaultSortingOrder = 0;

		private string _mapName = "Map";

		public Map(string mapXML, string MapName, string mapPath, bool makeUnique, GameObject parent, Material baseTileMaterial, int sortingOrder)
		{
			XDocument document = XDocument.Parse(mapXML);

			_mapName = MapName;

			Parent = parent;

			Initialize(document, makeUnique, "", mapPath, baseTileMaterial, sortingOrder);
		}

		public Map(TextAsset mapText, bool makeUnique, string mapPath, GameObject parent, Material baseTileMaterial, int sortingOrder)
		{
			XDocument document = XDocument.Parse(mapText.text);

			_mapName = mapText.name;

			Parent = parent;

			Initialize(document, makeUnique, "", mapPath, baseTileMaterial, sortingOrder);
		}

		private void Initialize(XDocument document, bool makeUnique, string fullPath, string mapPath, Material baseTileMaterial, int sortingOrder)
		{
			XElement mapNode = document.Root;
			Version = mapNode.Attribute("version").Value;
			Orientation = (Orientation)Enum.Parse(typeof(Orientation), mapNode.Attribute("orientation").Value, true);
			Width = int.Parse(mapNode.Attribute("width").Value, CultureInfo.InvariantCulture);
			Height = int.Parse(mapNode.Attribute("height").Value, CultureInfo.InvariantCulture);
			TileWidth = int.Parse(mapNode.Attribute("tilewidth").Value, CultureInfo.InvariantCulture);
			TileHeight = int.Parse(mapNode.Attribute("tileheight").Value, CultureInfo.InvariantCulture);

			if (_mapName == null)
				_mapName = "Map";

			if (!mapPath.EndsWith("/"))
				mapPath = mapPath + "/";

			MapObject = new GameObject(_mapName);
			MapObject.transform.parent = Parent.transform;
			MapObject.transform.localPosition = Vector3.zero;

			DefaultSortingOrder = sortingOrder;

			XElement propertiesElement = mapNode.Element("properties");
			if (propertiesElement != null)
				Properties = new PropertyCollection(propertiesElement);

			TileSets = new List<TileSet>();
			Tiles = new Dictionary<int, Tile>();
			foreach (XElement tileSet in mapNode.Descendants("tileset"))
			{
				if (tileSet.Attribute("source") != null)
				{
					TextAsset externalTileSetTextAsset = (TextAsset)Resources.Load(mapPath + Path.GetFileNameWithoutExtension(tileSet.Attribute("source").Value));

					XDocument externalTileSet = XDocument.Parse(externalTileSetTextAsset.text);

					XElement externalTileSetNode = externalTileSet.Element("tileset");
					
					TileSet t = new TileSet(externalTileSetNode, mapPath);
					TileSets.Add(t);
					foreach (KeyValuePair<int, Tile> item in t.Tiles)
					{
						this.Tiles.Add(item.Key, item.Value);
					}
				}
				else
				{
					TileSet t = new TileSet(tileSet, mapPath);
					TileSets.Add(t);
					foreach (KeyValuePair<int, Tile> item in t.Tiles)
					{
						this.Tiles.Add(item.Key, item.Value);
					}
				}
			}
			// Generate Materials for Map batching
			List<Material> materials = new List<Material>();
			// Generate Materials
			int i = 0;
			for (i = 0; i < TileSets.Count; i++)
			{
				Material layerMat = new Material(baseTileMaterial);
				layerMat.mainTexture = TileSets[i].Texture;
				materials.Add(layerMat);
			}

			Layers = new List<Layer>();
			i = 0;
			
			foreach (XElement layerNode in 
				mapNode.Elements("layer").Concat(
				mapNode.Elements("objectgroup").Concat(
				mapNode.Elements("imagelayer")))
				)
			{
				Layer layerContent;

				int layerDepth = 1 - (LayerDepthSpacing * i);

				if (layerNode.Name == "layer")
				{
					layerContent = new TileLayer(layerNode, this, layerDepth, makeUnique, materials);
				}
				else if (layerNode.Name == "objectgroup")
				{
					layerContent = new MapObjectLayer(layerNode, this, layerDepth, materials);
				}
				else if (layerNode.Name == "imagelayer")
				{
					layerContent = new ImageLayer(layerNode, this, mapPath, baseTileMaterial);
				}
				else
				{
					throw new Exception("Unknown layer name: " + layerNode.Name);
				}

				// Layer names need to be unique for our lookup system, but Tiled
				// doesn't require unique names.
				string layerName = layerContent.Name;
				int duplicateCount = 2;

				// if a layer already has the same name...
				if (Layers.Find(l => l.Name == layerName) != null)
				{
					// figure out a layer name that does work
					do
					{
						layerName = string.Format("{0}{1}", layerContent.Name, duplicateCount);
						duplicateCount++;
					} while (Layers.Find(l => l.Name == layerName) != null);

					// log a warning for the user to see
					Debug.Log("Renaming layer \"" + layerContent.Name + "\" to \"" + layerName + "\" to make a unique name.");

					// save that name
					layerContent.Name = layerName;
				}
				layerContent.LayerDepth = layerDepth;
				Layers.Add(layerContent);
				namedLayers.Add(layerName, layerContent);
				i++;
			}
		}

		/// <summary>
		/// Converts a point in world space into tiled space.
		/// </summary>
		/// <param name="worldPoint">The point in world space to convert into tiled space.</param>
		/// <returns>The point in Tiled space.</returns>
		public Vector2 WorldPointToTiledPosition(Vector2 worldPoint)
		{
			Vector2 p = new Vector2();

			if (Orientation == X_UniTMX.Orientation.Orthogonal)
			{
				// simple conversion to tile indices
				p.x = worldPoint.x;
				p.y = -worldPoint.y;
			}
			else if (Orientation == X_UniTMX.Orientation.Isometric)
			{
				float ratio = TileHeight / (float)TileWidth;
				// for some easier calculations, convert wordPoint to pixels
				Vector2 point = new Vector2(worldPoint.x * TileWidth, -worldPoint.y / ratio * TileHeight);

				// Code almost straight from Tiled's libtiled :P

				point.x -= Height * TileWidth / 2.0f;
				float tileX = point.x / (float)TileWidth;
				float tileY = point.y / (float)TileHeight;

				p.x = tileY + tileX;
				p.y = tileY - tileX;
			}
			else if (Orientation == X_UniTMX.Orientation.Staggered)
			{
				float ratio = TileHeight / (float)TileWidth;
				// for some easier calculations, convert wordPoint to pixels
				Vector2 point = new Vector2(worldPoint.x * (float)TileWidth, -worldPoint.y / ratio * (float)TileHeight);

				float halfTileHeight = TileHeight / 2.0f;

				// Code almost straight from Tiled's libtiled :P

				// Getting grid-aligned tile index
				float tileX = point.x / (float)TileWidth;
				float tileY = point.y / (float)TileHeight * 2;

				// Relative x and y pos to tile
				float relX = point.x - tileX * (float)TileWidth;
				float relY = point.y - tileY / 2.0f * (float)TileHeight;

				if (halfTileHeight - relX * ratio > relY)
				{
					p.y = tileY - 1;
					if (tileY % 2 > 0)
						p.x = tileX;
					else
						p.x = tileX - 1;
				}
				else if (-halfTileHeight + relX * ratio > relY)
				{
					p.y = tileY - 1;
					if (tileY % 2 > 0)
						p.x = tileX + 1;
					else
						p.x = tileX;
				}
				else if (halfTileHeight + relX * ratio < relY)
				{
					p.y = tileY + 1;
					if (tileY % 2 > 0)
						p.x = tileX;
					else
						p.x = tileX - 1;
				}
				else if (halfTileHeight * 3 - relX * ratio < relY)
				{
					p.y = tileY + 1;
					if (tileY % 2 > 0)
						p.x = tileX + 1;
					else
						p.x = tileX;
				}
				else
				{
					p.x = tileX;
					p.y = tileY;
				}
			}

			return p;
		}

		/// <summary>
		/// Converts a point in world space into tile indices that can be used to index into a TileLayer.
		/// </summary>
		/// <param name="worldPoint">The point in world space to convert into tile indices.</param>
		/// <returns>A Point containing the X/Y indices of the tile that contains the point.</returns>
		public Vector2 WorldPointToTileIndex(Vector2 worldPoint)
		{
			Vector2 p = new Vector2();
			
			if (Orientation == X_UniTMX.Orientation.Orthogonal)
			{
				// simple conversion to tile indices
				p.x = worldPoint.x;
				p.y = -worldPoint.y;
			}
			else if (Orientation == X_UniTMX.Orientation.Isometric)
			{
				float ratio = TileHeight / (float)TileWidth;
				// for some easier calculations, convert wordPoint to pixels
				Vector2 point = new Vector2(worldPoint.x * TileWidth, -worldPoint.y / ratio * TileHeight);
				
				// Code almost straight from Tiled's libtiled :P

				point.x -= Height * TileWidth / 2.0f;
				float tileX = point.x / TileWidth;
				float tileY = point.y / TileHeight;

				p.x = tileY + tileX;
				p.y = tileY - tileX;
			}
			else if (Orientation == X_UniTMX.Orientation.Staggered)
			{
				float ratio = TileHeight / (float)TileWidth;
				// for some easier calculations, convert wordPoint to pixels
				Vector2 point = new Vector2(worldPoint.x * TileWidth, -worldPoint.y / ratio * TileHeight);
				
				float halfTileHeight = TileHeight / 2.0f;

				// Code almost straight from Tiled's libtiled :P

				// Getting grid-aligned tile index
				int tileX = Mathf.FloorToInt(point.x / TileWidth);
				int tileY = Mathf.FloorToInt(point.y / TileHeight) * 2;

				// Relative x and y pos to tile
				float relX = point.x - tileX * TileWidth;
				float relY = point.y - tileY / 2.0f * TileHeight;

				if (halfTileHeight - relX * ratio > relY)
				{
					p.y = tileY - 1;
					if (tileY % 2 > 0)
						p.x = tileX;
					else
						p.x = tileX - 1;
				}
				else if (-halfTileHeight + relX * ratio > relY)
				{
					p.y = tileY - 1;
					if (tileY % 2 > 0)
						p.x = tileX + 1;
					else
						p.x = tileX;
				}
				else if (halfTileHeight + relX * ratio < relY)
				{
					p.y = tileY + 1;
					if (tileY % 2 > 0)
						p.x = tileX;
					else
						p.x = tileX - 1;
				}
				else if (halfTileHeight * 3 - relX * ratio < relY)
				{
					p.y = tileY + 1;
					if (tileY % 2 > 0)
						p.x = tileX + 1;
					else
						p.x = tileX;
				}
				else
				{
					p.x = tileX;
					p.y = tileY;
				}
			}

			p.x = Mathf.FloorToInt(p.x);
			p.y = Mathf.FloorToInt(p.y);

			return p;
		}

		/// <summary>
		/// Converts a tile index or position into world coordinates
		/// </summary>
		/// <param name="posX">Tile index or position of object in tiled</param>
		/// <param name="posY">Tile index or position of object in tiled</param>
		/// <param name="tile">Tile to get size from</param>
		/// <returns>World's X and Y position</returns>
		public Vector2 TiledPositionToWorldPoint(float posX, float posY, Tile tile = null)
		{
			Vector2 p = Vector2.zero;
			float currentTileWidth = TileWidth;
			float currentTileHeight = TileHeight;
			if (tile == null)
			{
				Dictionary<int, Tile>.ValueCollection.Enumerator enumerator = Tiles.Values.GetEnumerator();
				enumerator.MoveNext();
				currentTileWidth = enumerator.Current.TileSet.TileWidth;
				currentTileHeight = enumerator.Current.TileSet.TileHeight;
			}
			else
			{
				currentTileWidth = tile.TileSet.TileWidth;
				currentTileHeight = tile.TileSet.TileHeight;
			}

			if (Orientation == Orientation.Orthogonal)
			{
				p.x = posX * (TileWidth / currentTileWidth);
				p.y = -posY * (TileHeight / currentTileHeight) * (currentTileHeight / currentTileWidth);
			}
			else if (Orientation == Orientation.Isometric)
			{
				p.x = (TileWidth / 2.0f * (Width - posY + posX)) / (float)TileWidth;//(TileWidth / 2.0f * (Width / 2.0f - posY + posX)) / (float)TileWidth;//
				p.y = -Height + TileHeight * (Height - ((posX + posY) / (TileWidth / (float)TileHeight)) / 2.0f) / (float)TileHeight;				
			}
			else if (Orientation == X_UniTMX.Orientation.Staggered)
			{
				p.x = posX * (TileWidth / currentTileWidth);
				if (Mathf.FloorToInt(Mathf.Abs(posY)) % 2 > 0)
					p.x += 0.5f;
				p.y = -posY * (TileHeight / 2.0f / currentTileHeight) * (currentTileHeight / currentTileWidth);
			}

			return p;
		}

		/// <summary>
		/// Converts a tile index or position into 3D world coordinates
		/// </summary>
		/// <param name="posX">Tile index or position of object in tiled</param>
		/// <param name="posY">Tile index or position of object in tiled</param>
		/// <param name="posZ">zIndex of object</param>
		/// <param name="tile">Tile to get size from</param>
		/// <returns>World's X, Y and Z position</returns>
		public Vector3 TiledPositionToWorldPoint(float posX, float posY, float posZ, Tile tile = null)
		{
			Vector3 p = new Vector3();

			Vector2 p2d = TiledPositionToWorldPoint(posX, posY, tile);
			// No need to change Z value, this function is just a helper
			p.x = p2d.x;
			p.y = p2d.y;
			p.z = posZ;
			return p;
		}

		public Vector2 TiledPositionToWorldPoint(Vector2 position, Tile tile = null)
		{
			return TiledPositionToWorldPoint(position.x, position.y, tile);
		}

		/// <summary>
		/// Returns the set of all objects in the map.
		/// </summary>
		/// <returns>A new set of all objects in the map.</returns>
		public IEnumerable<MapObject> GetAllObjects()
		{
			foreach (var layer in Layers)
			{
				MapObjectLayer objLayer = layer as MapObjectLayer;
				if (objLayer == null)
					continue;

				foreach (var obj in objLayer.Objects)
				{
					yield return obj;
				}
			}
		}

		/// <summary>
		/// Finds an object in the map using a delegate.
		/// </summary>
		/// <remarks>
		/// This method is used when an object is desired, but there is no specific
		/// layer to find the object on. The delegate allows the caller to create 
		/// any logic they want for finding the object. A simple example for finding
		/// the first object named "goal" in any layer would be this:
		/// 
		/// var goal = map.FindObject((layer, obj) => return obj.Name.Equals("goal"));
		/// 
		/// You could also use the layer name or any other logic to find an object.
		/// The first object for which the delegate returns true is the object returned
		/// to the caller. If the delegate never returns true, the method returns null.
		/// </remarks>
		/// <param name="finder">The delegate used to search for the object.</param>
		/// <returns>The MapObject if the delegate returned true, null otherwise.</returns>
		public MapObject FindObject(MapObjectFinder finder)
		{
			foreach (var layer in Layers)
			{
				MapObjectLayer objLayer = layer as MapObjectLayer;
				if (objLayer == null)
					continue;

				foreach (var obj in objLayer.Objects)
				{
					if (finder(objLayer, obj))
						return obj;
				}
			}

			return null;
		}

		/// <summary>
		/// Finds a collection of objects in the map using a delegate.
		/// </summary>
		/// <remarks>
		/// This method performs basically the same process as FindObject, but instead
		/// of returning the first object for which the delegate returns true, it returns
		/// a collection of all objects for which the delegate returns true.
		/// </remarks>
		/// <param name="finder">The delegate used to search for the object.</param>
		/// <returns>A collection of all MapObjects for which the delegate returned true.</returns>
		public IEnumerable<MapObject> FindObjects(MapObjectFinder finder)
		{
			foreach (var layer in Layers)
			{
				MapObjectLayer objLayer = layer as MapObjectLayer;
				if (objLayer == null)
					continue;

				foreach (var obj in objLayer.Objects)
				{
					if (finder(objLayer, obj))
						yield return obj;
				}
			}
		}

		/// <summary>
		/// Gets a layer by name.
		/// </summary>
		/// <param name="name">The name of the layer to retrieve.</param>
		/// <returns>The layer with the given name.</returns>
		public Layer GetLayer(string name)
		{
			if (namedLayers.ContainsKey(name))
				return namedLayers[name];
			return null;
		}

		/// <summary>
		/// Gets a tile layer by name.
		/// </summary>
		/// <param name="name">The name of the tile layer to retrieve.</param>
		/// <returns>The tile layer with the given name.</returns>
		public TileLayer GetTileLayer(string name)
		{
			if (namedLayers.ContainsKey(name))
				return namedLayers[name] as TileLayer;
			return null;
		}

		/// <summary>
		/// Gets an object layer by name.
		/// </summary>
		/// <param name="name">The name of the object layer to retrieve.</param>
		/// <returns>The object layer with the given name.</returns>
		public MapObjectLayer GetObjectLayer(string name)
		{
			if (namedLayers.ContainsKey(name))
				return namedLayers[name] as MapObjectLayer;
			return null;
		}

		/// <summary>
		/// Gets a string property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property, String.Empty if property not found</returns>
		public string GetPropertyAsString(string property)
		{
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return str;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			return str;
		}
		/// <summary>
		/// Gets a boolean property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public bool GetPropertyAsBoolean(string property)
		{
			bool b = false;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			Boolean.TryParse(str, out b);

			return b;
		}
		/// <summary>
		/// Gets an integer property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public int GetPropertyAsInt(string property)
		{
			int b = 0;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			Int32.TryParse(str, out b);

			return b;
		}
		/// <summary>
		/// Gets a float property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public float GetPropertyAsFloat(string property)
		{
			float b = 0;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			float.TryParse(str, out b);

			return b;
		}

		private GameObject GenerateBoxCollider3D(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider;
			// Orthogonal and Staggered maps can use BoxCollider, Isometric maps must use polygon collider
			if (Orientation != X_UniTMX.Orientation.Isometric)
			{
				newCollider = new GameObject(obj.Name);
				newCollider.transform.parent = MapObject.transform;
				
				BoxCollider bx = newCollider.AddComponent<BoxCollider>();
				bx.isTrigger = isTrigger || obj.Type.Equals("Trigger");

				newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
				bx.center = new Vector3(obj.Bounds.width / 2.0f, -obj.Bounds.height / 2.0f);

				bx.size = new Vector3(obj.Bounds.width, obj.Bounds.height, colliderWidth);

				newCollider.isStatic = true;
				newCollider.SetActive(obj.Visible);
			}
			else
			{
				List<Vector2> points = new List<Vector2>();
				points.Add(new Vector2(obj.Bounds.xMin - obj.Bounds.x, obj.Bounds.yMax - obj.Bounds.y));
				points.Add(new Vector2(obj.Bounds.xMin - obj.Bounds.x, obj.Bounds.yMin - obj.Bounds.y));
				points.Add(new Vector2(obj.Bounds.xMax - obj.Bounds.x, obj.Bounds.yMin - obj.Bounds.y));
				points.Add(new Vector2(obj.Bounds.xMax - obj.Bounds.x, obj.Bounds.yMax - obj.Bounds.y));
				X_UniTMX.MapObject isoBox = new MapObject(obj.Name, obj.Type, obj.Bounds, obj.Properties, obj.GID, points, obj.Rotation);

				newCollider = GeneratePolygonCollider3D(isoBox, isTrigger, zDepth, colliderWidth);
			}

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody>();
				newCollider.rigidbody.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		private GameObject GenerateBoxCollider2D(MapObject obj, bool isTrigger = false, float zDepth = 0, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.parent = MapObject.transform;
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			// Orthogonal and Staggered maps can use BoxCollider, Isometric maps must use polygon collider
			if (Orientation != X_UniTMX.Orientation.Isometric)
			{
				BoxCollider2D bx = newCollider.AddComponent<BoxCollider2D>();
				bx.isTrigger = isTrigger || obj.Type.Equals("Trigger");

				bx.center = new Vector2(obj.Bounds.width / 2.0f, -obj.Bounds.height / 2.0f);
				bx.size = new Vector2(obj.Bounds.width, obj.Bounds.height);
			}
			else if(Orientation == X_UniTMX.Orientation.Isometric)
			{
				PolygonCollider2D pc = newCollider.AddComponent<PolygonCollider2D>();
				pc.isTrigger = isTrigger || obj.Type.Equals("Trigger");
				Vector2[] points = new Vector2[4];
				points[0] = TiledPositionToWorldPoint(0, 0);
				points[1] = TiledPositionToWorldPoint(0, obj.Bounds.height);
				points[2] = TiledPositionToWorldPoint(obj.Bounds.width, obj.Bounds.height);
				points[3] = TiledPositionToWorldPoint(obj.Bounds.width, 0);
				points[0].x -= Width / 2.0f;
				points[1].x -= Width / 2.0f;
				points[2].x -= Width / 2.0f;
				points[3].x -= Width / 2.0f;
				pc.SetPath(0, points);

			}


			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody2D>();
				newCollider.rigidbody2D.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
			
		}
		/// <summary>
		/// Generate a Box collider mesh for 3D, or a BoxCollider2D for 2D (a PolygonCollider2D will be created for Isometric maps).
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate this collider.</param>
		/// <param name="isTrigger">True for Trigger Collider, false otherwise</param>
		/// <param name="zDepth">Z Depth of the collider.</param>
		/// <param name="colliderWidth">Width of the collider, in Units</param>
		/// <param name="used2DColider">True to generate a 2D collider, false to generate a 3D collider.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public GameObject GenerateBoxCollider(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool used2DColider = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			return used2DColider ? GenerateBoxCollider2D(obj, isTrigger, zDepth, createRigidbody, rigidbodyIsKinematic) : GenerateBoxCollider3D(obj, isTrigger, zDepth, colliderWidth, createRigidbody, rigidbodyIsKinematic);
		}

		private void ApproximateEllipse2D(GameObject newCollider, MapObject obj, bool isTrigger = false, float zDepth = 0, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			// since there's no "EllipseCollider2D", we must create one by approximating a polygon collider
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			
			PolygonCollider2D polygonCollider = newCollider.AddComponent<PolygonCollider2D>();

			polygonCollider.isTrigger = isTrigger || obj.Type.Equals("Trigger");

			int segments = 16; // Increase this for higher-quality ellipsoides.
			//float increment = 2 * Mathf.PI / segments;

			// Segments per quadrant
			int incFactor = Mathf.FloorToInt(segments / 4.0f);
			float minIncrement = 2 * Mathf.PI / (incFactor * segments / 2.0f);
			int currentInc = 0;
			bool grow = true;

			Vector2[] points = new Vector2[segments];

			Vector2 center = new Vector2(obj.Bounds.width / 2.0f, obj.Bounds.height / 2.0f);

			float r = 0;
			float angle = 0;
			for (int i = 0; i < segments; i++)
			{
				// Calculate radius at each point
				angle += currentInc * minIncrement;

				r = obj.Bounds.width * obj.Bounds.height / Mathf.Sqrt(Mathf.Pow(obj.Bounds.height * Mathf.Cos(angle), 2) + Mathf.Pow(obj.Bounds.width * Mathf.Sin(angle), 2)) / 2.0f;

				points[i] = r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + center;

				points[i] = TiledPositionToWorldPoint(points[i].x, points[i].y);

				// Offset points where needed
				if (Orientation == X_UniTMX.Orientation.Isometric)
					points[i].x -= Width / 2.0f;
				if (Orientation == X_UniTMX.Orientation.Staggered)
					points[i].y *= TileWidth / (float)TileHeight * 2.0f;

				if (grow)
					currentInc++;
				else
					currentInc--;
				if (currentInc > incFactor - 1 || currentInc < 1)
					grow = !grow;

				// POG :P
				if (Orientation != X_UniTMX.Orientation.Isometric)
				{
					if (i < 1 || i == segments / 2 - 1)
						points[i].y -= obj.Bounds.height / 20.0f;
					if (i >= segments - 1 || i == segments / 2)
						points[i].y += obj.Bounds.height / 20.0f;
				}
			}

			polygonCollider.SetPath(0, points);
		}

		private void ApproximateEllipse3D(GameObject newCollider, MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			// since there's no "EllipseCollider", we must create one by approximating a polygon collider
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);

			Mesh colliderMesh = new Mesh();
			colliderMesh.name = "Collider_" + obj.Name;
			MeshCollider mc = newCollider.AddComponent<MeshCollider>();
			mc.isTrigger = isTrigger || obj.Type.Equals("Trigger");

			int segments = 16; // Increase this for higher-quality ellipsoides. Please note that 3D colliders doubles this value, as they needs front and back vertices.
			//float increment = 2 * Mathf.PI / segments;

			// Segments per quadrant
			int incFactor = Mathf.FloorToInt(segments / 4.0f);
			float minIncrement = 2 * Mathf.PI / (incFactor * segments / 2.0f);
			int currentInc = 0;
			bool grow = true;

			Vector2[] points = new Vector2[segments];

			float width = obj.Bounds.width;
			float height = obj.Bounds.height;

			Vector2 center = new Vector2(width / 2.0f, height / 2.0f);
			
			float r = 0;
			float angle = 0;
			for (int i = 0; i < segments; i++)
			{
				// Calculate radius at each point
				//angle = i * increment;
				angle += currentInc * minIncrement;
				r = width * height / Mathf.Sqrt(Mathf.Pow(height * Mathf.Cos(angle), 2) + Mathf.Pow(width * Mathf.Sin(angle), 2)) / 2.0f;
				points[i] = r * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) + center;
				if (Orientation == X_UniTMX.Orientation.Staggered)
					points[i].y *= -1;

				if(grow)
					currentInc++;
				else
					currentInc--;
				if (currentInc > incFactor - 1 || currentInc < 1)
					grow = !grow;

				// POG :P
				if (Orientation != X_UniTMX.Orientation.Isometric)
				{
					if (i < 1 || i == segments / 2 - 1)
						points[i].y -= obj.Bounds.height / 20.0f;
					if (i >= segments - 1 || i == segments / 2)
						points[i].y += obj.Bounds.height / 20.0f;
				}
			}

			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();

			GenerateVerticesAndTris(new List<Vector2>(points), vertices, triangles, zDepth, colliderWidth, false, !(Orientation == X_UniTMX.Orientation.Staggered));

			// Connect last point with first point (create the face between them)
			triangles.Add(vertices.Count - 1);
			triangles.Add(1);
			triangles.Add(0);

			triangles.Add(0);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 1);

			colliderMesh.vertices = vertices.ToArray();
			colliderMesh.triangles = triangles.ToArray();
			colliderMesh.RecalculateNormals();

			mc.sharedMesh = colliderMesh;
		}

		private GameObject GenerateEllipseCollider3D(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			newCollider.transform.parent = MapObject.transform;

			if (Orientation != X_UniTMX.Orientation.Isometric && obj.Bounds.width == obj.Bounds.height)
			{
				CapsuleCollider cc = newCollider.AddComponent<CapsuleCollider>();

				cc.isTrigger = isTrigger || obj.Type.Equals("Trigger");

				cc.center = new Vector3(obj.Bounds.height / 2.0f, -obj.Bounds.width / 2.0f);

				cc.direction = 0;
				cc.radius = obj.Bounds.height / 2.0f;
				cc.height = obj.Bounds.width;
			}
			else
			{
				ApproximateEllipse3D(newCollider, obj, isTrigger, zDepth, colliderWidth, createRigidbody, rigidbodyIsKinematic);
			}

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody>();
				newCollider.rigidbody.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		private GameObject GenerateEllipseCollider2D(MapObject obj, bool isTrigger = false, float zDepth = 0, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.parent = MapObject.transform;
			if (Orientation != X_UniTMX.Orientation.Isometric && obj.Bounds.width == obj.Bounds.height)
			{
				CircleCollider2D cc = newCollider.AddComponent<CircleCollider2D>();
				cc.isTrigger = isTrigger || obj.Type.Equals("Trigger");

				newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
				cc.center = new Vector2(obj.Bounds.width / 2.0f, -obj.Bounds.height / 2.0f);

				cc.radius = obj.Bounds.width / 2.0f;
				
			}
			else
			{
				ApproximateEllipse2D(newCollider, obj, isTrigger, zDepth, createRigidbody, rigidbodyIsKinematic);
			}

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody2D>();
				newCollider.rigidbody2D.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		/// <summary>
		/// Generate an Ellipse Collider mesh.
		/// To mimic Tiled's Ellipse Object properties, a Capsule collider is created.
		/// For 2D, a CircleCollider2D will be created if ellipse is a circle, else a PolygonCollider wiil be approximated to an ellipsoid, for 3D, the capsule collider will have same width of the ellipse, but won't have an ellipsoid format...
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate this collider.</param>
		/// <param name="isTrigger">True for Trigger Collider, false otherwise</param>
		/// <param name="zDepth">Z Depth of the collider.</param>
		/// <param name="colliderWidth">Width of the collider, in Units</param>
		/// <param name="used2DColider">True to generate a 2D collider, false to generate a 3D collider.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public GameObject GenerateEllipseCollider(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool used2DColider = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			return used2DColider ? GenerateEllipseCollider2D(obj, isTrigger, zDepth, createRigidbody, rigidbodyIsKinematic) : GenerateEllipseCollider3D(obj, isTrigger, zDepth, colliderWidth, createRigidbody, rigidbodyIsKinematic);
		}

		private void GenerateVerticesAndTris(List<Vector2> points, List<Vector3> generatedVertices, List<int> generatedTriangles, float zDepth = 0, float colliderWidth = 1.0f, bool innerCollision = false, bool calculateWorldPos = true)
		{
			Vector3 firstPoint = (Vector3)points[0];
			Vector3 secondPoint, firstFront, firstBack, secondFront, secondBack;
			// Create and Add first points
			if (calculateWorldPos)
			{
				firstFront = TiledPositionToWorldPoint(firstPoint.x, firstPoint.y, zDepth - colliderWidth / 2.0f);
				firstBack = TiledPositionToWorldPoint(firstPoint.x, firstPoint.y, zDepth + colliderWidth / 2.0f);
			}
			else
			{
				firstFront = new Vector3(firstPoint.x, firstPoint.y, zDepth - colliderWidth / 2.0f);
				firstBack = new Vector3(firstPoint.x, firstPoint.y, zDepth + colliderWidth / 2.0f);
			}
			if (Orientation == X_UniTMX.Orientation.Isometric)
			{
				firstFront.x -= Width / 2.0f;
				firstBack.x -= Width / 2.0f;
			}
			if (innerCollision)
			{
				generatedVertices.Add(firstBack); // 3
				generatedVertices.Add(firstFront); // 2
			}
			else
			{
				generatedVertices.Add(firstFront); // 3
				generatedVertices.Add(firstBack); // 2
			}

			for (int i = 1; i < points.Count; i++)
			{
				secondPoint = (Vector3)points[i];
				if (calculateWorldPos)
				{
					secondFront = TiledPositionToWorldPoint(secondPoint.x, secondPoint.y, zDepth - colliderWidth / 2.0f);
					secondBack = TiledPositionToWorldPoint(secondPoint.x, secondPoint.y, zDepth + colliderWidth / 2.0f);
				}
				else
				{
					secondFront = new Vector3(secondPoint.x, secondPoint.y, zDepth - colliderWidth / 2.0f);
					secondBack = new Vector3(secondPoint.x, secondPoint.y, zDepth + colliderWidth / 2.0f);
				}

				if (Orientation == X_UniTMX.Orientation.Isometric)
				{
					secondFront.x -= Width / 2.0f;
					secondBack.x -= Width / 2.0f;
				}
				if (innerCollision)
				{
					generatedVertices.Add(secondBack); // 1
					generatedVertices.Add(secondFront); // 0
				}
				else
				{
					generatedVertices.Add(secondFront); // 1
					generatedVertices.Add(secondBack); // 0
				}

				generatedTriangles.Add((i - 1) * 2 + 3);
				generatedTriangles.Add((i - 1) * 2 + 2);
				generatedTriangles.Add((i - 1) * 2 + 0);

				generatedTriangles.Add((i - 1) * 2 + 0);
				generatedTriangles.Add((i - 1) * 2 + 1);
				generatedTriangles.Add((i - 1) * 2 + 3);

				firstPoint = secondPoint;
				firstFront = secondFront;
				firstBack = secondBack;
			}
		}

		private GameObject GeneratePolylineCollider3D(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool innerCollision = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			//newCollider.transform.position = MapObject.transform.position;
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			newCollider.transform.parent = MapObject.transform;

			Mesh colliderMesh = new Mesh();
			colliderMesh.name = "Collider_" + obj.Name;
			MeshCollider mc = newCollider.AddComponent<MeshCollider>();

			mc.isTrigger = isTrigger || obj.Type.Equals("Trigger");

			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();

			GenerateVerticesAndTris(obj.Points, vertices, triangles, zDepth, colliderWidth, innerCollision);
			
			colliderMesh.vertices = vertices.ToArray();
			colliderMesh.triangles = triangles.ToArray();
			colliderMesh.RecalculateNormals();

			mc.sharedMesh = colliderMesh;

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody>();
				newCollider.rigidbody.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		private GameObject GeneratePolylineCollider2D(MapObject obj, bool isTrigger = false, float zDepth = 0, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.parent = MapObject.transform;
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			//newCollider.transform.localPosition = TiledPositionToWorldPoint(0, 0, zDepth);

			EdgeCollider2D edgeCollider = newCollider.AddComponent<EdgeCollider2D>();

			edgeCollider.isTrigger = isTrigger || obj.Type.Equals("Trigger");
			
			Vector2[] points = obj.Points.ToArray();
			
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = TiledPositionToWorldPoint(points[i].x, points[i].y);
				if (Orientation == X_UniTMX.Orientation.Isometric)
					points[i].x -= Width / 2.0f;
				//points[x].y -= obj.Bounds.y;
			}

			edgeCollider.points = points;

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody2D>();
				newCollider.rigidbody2D.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		/// <summary>
		/// Generate a Polyline collider mesh, or a sequence of EdgeCollider2D for 2D collisions.
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate this collider.</param>
		/// <param name="isTrigger">True for Trigger Collider, false otherwise</param>
		/// <param name="zDepth">Z Depth of the collider.</param>
		/// <param name="colliderWidth">Width of the collider, in Units</param>
		/// <param name="used2DColider">True to generate a 2D collider, false to generate a 3D collider.</param>
		/// <param name="innerCollision">If true, calculate normals facing the anchor of the collider (inside collisions), else, outside collisions.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public GameObject GeneratePolylineCollider(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool innerCollision = false, bool used2DColider = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			return used2DColider ? GeneratePolylineCollider2D(obj, isTrigger, zDepth, createRigidbody, rigidbodyIsKinematic) : GeneratePolylineCollider3D(obj, isTrigger, zDepth, colliderWidth, innerCollision, createRigidbody, rigidbodyIsKinematic);
		}

		private GameObject GeneratePolygonCollider2D(MapObject obj, bool isTrigger = false, float zDepth = 0, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.parent = MapObject.transform;
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);
			//newCollider.transform.localPosition = new Vector3(0, 0, zDepth);

			PolygonCollider2D polygonCollider = newCollider.AddComponent<PolygonCollider2D>();

			polygonCollider.isTrigger = isTrigger || obj.Type.Equals("Trigger");
			
			Vector2[] points = obj.Points.ToArray();

			for (int i = 0; i < points.Length; i++)
			{
				points[i] = TiledPositionToWorldPoint(points[i].x, points[i].y);
				if (Orientation == X_UniTMX.Orientation.Isometric)
					points[i].x -= Width / 2.0f;
			}

			polygonCollider.SetPath(0, points);

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);
			
			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody2D>();
				newCollider.rigidbody2D.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);

			return newCollider;
		}

		private void SweepSortVerticesList(List<Vector3> vertices, List<int> verticesIndexList)
		{
			bool swapped = false;
			int vertex = 0;
			for (int i = 0; i < verticesIndexList.Count - 1; i++)
			{
				swapped = false;
				for (int j = verticesIndexList.Count - 1; j > i; j--)
				{
					if (vertices[verticesIndexList[j]].x < vertices[verticesIndexList[j - 1]].x ||
						(vertices[verticesIndexList[j]].x == vertices[verticesIndexList[j - 1]].x &&
						vertices[verticesIndexList[j]].y < vertices[verticesIndexList[j - 1]].y))
					{
						vertex = verticesIndexList[j - 1];
						verticesIndexList[j - 1] = verticesIndexList[j];
						verticesIndexList[j] = vertex;
						swapped = true;
					}
				}
				if (!swapped)
					break;
			}
		}

		private GameObject GeneratePolygonCollider3D(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool innerCollision = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject newCollider = new GameObject(obj.Name);
			newCollider.transform.parent = MapObject.transform;
			newCollider.transform.localPosition = TiledPositionToWorldPoint(obj.Bounds.x, obj.Bounds.y, zDepth);

			Mesh colliderMesh = new Mesh();
			colliderMesh.name = "Collider_" + obj.Name;
			MeshCollider mc = newCollider.AddComponent<MeshCollider>();

			mc.isTrigger = isTrigger || obj.Type.Equals("Trigger");

			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();

			GenerateVerticesAndTris(obj.Points, vertices, triangles, zDepth, colliderWidth, innerCollision);

			// Connect last point with first point (create the face between them)
			triangles.Add(vertices.Count - 1);
			triangles.Add(1);
			triangles.Add(0);

			triangles.Add(0);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 1);

			////	Fill Faces
			////  Unfortunately I could'n come up with a good solution for both concave and convex polygons :/
			////  This code works for convex polygons, so, in case you need it, just uncomment it (and the other comments flagged as Fill Faces)
			//// Find leftmost vertex
			//List<int> sweepVerticesFront = new List<int>();
			//for (int x = 0; x < vertices.Count; x += 2)
			//{
			//	sweepVerticesFront.Add(x);
			//}
			//// Sort it by vertex X
			//SweepSortVerticesList(vertices, sweepVerticesFront);

			//List<int> L = new List<int>();
			//L.Add(sweepVerticesFront[0]);
			//L.Add(sweepVerticesFront[1]);
			//int vertex = 0;
			//int count = 2;
			//int oppositeVertex1 = 0;
			//int oppositeVertex2 = 0;
			//bool b_oppositeVertex1 = false;
			//bool b_oppositeVertex2 = false;
			//int indexOppositeVertex1 = 0;
			//int indexOppositeVertex2 = 0;
			//int[] cclockwise;

			//while (sweepVerticesFront.Count > count)
			//{
			//	vertex = sweepVerticesFront[count];
			//	// Is vertex opposite to any other vertex in L?
			//	b_oppositeVertex1 = false;
			//	b_oppositeVertex2 = false;
			//	oppositeVertex1 = vertex - 2;
			//	if (oppositeVertex1 < 0)
			//		oppositeVertex1 = vertices.Count - 2;
			//	oppositeVertex2 = vertex + 2;
			//	if (oppositeVertex2 > vertices.Count - 2)
			//		oppositeVertex2 = 0;
			//	indexOppositeVertex1 = 0;
			//	indexOppositeVertex2 = 0;
			//	for (int x = 0; x < L.Count; x++)
			//	{
			//		if (L[x] == oppositeVertex1)
			//		{
			//			b_oppositeVertex1 = true;
			//			indexOppositeVertex1 = x;
			//		}
			//		if (L[x] == oppositeVertex2)
			//		{
			//			b_oppositeVertex2 = true;
			//			indexOppositeVertex2 = x;
			//		}
			//	}
			//	if (b_oppositeVertex1 || b_oppositeVertex2)
			//	{
			//		while (L.Count > 1)
			//		{
			//			cclockwise = GetCounterClockwiseOrder(vertices, vertex, L[1], L[0]);
			//			triangles.Add(cclockwise[0]);
			//			triangles.Add(cclockwise[1]);
			//			triangles.Add(cclockwise[2]);

			//			cclockwise = GetCounterClockwiseOrder(vertices, vertex + 1, L[1] + 1, L[0] + 1);
			//			triangles.Add(cclockwise[2]);
			//			triangles.Add(cclockwise[1]);
			//			triangles.Add(cclockwise[0]);
						
			//			if(b_oppositeVertex1)
			//				L.RemoveAt(indexOppositeVertex1 > L.Count - 1 ? L.Count - 1 : indexOppositeVertex1);
			//			else
			//				L.RemoveAt(indexOppositeVertex2 > L.Count - 1 ? L.Count - 1 : indexOppositeVertex2);
			//		}
			//	}
			//	else
			//	{
			//		while (L.Count > 1 && (Vector3.Angle(vertices[L[L.Count - 1]], vertices[L[L.Count - 2]]) + Vector3.Angle(vertices[L[L.Count - 2]], vertices[vertex]) <= 180))
			//		{
			//			cclockwise = GetCounterClockwiseOrder(vertices, vertex, L[L.Count - 2], L[L.Count - 1]);
			//			triangles.Add(cclockwise[0]);
			//			triangles.Add(cclockwise[1]);
			//			triangles.Add(cclockwise[2]);

			//			cclockwise = GetCounterClockwiseOrder(vertices, vertex + 1, L[L.Count - 2] + 1, L[L.Count - 1] + 1);
			//			triangles.Add(cclockwise[2]);
			//			triangles.Add(cclockwise[1]);
			//			triangles.Add(cclockwise[0]);

			//			L.RemoveAt(L.Count - 1);
			//		}
			//	}
			//	L.Add(vertex);

			//	count++;
			//}

			colliderMesh.vertices = vertices.ToArray();
			colliderMesh.triangles = triangles.ToArray();
			colliderMesh.RecalculateNormals();

			mc.sharedMesh = colliderMesh;

			newCollider.isStatic = true;
			newCollider.SetActive(obj.Visible);

			if (createRigidbody)
			{
				newCollider.AddComponent<Rigidbody>();
				newCollider.rigidbody.isKinematic = rigidbodyIsKinematic;
			}

			if (obj.Rotation != 0)
			{
				newCollider.transform.localRotation = Quaternion.AngleAxis(obj.Rotation, Vector3.forward);
				Debug.Log("rotated!");
			}

			return newCollider;
		}
		/*	Fill Faces
		 * Unfortunately I could'n come up with a good solution for both concave and convex polygons :/
		 * This code works for convex polygons, so, in case you need it, just uncomment it (and the other comments flagged as Fill Faces)
		private int[] GetCounterClockwiseOrder(List<Vector3> vertices, int a, int b, int c)
		{
			int[] counterClockwise = new int[3];
			float crossAB = vertices[a].x * vertices[b].y - vertices[b].x * vertices[a].y;
			Debug.Log("crossAB: " + crossAB);
			float crossBC = vertices[b].x * vertices[c].y - vertices[c].x * vertices[b].y;
			Debug.Log("crossBC: " + crossBC);

			if (crossAB < 0)
			{
				counterClockwise[0] = a;
				if (crossBC < 0)
				{
					counterClockwise[2] = b;
					counterClockwise[1] = c;
				}
				else
				{
					counterClockwise[2] = c;
					counterClockwise[1] = b;
				}
			}
			else
			{
				counterClockwise[0] = b;
				counterClockwise[1] = a;
				counterClockwise[2] = c;
			}
			return counterClockwise;
		}
		*/
		/// <summary>
		/// Generate a Polygon collider mesh, or a PolygonCollider2D for 2D collisions.
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate this collider.</param>
		/// <param name="isTrigger">True for Trigger Collider, false otherwise</param>
		/// <param name="zDepth">Z Depth of the collider.</param>
		/// <param name="colliderWidth">Width of the collider, in Units</param>
		/// <param name="used2DColider">True to generate a 2D collider, false to generate a 3D collider.</param>
		/// <param name="innerCollision">If true, calculate normals facing the anchor of the collider (inside collisions), else, outside collisions.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public GameObject GeneratePolygonCollider(MapObject obj, bool isTrigger = false, float zDepth = 0, float colliderWidth = 1.0f, bool innerCollision = false, bool used2DColider = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			return used2DColider ? GeneratePolygonCollider2D(obj, isTrigger, zDepth, createRigidbody, rigidbodyIsKinematic) : GeneratePolygonCollider3D(obj, isTrigger, zDepth, colliderWidth, innerCollision, createRigidbody, rigidbodyIsKinematic);
		}

		/// <summary>
		/// Generate a collider based on object type
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate this collider.</param>
		/// <param name="used2DColider">True to generate 2D colliders, otherwise 3D colliders will be generated.</param>
		/// <param name="zDepth">Z Depth of the 3D collider.</param>
		/// <param name="colliderWidth">>Width of the 3D collider.</param>
		/// <param name="innerCollision">If true, calculate normals facing the anchor of the collider (inside collisions), else, outside collisions.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public GameObject GenerateCollider(MapObject obj, bool isTrigger = false, bool used2DColider = false, float zDepth = 0, float colliderWidth = 1, bool innerCollision = false, bool createRigidbody = false, bool rigidbodyIsKinematic = true)
		{
			GameObject col = null;

			switch (obj.MapObjectType)
			{
				case MapObjectType.Box:
					col = GenerateBoxCollider(obj, isTrigger, zDepth, colliderWidth, used2DColider, createRigidbody, rigidbodyIsKinematic);
					break;
				case MapObjectType.Ellipse:
					col = GenerateEllipseCollider(obj, isTrigger, zDepth, colliderWidth, used2DColider, createRigidbody, rigidbodyIsKinematic);
					break;
				case MapObjectType.Polygon:
					col = GeneratePolygonCollider(obj, isTrigger, zDepth, colliderWidth, innerCollision, used2DColider, createRigidbody, rigidbodyIsKinematic);
					break;
				case MapObjectType.Polyline:
					col = GeneratePolylineCollider(obj, isTrigger, zDepth, colliderWidth, innerCollision, used2DColider, createRigidbody, rigidbodyIsKinematic);
					break;
			}

			return col;
		}

		/// <summary>
		/// Generates Colliders from an MapObject Layer. Every Object in it will generate a GameObject with a Collider.
		/// </summary>
		/// <param name="objectLayerName">MapObject Layer's name</param>
		/// <param name="collidersAreTrigger">true to generate Trigger colliders, false otherwhise.</param>
		/// <param name="is2DCollider">true to generate 2D colliders, false for 3D colliders</param>
		/// <param name="collidersZDepth">Z position of the colliders</param>
		/// <param name="collidersWidth">Width for 3D colliders</param>
		/// <param name="collidersAreInner">true to generate inner collisions for 3D colliders</param>
		/// <returns></returns>
		public GameObject[] GenerateCollidersFromLayer(string objectLayerName, bool collidersAreTrigger = false, bool is2DCollider = true, float collidersZDepth = 0, float collidersWidth = 1, bool collidersAreInner = false)
		{
			List<GameObject> generatedGameObjects = new List<GameObject>();

			MapObjectLayer collisionLayer = GetObjectLayer(objectLayerName);
			if (collisionLayer != null)
			{
				List<MapObject> colliders = collisionLayer.Objects;
				foreach (MapObject colliderObjMap in colliders)
				{
					GameObject newColliderObject = null;
					if ("NoCollider".Equals(colliderObjMap.Type) == false)
					{
						newColliderObject = GenerateCollider(colliderObjMap, collidersAreTrigger, is2DCollider, collidersZDepth, collidersWidth, collidersAreInner);
					}

					if (colliderObjMap.GetPropertyAsBoolean("detach prefab"))
					{
						newColliderObject = null;
					}

					AddPrefabs(colliderObjMap, newColliderObject, is2DCollider);

					// if this colider has transfer, so delete this colider
					if (colliderObjMap.GetPropertyAsBoolean("transfer collider"))
					{
						if (is2DCollider)
						{
							GameObject.Destroy(newColliderObject.collider2D);
						}
						else
						{
							GameObject.Destroy(newColliderObject.collider);
						}
					}
					if(newColliderObject)
						generatedGameObjects.Add(newColliderObject);
				}
			}
			else
			{
				Debug.LogError("There's no Layer \"" + objectLayerName + "\" in tile map.");
			}

			return generatedGameObjects.ToArray();
		}

		/// <summary>
		/// Generate a prefab based in object colider layer
		/// </summary>
		/// <param name="obj">Object which properties will be used to generate a prefab.</param>
		/// <param name="newColliderObject">if null add relative parent object,.</param>
		/// <returns>Generated Game Object containing the Collider.</returns>
		public void AddPrefabs(MapObject obj, GameObject newColliderObject = null, bool is2DColliders = false, bool addTileName = true)
		{
			int indexPrefab = 0;
			while ("".Equals(obj.GetPropertyAsString(indexPrefab + "-prefab")) == false)
			{
				string prefabName = obj.GetPropertyAsString(indexPrefab + "-prefab");
				string baseResourcePath = obj.GetPropertyAsString(indexPrefab + "-prefab path");
				UnityEngine.Object resourceObject = Resources.Load(baseResourcePath + prefabName);
				Resources.UnloadUnusedAssets();
				if (resourceObject != null)
				{
					float zDepth = obj.GetPropertyAsFloat(indexPrefab + "-prefab z depth");
					GameObject newPrefab = UnityEngine.Object.Instantiate(resourceObject) as GameObject;
					//if (newColliderObject == null)
					//{
					//	newPrefab.transform.parent = MapObject.transform;
					//	newPrefab.transform.localPosition = new Vector3(obj.Bounds.center.x, -obj.Bounds.center.y, zDepth);
					//}
					//else
					//{
					//	newPrefab.transform.parent = newColliderObject.transform;
					//	newPrefab.transform.localPosition = new Vector3(0, 0, zDepth);
					//}
					newPrefab.transform.parent = MapObject.transform;
					newPrefab.transform.localPosition = new Vector3(obj.Bounds.center.x, -obj.Bounds.center.y, zDepth);

					// copy coliders from newColliderObject
					// only copy if type of this object is diferent of "NoCollider"
					if ("NoCollider".Equals(obj.Type) == false)
					{
						if (obj.GetPropertyAsBoolean(indexPrefab + "-prefab add collider"))
						{
							CopyCollider(obj, ref newColliderObject, ref newPrefab, is2DColliders);
						}
					}

					newPrefab.name = (addTileName ? (_mapName + "_") : "") + obj.Name;
					int indexMessage = 0;
					string prefabMensage = obj.GetPropertyAsString(indexPrefab + "-prefab sendmessage " + indexMessage);
					while ("".Equals(prefabMensage) == false)
					{
						string[] menssage = prefabMensage.Split(new[] { '|' }, StringSplitOptions.None);
						if (menssage.Length == 2)
						{
							newPrefab.BroadcastMessage(menssage[0], menssage[1]);
						}
						indexMessage++;
						prefabMensage = obj.GetPropertyAsString(indexPrefab + "-prefab sendmessage " + indexMessage);
					}

				}
				else
				{
					Debug.LogError("Prefab doesn't exist at: Resources/" + baseResourcePath + prefabName);
				}
				indexPrefab++;
			}
		}

		private void CopyCollider(MapObject obj, ref GameObject origin, ref GameObject destination, bool is2DColliders)
		{
			switch (obj.MapObjectType)
			{
				case MapObjectType.Box:
					if (is2DColliders)
					{
						BoxCollider2D c = destination.AddComponent<BoxCollider2D>();
						c.isTrigger = ((BoxCollider2D)origin.collider2D).isTrigger;
						c.size = ((BoxCollider2D)origin.collider2D).size;
						c.center = ((BoxCollider2D)origin.collider2D).center;
					}
					else
					{
						BoxCollider box = destination.AddComponent<BoxCollider>();
						box.size = ((BoxCollider)origin.collider).size;
						box.center = ((BoxCollider)origin.collider).center;
					}
					break;
				case MapObjectType.Ellipse:
					if (is2DColliders)
					{
						CircleCollider2D c = destination.AddComponent<CircleCollider2D>();
						c.isTrigger = ((CircleCollider2D)origin.collider2D).isTrigger;
						c.center = ((CircleCollider2D)origin.collider2D).center;
						c.radius = ((CircleCollider2D)origin.collider2D).radius;
					}
					else
					{
						CapsuleCollider capsule = destination.AddComponent<CapsuleCollider>();
						capsule.isTrigger = ((CapsuleCollider)origin.collider).isTrigger;
						capsule.height = ((CapsuleCollider)origin.collider).height;
						capsule.radius = ((CapsuleCollider)origin.collider).radius;
						capsule.center = ((CapsuleCollider)origin.collider).center;
						capsule.direction = ((CapsuleCollider)origin.collider).direction;
					}
					break;
				case MapObjectType.Polygon:
					if (is2DColliders)
					{
						PolygonCollider2D c = destination.AddComponent<PolygonCollider2D>();
						c.isTrigger = ((PolygonCollider2D)origin.collider2D).isTrigger;
						for (int i = 0; i < ((PolygonCollider2D)origin.collider2D).pathCount; i++)
						{
							c.SetPath(i, ((PolygonCollider2D)origin.collider2D).GetPath(i));
						}
					}
					else
					{
						MeshCollider mc = destination.AddComponent<MeshCollider>();
						mc.isTrigger = ((MeshCollider)origin.collider).isTrigger;
						mc.convex = ((MeshCollider)origin.collider).convex;
						mc.smoothSphereCollisions = ((MeshCollider)origin.collider).smoothSphereCollisions;
						mc.sharedMesh = ((MeshCollider)origin.collider).sharedMesh;
						mc.sharedMesh.RecalculateBounds();
						mc.sharedMesh.RecalculateNormals();
					}
					break;
				case MapObjectType.Polyline:
					if (is2DColliders)
					{
						EdgeCollider2D c = destination.AddComponent<EdgeCollider2D>();
						c.isTrigger = ((EdgeCollider2D)origin.collider2D).isTrigger;
						c.points = ((EdgeCollider2D)origin.collider2D).points;
					}
					else
					{
						MeshCollider mc = destination.AddComponent<MeshCollider>();
						mc.isTrigger = ((MeshCollider)origin.collider).isTrigger;
						mc.convex = ((MeshCollider)origin.collider).convex;
						mc.smoothSphereCollisions = ((MeshCollider)origin.collider).smoothSphereCollisions;
						mc.sharedMesh = ((MeshCollider)origin.collider).sharedMesh;
						mc.sharedMesh.RecalculateBounds();
						mc.sharedMesh.RecalculateNormals();
					}
					break;
			}

		}

		public override string ToString()
		{
			string str = "Map Size (" + Width + ", " + Height + ")";
			str += "\nTile Size (" + TileWidth + ", " + TileHeight + ")";
			str += "\nOrientation: " + Orientation.ToString();
			str += "\nTiled Version: " + Version;
			return str;
		}
	}
}
