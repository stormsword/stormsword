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
using System.Xml.Linq;
using UnityEngine;

namespace X_UniTMX
{
	/// <summary>
	/// A layer comprised of objects.
	/// </summary>
	public class MapObjectLayer : Layer
	{
		private Dictionary<string, MapObject> namedObjects = new Dictionary<string, MapObject>();

		/// <summary>
		/// Gets or sets this layer's color.
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Gets the objects on the current layer.
		/// </summary>
		public List<MapObject> Objects { get; private set; }

		/// <summary>
		/// Creates a map object layer from .tmx
		/// </summary>
		/// <param name="node"></param>
		public MapObjectLayer(XElement node, Map tiledMap, int layerDepth, List<Material> materials)
            : base(node)
        {
            if (node.Attribute("color") != null)
            {
                // get the color string, removing the leading #
                string color = node.Attribute("color").Value.Substring(1);

                // get the RGB individually
                string r = color.Substring(0, 2);
                string g = color.Substring(2, 2);
                string b = color.Substring(4, 2);

                // convert to the color
                Color = new Color(
                    (byte)int.Parse(r, NumberStyles.AllowHexSpecifier),
                    (byte)int.Parse(g, NumberStyles.AllowHexSpecifier),
                    (byte)int.Parse(b, NumberStyles.AllowHexSpecifier));
            }

			Objects = new List<MapObject>();

            foreach (XElement objectNode in node.Descendants("object"))
            {
                MapObject mapObjectContent = new MapObject(objectNode);
				//if (tiledMap.Orientation == Orientation.Orthogonal)
				//{
				//	mapObjectContent.ScaleObject(tiledMap.TileWidth, tiledMap.TileHeight);
				//}
				//// In Isometric maps, we must consider tile width == height for objects so their size can be correctly calculated
				//else if (tiledMap.Orientation == Orientation.Isometric)
				//{
				//	mapObjectContent.ScaleObject(tiledMap.TileHeight, tiledMap.TileHeight);
				//}
				//// In Staggered maps, we must pre-alter object position, as it comes mixed between staggered and orthogonal properties
				//else if (tiledMap.Orientation == Orientation.Staggered)
				//{
				//	float x = mapObjectContent.Bounds.x / (float)tiledMap.TileWidth;
				//	float y = mapObjectContent.Bounds.y / (float)tiledMap.TileHeight * 2.0f;
				//	float width = mapObjectContent.Bounds.width / (float)tiledMap.TileWidth;
				//	float height = mapObjectContent.Bounds.height / (float)tiledMap.TileWidth;

				//	if (Mathf.FloorToInt(Mathf.Abs(y)) % 2 > 0)
				//		x -= 0.5f;

				//	mapObjectContent.Bounds = new Rect(x, y, width, height);
					
				//	if (mapObjectContent.Points != null)
				//	{
				//		for (int i = 0; i < mapObjectContent.Points.Count; i++)
				//		{
				//			mapObjectContent.Points[i] = new Vector2(mapObjectContent.Points[i].x / (float)tiledMap.TileWidth, mapObjectContent.Points[i].y / (float)tiledMap.TileHeight * 2.0f);
				//		}
				//	}
				//}
				mapObjectContent.ScaleObject(tiledMap.TileWidth, tiledMap.TileHeight, tiledMap.Orientation);
				mapObjectContent.Name = this.Name + "_" + mapObjectContent.Name;
                // Object names need to be unique for our lookup system, but Tiled
                // doesn't require unique names.
                string objectName = mapObjectContent.Name;
                int duplicateCount = 2;
				
                // if a object already has the same name...
                if (Objects.Find(o => o.Name.Equals(objectName)) != null)
                {
                    // figure out a object name that does work
                    do
                    {
                        objectName = string.Format("{0}{1}", mapObjectContent.Name, duplicateCount);
                        duplicateCount++;
                    } while (Objects.Find(o => o.Name.Equals(objectName)) != null);

                    // log a warning for the user to see
                    Debug.Log("Renaming object \"" + mapObjectContent.Name + "\" to \"" + objectName + "\" in layer \"" + Name + "\" to make a unique name.");

                    // save that name
					mapObjectContent.Name = objectName;
                }
				mapObjectContent.CreateTileObject(tiledMap, Name, layerDepth, materials);

				AddObject(mapObjectContent);
            }
        }

		internal MapObjectLayer(string name, int width, int height, int layerDepth, bool visible, float opacity, PropertyCollection properties, List<MapObject> initialObjects)
			: base(name, width, height, layerDepth, visible, opacity, properties)
		{
			Objects = new List<MapObject>();
			initialObjects.ForEach(AddObject);
		}

		/// <summary>
		/// Adds a MapObject to the layer.
		/// </summary>
		/// <param name="mapObject">The MapObject to add.</param>
		public void AddObject(MapObject mapObject)
		{
			// avoid adding the object to the layer twice
			if (Objects.Contains(mapObject))
				return;

			namedObjects.Add(mapObject.Name, mapObject);
			Objects.Add(mapObject);
		}

		/// <summary>
		/// Gets a MapObject by name.
		/// </summary>
		/// <param name="objectName">The name of the object to retrieve.</param>
		/// <returns>The MapObject with the given name.</returns>
		public MapObject GetObject(string objectName)
		{
			return namedObjects[objectName];
		}

		/// <summary>
		/// Removes an object from the layer.
		/// </summary>
		/// <param name="mapObject">The object to remove.</param>
		/// <returns>True if the object was found and removed, false otherwise.</returns>
		public bool RemoveObject(MapObject mapObject)
		{
			return RemoveObject(mapObject.Name);
		}

		/// <summary>
		/// Removes an object from the layer.
		/// </summary>
		/// <param name="objectName">The name of the object to remove.</param>
		/// <returns>True if the object was found and removed, false otherwise.</returns>
		public bool RemoveObject(string objectName)
		{
			MapObject obj;
			if (namedObjects.TryGetValue(objectName, out obj))
			{
				Objects.Remove(obj);
				namedObjects.Remove(objectName);
				return true;
			}
			return false;
		}
	}
}
