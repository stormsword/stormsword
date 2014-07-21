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
using System.Xml.Linq;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace X_UniTMX
{
	public class ImageLayer : Layer
	{
		public string Image;
		public Texture2D Texture;
		public Color? ColorKey;

		/// <summary>
		/// Layer's Game Object
		/// </summary>
		public GameObject LayerGameObject { get; private set; }

		public ImageLayer(XElement node, Map map, string mapPath, Material baseMaterial)
			: base(node)
		{
			XElement imageNode = node.Element("image");
			this.Image = imageNode.Attribute("source").Value;

			// if the image is in any director up from us, just take the filename
			if (this.Image.StartsWith(".."))
				this.Image = Path.GetFileName(this.Image);

			if (imageNode.Attribute("trans") != null)
			{
				string color = imageNode.Attribute("trans").Value;
				string r = color.Substring(0, 2);
				string g = color.Substring(2, 2);
				string b = color.Substring(4, 2);
				this.ColorKey = new Color((byte)Convert.ToInt32(r, 16), (byte)Convert.ToInt32(g, 16), (byte)Convert.ToInt32(b, 16));
			}

			string texturePath = mapPath;
			if (Path.GetDirectoryName(this.Image).Length > 0)
				texturePath += Path.GetDirectoryName(this.Image) + Path.AltDirectorySeparatorChar;
			this.Texture = (Texture2D)Resources.Load(texturePath + Path.GetFileNameWithoutExtension(this.Image), typeof(Texture2D));
			
			LayerGameObject = new GameObject(Name);
			LayerGameObject.transform.parent = map.MapObject.transform;
			LayerGameObject.transform.localPosition = new Vector3(0, 0, this.LayerDepth);

			LayerGameObject.isStatic = true;
			LayerGameObject.SetActive(Visible);

			SpriteRenderer tileRenderer = LayerGameObject.AddComponent<SpriteRenderer>();
			tileRenderer.sprite = Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), Vector2.up, map.TileWidth);
			tileRenderer.sprite.name = Texture.name;
			tileRenderer.sortingOrder = map.DefaultSortingOrder - LayerDepth;
			// Use Layer's name as Sorting Layer
			tileRenderer.sortingLayerName = this.Name;
			//tileRenderer.material = new Material(baseMaterial);
			//tileRenderer.material.mainTexture = Texture;
		}
	}
}
