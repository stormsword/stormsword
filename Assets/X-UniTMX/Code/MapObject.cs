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
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using UnityEngine;

namespace X_UniTMX
{
	/// <summary>
	/// Map Object Type, from Tiled's Objects types
	/// </summary>
	public enum MapObjectType : byte
	{
		Tile,
		Box,	
		Ellipse,
		Polygon,
		Polyline
	}

	/// <summary>
	/// An arbitrary object placed on an ObjectLayer.
	/// </summary>
	public class MapObject
	{
		/// <summary>
		/// Gets the name of the object.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the type of the object.
		/// </summary>
		public string Type { get; private set; }

		/// <summary>
		/// Gets the mapobjecttype of the object.
		/// </summary>
		public MapObjectType MapObjectType { get; private set; }

		/// <summary>
		/// Gets or sets the bounds of the object.
		/// </summary>
		public Rect Bounds { get; set; }

		/// <summary>
		/// Gets a list of the object's properties.
		/// </summary>
		public PropertyCollection Properties { get; private set; }

		/// <summary>
		/// Gets the object GID
		/// </summary>
		public int GID { get; private set; }

		/// <summary>
		/// Gets a list of the object's points
		/// </summary>
		public List<Vector2> Points { get; private set; }

		/// <summary>
		/// Gets or sets the whether the object is visible.
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// Gets or sets this object's rotation
		/// </summary>
		public float Rotation { get; set; }

		/// <summary>
		/// Creates a new MapObject.
		/// </summary>
		/// <param name="name">The name of the object.</param>
		/// <param name="type">The type of object to create.</param>
		public MapObject(string name, string type) : this(name, type, new Rect(), new PropertyCollection(), 0, new List<Vector2>(), 0) { }

		/// <summary>
		/// Creates a new MapObject.
		/// </summary>
		/// <param name="name">The name of the object.</param>
		/// <param name="type">The type of object to create.</param>
		/// <param name="bounds">The initial bounds of the object.</param>
		public MapObject(string name, string type, Rect bounds) : this(name, type, bounds, new PropertyCollection(), 0, new List<Vector2>(), 0) { }

		/// <summary>
		/// Creates a new MapObject.
		/// </summary>
		/// <param name="name">The name of the object.</param>
		/// <param name="type">The type of object to create.</param>
		/// <param name="bounds">The initial bounds of the object.</param>
		/// <param name="properties">The initial property collection or null to create an empty property collection.</param>
		/// <param name="rotation">This object's rotation</param>
		public MapObject(string name, string type, Rect bounds, PropertyCollection properties, int gid, List<Vector2> points, float rotation)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException(null, "name");

			Name = name;
			Type = type;
			Bounds = bounds;
			Properties = properties ?? new PropertyCollection();
			GID = gid;
			Points = points;
			Visible = true;
			Rotation = rotation;
		}

		public MapObject(XElement node)
        {
			if (node.Attribute("name") != null)
			{
				Name = node.Attribute("name").Value;
			}
			else
			{
				Name = "Object";
			}

			if (node.Attribute("type") != null)
			{
				Type = node.Attribute("type").Value;
			}
			else
				Type = "";

			if (node.Attribute("visible") != null)
			{
				Visible = int.Parse(node.Attribute("visible").Value, CultureInfo.InvariantCulture) == 1;
			}
			else
				Visible = true;

			if (node.Attribute("rotation") != null)
			{
				Rotation = 360 - float.Parse(node.Attribute("rotation").Value, CultureInfo.InvariantCulture);
			}
			else
				Rotation = 0;

            // values default to 0 if the attribute is missing from the node
			float x = node.Attribute("x") != null ? float.Parse(node.Attribute("x").Value, CultureInfo.InvariantCulture) : 0;
			float y = node.Attribute("y") != null ? float.Parse(node.Attribute("y").Value, CultureInfo.InvariantCulture) : 0;
			float width = node.Attribute("width") != null ? float.Parse(node.Attribute("width").Value, CultureInfo.InvariantCulture) : 1;
			float height = node.Attribute("height") != null ? float.Parse(node.Attribute("height").Value, CultureInfo.InvariantCulture) : 1;

            Bounds = new Rect(x, y, width, height);

			MapObjectType = MapObjectType.Box;

            XElement propertiesNode = node.Element("properties");
            if (propertiesNode != null)
            {
                Properties = new PropertyCollection(propertiesNode);
            }

            // stores a string of points to parse out if this object is a polygon or polyline
            string pointsAsString = null;

            // if there's a GID, it's a tile object
            if (node.Attribute("gid") != null)
            {
				MapObjectType = MapObjectType.Tile;
                GID = int.Parse(node.Attribute("gid").Value, CultureInfo.InvariantCulture);
            }
			// if there's an ellipse node, it's an ellipse object
			else if (node.Element("ellipse") != null)
			{
				MapObjectType = MapObjectType.Ellipse;
			}
			// if there's a polygon node, it's a polygon object
			else if (node.Element("polygon") != null)
			{
				pointsAsString = node.Element("polygon").Attribute("points").Value;
				MapObjectType = MapObjectType.Polygon;
			}
			// if there's a polyline node, it's a polyline object
			else if (node.Element("polyline") != null)
			{
				pointsAsString = node.Element("polyline").Attribute("points").Value;
				MapObjectType = MapObjectType.Polyline;
			}

            // if we have some points to parse, we do that now
            if (pointsAsString != null)
            {
                // points are separated first by spaces
				Points = new List<Vector2>();
                string[] pointPairs = pointsAsString.Split(' ');
                foreach (string p in pointPairs)
                {
                    // then we split on commas
                    string[] coords = p.Split(',');

                    // then we parse the X/Y coordinates
                    Points.Add(new Vector2(
                        float.Parse(coords[0], CultureInfo.InvariantCulture),
                        float.Parse(coords[1], CultureInfo.InvariantCulture)));
                }
            }
        }

		/// <summary>
		/// Scale this object's dimensions using map's tile size.
		/// We need to do this as Tiled saves object dimensions in Pixels, but we need to convert it to Unity's Unit
		/// </summary>
		/// <param name="TileWidth">Tiled Map Tile Width</param>
		/// <param name="TileHeight">Tiled Map Tile Height</param>
		public void ScaleObject(float TileWidth, float TileHeight)
		{
			this.Bounds = new Rect(this.Bounds.x / TileWidth, this.Bounds.y / TileHeight, this.Bounds.width / TileWidth, this.Bounds.height / TileHeight);
			
			if (this.Points != null)
			{
				for (int i = 0; i < this.Points.Count; i++)
				{
					this.Points[i] = new Vector2(this.Points[i].x / TileWidth, this.Points[i].y / TileHeight);
				}
			}
		}

		public void ScaleObject(float TileWidth, float TileHeight, Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.Orthogonal:
					ScaleObject(TileWidth, TileHeight);
					break;
				case Orientation.Isometric:
					// In Isometric maps, we must consider tile width == height for objects so their size can be correctly calculated
					ScaleObject(TileHeight, TileHeight);
					break;
				case Orientation.Staggered:
					// In Staggered maps, we must pre-alter object position and size, as it comes mixed between staggered and orthogonal properties
					float x = Bounds.x / (float)TileWidth;
					float y = Bounds.y / (float)TileHeight * 2.0f;
					float width = Bounds.width / (float)TileWidth;
					float height = Bounds.height / (float)TileWidth;

					if (Mathf.FloorToInt(Mathf.Abs(y)) % 2 > 0)
						x -= 0.5f;

					Bounds = new Rect(x, y, width, height);
					
					if (Points != null)
					{
						for (int i = 0; i < Points.Count; i++)
						{
							Points[i] = new Vector2(Points[i].x / (float)TileWidth, Points[i].y / (float)TileHeight * 2.0f);
						}
					}
					break;
			}
		}

		/// <summary>
		/// Creates this Tile Object (an object that has GID) if applicable
		/// </summary>
		/// <param name="tiledMap">The base Tile Map</param>
		public void CreateTileObject(Map tiledMap, string sortingLayerName, int layerDepth, List<Material> materials)
		{
			if(GID > 0) {
				Tile objTile = tiledMap.Tiles[GID].Clone();
				objTile.CreateTileObject(Name,
					tiledMap.MapObject.transform,
					sortingLayerName,
					new Vector3(Bounds.x, -Bounds.y, layerDepth),
					materials);
				
				objTile.TileObject.SetActive(Visible);
			}
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
	}
}
