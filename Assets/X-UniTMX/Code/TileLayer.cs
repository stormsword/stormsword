/*!
 * X-UniTMX: A tiled map editor file importer for Unity3d
 * https://bitbucket.org/Chaoseiro/x-unitmx
 * 
 * Copyright 2013 Guilherme "Chaoseiro" Maia
 * Released under the MIT license
 * Check LICENSE.MIT for more details.
 */

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.IO;
using System.IO.Compression;
//using Ionic.Zlib;

namespace X_UniTMX
{
	/// <summary>
	/// A map layer containing tiles.
	/// </summary>
	public class TileLayer : Layer
	{
		// The data coming in combines flags for whether the tile is flipped as well as
		// the actual index. These flags are used to first figure out if it's flipped and
		// then to remove those flags and get us the actual ID.
		private const uint FlippedHorizontallyFlag = 0x80000000;
		private const uint FlippedVerticallyFlag = 0x40000000;

		/// <summary>
		/// Gets the layout of tiles on the layer.
		/// </summary>
		public TileGrid Tiles { get; private set; }

		/// <summary>
		/// Layer's Game Object
		/// </summary>
		public GameObject LayerGameObject { get; private set; }


		/*internal TileLayer(string name, int width, int height, float layerDepth, bool visible, float opacity, PropertyCollection properties, Map map, uint[] data, bool makeUnique)
			: base(name, width, height, layerDepth, visible, opacity, properties)
		{
			Initialize(map, data, makeUnique);
		}*/

		public uint[] Data;

		public TileLayer(XElement node, Map map, int layerDepth, bool makeUnique, List<Material> materials)
            : base(node)
        {
            XElement dataNode = node.Element("data");
            Data = new uint[Width * Height];
			LayerDepth = layerDepth;
			//Debug.Log("Layer Depth: " + LayerDepth);
            // figure out what encoding is being used, if any, and process
            // the data appropriately
            if (dataNode.Attribute("encoding") != null)
            {
                string encoding = dataNode.Attribute("encoding").Value;

                if (encoding == "base64")
                {
                    ReadAsBase64(node, dataNode);
                }
                else if (encoding == "csv")
                {
                    ReadAsCsv(node, dataNode);
                }
                else
                {
                    throw new Exception("Unknown encoding: " + encoding);
                }
            }
            else
            {
                // XML format simply lays out a lot of <tile gid="X" /> nodes inside of data.

                int i = 0;
                foreach (XElement tileNode in dataNode.Descendants("tile"))
                {
                    Data[i] = uint.Parse(tileNode.Attribute("gid").Value, CultureInfo.InvariantCulture);
                    i++;
                }

                if (i != Data.Length)
                    throw new Exception("Not enough tile nodes to fill data");
            }

			Initialize(map, Data, makeUnique, materials);
        }

        private void ReadAsCsv(XElement node, XElement dataNode)
        {
            // split the text up into lines
            string[] lines = node.Value.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // iterate each line
            for (int i = 0; i < lines.Length; i++)
            {
                // split the line into individual pieces
                string[] indices = lines[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // iterate the indices and store in our data
                for (int j = 0; j < indices.Length; j++)
                {
                    Data[i * Width + j] = uint.Parse(indices[j], CultureInfo.InvariantCulture);
                }
            }
        }

        private void ReadAsBase64(XElement node, XElement dataNode)
        {
            // get a stream to the decoded Base64 text
            Stream data = new MemoryStream(Convert.FromBase64String(node.Value), false);

            // figure out what, if any, compression we're using. the compression determines
            // if we need to wrap our data stream in a decompression stream
            if (dataNode.Attribute("compression") != null)
            {
                string compression = dataNode.Attribute("compression").Value;

                if (compression == "gzip")
                {
					data = new Ionic.Zlib.GZipStream(data, Ionic.Zlib.CompressionMode.Decompress, false);
                }
                else if (compression == "zlib")
                {
                    data = new Ionic.Zlib.ZlibStream(data, Ionic.Zlib.CompressionMode.Decompress, false);
                }
                else
                {
                    throw new InvalidOperationException("Unknown compression: " + compression);
                }
            }

            // simply read in all the integers
            using (data)
            {
                using (BinaryReader reader = new BinaryReader(data))
                {
                    for (int i = 0; i < Data.Length; i++)
                    {
                        Data[i] = reader.ReadUInt32();
                    }
                }
            }
        }

		private void Initialize(Map map, uint[] data, bool makeUnique, List<Material> materials)
		{
			Tiles = new TileGrid(Width, Height);
			
			// data is left-to-right, top-to-bottom
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					uint index = data[y * Width + x];

					// compute the SpriteEffects to apply to this tile
					SpriteEffects spriteEffects = SpriteEffects.None;
					if ((index & FlippedHorizontallyFlag) != 0)
						spriteEffects |= SpriteEffects.FlipHorizontally;
					if ((index & FlippedVerticallyFlag) != 0)
						spriteEffects |= SpriteEffects.FlipVertically;

					// strip out the flip flags to get the real ID
					int id = (int)(index & ~(FlippedVerticallyFlag | FlippedHorizontallyFlag));
					//Debug.Log("Tile ID: " + id + " (index : " + index + ")");
					// get the tile
					Tile t = null;
					map.Tiles.TryGetValue(id, out t);
					
					// if the tile is non-null...
					if (t != null)
					{
						// if we want unique instances, clone it
						if (makeUnique)
						{
							t = t.Clone();
							t.SpriteEffects = spriteEffects;
						}

						// otherwise we may need to clone if the tile doesn't have the correct effects
						// in this world a flipped tile is different than a non-flipped one; just because
						// they have the same source rect doesn't mean they're equal.
						else if (t.SpriteEffects != spriteEffects)
						{
							t = t.Clone();
							t.SpriteEffects = spriteEffects;
						}
					}

					// put that tile in our grid
					Tiles[x, y] = t;
				}
			}
			
			GenerateLayer(map, materials);
		}
		
		// Renders the tile vertices.
		// Basically, it reads the tiles and creates its 4 vertexes (forming a rectangle or square according to settings) 
		private void GenerateLayer(Map map, List<Material> materials)
		{
			LayerGameObject = new GameObject(Name);
			Tile t;
			
			// We must create tiles in reverse order so Z order is correct according to the majority of tilesets
			for (int x = Width - 1; x > -1; x--)
			{
				for (int y = Height - 1; y > -1; y--)
				{
					t = Tiles[x, y];
					if(t != null) {
						Vector3 pos = Vector3.zero;
						// Set Tile's position according to map orientation
						// Can't use Map.TiledPositionToWorldPoint as sprites' anchors doesn't follow tile anchor point
						if (map.Orientation == Orientation.Orthogonal)
						{
							pos = new Vector3(
								x * (map.TileWidth / (float)t.TileSet.TileWidth),
								(-y - 1) * (map.TileHeight / (float)t.TileSet.TileHeight) * ((float)t.TileSet.TileHeight / (float)t.TileSet.TileWidth),
								0);
						}
						else if (map.Orientation == Orientation.Isometric)
						{
							pos = new Vector3(
								(map.TileWidth / 2.0f * (map.Width - y + x) - t.TileSet.TileWidth / 2.0f) / (float)map.TileWidth,
								-map.Height + map.TileHeight * (map.Height - ((x + y) / (map.TileWidth / (float)map.TileHeight)) / 2.0f) / (float)map.TileHeight - (map.TileHeight / (float)map.TileWidth),
								0);
						}
						else if (map.Orientation == Orientation.Staggered)
						{
							// In Staggered maps, odd rows and even rows are handled differently
							if (y % 2 < 1)
							{
								// Even row
								pos.x = x * (map.TileWidth / (float)t.TileSet.TileWidth);
								pos.y = (-y - 2) * (map.TileHeight / 2.0f / (float)t.TileSet.TileHeight) * ((float)t.TileSet.TileHeight / (float)t.TileSet.TileWidth);
							}
							else
							{
								// Odd row
								pos.x = x * (map.TileWidth / (float)t.TileSet.TileWidth) + (map.TileWidth / (float)t.TileSet.TileWidth) / 2.0f;
								pos.y = (-y - 2) * (map.TileHeight / 2.0f / (float)t.TileSet.TileHeight) * ((float)t.TileSet.TileHeight / (float)t.TileSet.TileWidth);
							}
						}

						// Create Tile's GameObject
						t.CreateTileObject(Name + "[" + x + ", " + y + "]",
							LayerGameObject.transform,
							Name,
							pos,
							materials,
							Opacity);
						
					}
				}
			}

			LayerGameObject.transform.parent = map.MapObject.transform;
			LayerGameObject.transform.localPosition = new Vector3(0, 0, this.LayerDepth);
			LayerGameObject.isStatic = true;
			LayerGameObject.SetActive(Visible);
		}
	}
}