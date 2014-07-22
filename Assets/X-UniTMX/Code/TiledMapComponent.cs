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
using System.IO;
using UnityEngine;
using System.Collections;
using X_UniTMX;
using System.Xml;

[AddComponentMenu("Tiled Map/Tiled Map Component")]
public class TiledMapComponent : MonoBehaviour {
	public Material materialDefaultFile;
	public TextAsset MapTMX;
	public string MapTMXPath = "GameMaps/";
	public int DefaultSortingOrder = 0;
	public bool GenerateCollider = false;
	public float[] CollidersZDepth;
	public float[] CollidersWidth;
	public string[] CollidersLayerName;
	public bool[] CollidersIsInner;
	public bool[] CollidersIsTrigger;
	public bool MakeUniqueTiles = true;
	public bool is2DCollider = false;
	public bool isToLoadOnStart = false;
	public bool addTileNameToColliderName = true;

	private Map tiledMap;

	public Map TiledMap
	{
		get { return tiledMap; }
		set { tiledMap = value; }
	}

	public void Awake()
	{
		if (isToLoadOnStart) Initialize();
	}

	// Use this for initialization
	public bool Initialize()
	{
		if (MapTMX == null)
		{
			Debug.LogError("No xml map set!");
			return false;
		}
		// load a default tile material
		if (materialDefaultFile == null)
		{
			Debug.LogError("No default material set!");
			return false;
		}
		tiledMap = new Map(MapTMX, MakeUniqueTiles, MapTMXPath, gameObject, materialDefaultFile, DefaultSortingOrder);
		Resources.UnloadUnusedAssets();
		if (GenerateCollider) GenerateColliders();
		return true;
	}

	public void GenerateColliders()
	{
		for (int i = 0; i < CollidersLayerName.Length; i++)
		{
			MapObjectLayer collisionLayer = (MapObjectLayer)tiledMap.GetLayer(CollidersLayerName[i]);
			if (collisionLayer != null)
			{
				List<MapObject> colliders = collisionLayer.Objects;
				foreach (MapObject colliderObjMap in colliders)
				{
					GameObject newColliderObject = null;
					if ("NoCollider".Equals(colliderObjMap.Type) == false)
					{
						newColliderObject = tiledMap.GenerateCollider(colliderObjMap, CollidersIsTrigger[i], is2DCollider, CollidersZDepth[i], CollidersWidth[i], CollidersIsInner[i]);
					}

					if (colliderObjMap.GetPropertyAsBoolean("detach prefab"))
					{
						newColliderObject = null;
					}

					tiledMap.AddPrefabs(colliderObjMap, newColliderObject, is2DCollider, addTileNameToColliderName);

					// if this colider has transfer, so delete this colider
					if (colliderObjMap.GetPropertyAsBoolean("transfer collider"))
					{
						if (is2DCollider)
						{
							Destroy(newColliderObject.collider2D);
						}
						else
						{
							Destroy(newColliderObject.collider);
						}
					}
				}
			}
			else
			{
				Debug.LogError("There's no Layer \"" + CollidersLayerName[i] + "\" in tile map.");
			}
		}
		
	}
}
