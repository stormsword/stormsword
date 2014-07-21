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
using System.Text;
using X_UniTMX;
using UnityEditor;
using UnityEngine;

namespace X_UniTMX
{
	[CustomEditor (typeof(TiledMapComponent))]
	public class TiledMapEditor : Editor
	{	
		int arraySize = 0;
		List<string> collidersLayers = new List<string>();
		List<float> collidersWidth = new List<float>();
		List<float> collidersZDepth = new List<float>();
		List<bool> collidersIsInner = new List<bool>();
		List<bool> collidersIsTrigger = new List<bool>();
		bool foldoutColliders = false;

		void OnEnable()
		{
			if (collidersLayers == null)
				collidersLayers = new List<string>();
			if (collidersWidth == null)
				collidersWidth = new List<float>();
			if (collidersZDepth == null)
				collidersZDepth = new List<float>();
			if (collidersIsInner == null)
				collidersIsInner = new List<bool>();
			if (collidersIsTrigger == null)
				collidersIsTrigger = new List<bool>();

		}

		private void ClearCurrentmap(TiledMapComponent TMComponent)
		{
			// Destroy any previous map entities
			var children = new List<GameObject>();
			foreach (Transform child in TMComponent.transform)
				children.Add(child.gameObject);
			children.ForEach(child => DestroyImmediate(child, true));
			MeshFilter filter = TMComponent.GetComponent<MeshFilter>();
			if (filter)
				DestroyImmediate(filter, true);
		}

		private void DoImportMapButtonGUI(TiledMapComponent TMComponent)
		{
			if (GUILayout.Button("Import as static Tile Map"))
			{
				ClearCurrentmap(TMComponent);
				
				if (TMComponent.Initialize())
				{
					Debug.Log("Map sucessfull loaded!");
				}
			}
		}

		private void DoCollidersGUI(TiledMapComponent TMComponent)
		{
			TMComponent.GenerateCollider = EditorGUILayout.BeginToggleGroup("Generate Colliders", TMComponent.GenerateCollider);
			foldoutColliders = EditorGUILayout.Foldout(foldoutColliders, "Colliders Layers");
			if (foldoutColliders)
			{
				TMComponent.addTileNameToColliderName = EditorGUILayout.Toggle("Add tile name to collidder name?", TMComponent.addTileNameToColliderName);
				TMComponent.is2DCollider = EditorGUILayout.Toggle("Create Colliders 2D?", TMComponent.is2DCollider);

				if (TMComponent.CollidersLayerName != null && TMComponent.CollidersLayerName.Length > 0)
				{
					arraySize = TMComponent.CollidersLayerName.Length;
				}
				
				arraySize = EditorGUILayout.IntField("Colliders Layers Number", arraySize);
				if (arraySize < 1)
					arraySize = 1;
				int i = collidersLayers.Count;
				if (collidersLayers.Count < arraySize)
				{
					while (collidersLayers.Count < arraySize)
					{
						if (TMComponent.CollidersLayerName != null && TMComponent.CollidersLayerName.Length > i && TMComponent.CollidersLayerName[i].Length > 0)
						{
							collidersLayers.Add(TMComponent.CollidersLayerName[i]);
							collidersWidth.Add(TMComponent.CollidersWidth[i]);
							collidersZDepth.Add(TMComponent.CollidersZDepth[i]);
							collidersIsInner.Add(TMComponent.CollidersIsInner[i]);
							collidersIsTrigger.Add(TMComponent.CollidersIsTrigger[i]);
						}
						else
						{
							collidersLayers.Add("Collider_" + i);
							if (i > 0)
							{
								collidersWidth.Add(collidersWidth[0]);
								collidersZDepth.Add(collidersZDepth[0]);
								collidersIsInner.Add(collidersIsInner[0]);
								collidersIsTrigger.Add(collidersIsTrigger[0]);
							}
							else
							{
								collidersWidth.Add(1);
								collidersZDepth.Add(0);
								collidersIsInner.Add(false);
								collidersIsTrigger.Add(false);
							}
						}

						i++;
					}
				}
				else
				{
					if (collidersLayers.Count > arraySize)
					{
						while (collidersLayers.Count > arraySize)
						{
							collidersLayers.RemoveAt(collidersLayers.Count - 1);
							collidersWidth.RemoveAt(collidersLayers.Count - 1);
							collidersZDepth.RemoveAt(collidersLayers.Count - 1);
							collidersIsInner.RemoveAt(collidersLayers.Count - 1);
							collidersIsTrigger.RemoveAt(collidersLayers.Count - 1);
						}
					}
				}
				for (i = 0; i < arraySize; i++)
				{
					collidersLayers[i] = EditorGUILayout.TextField("Collider Layer " + i, collidersLayers[i]);
					if (!TMComponent.is2DCollider)
					{
						collidersWidth[i] = EditorGUILayout.FloatField(collidersLayers[i] + " Width", collidersWidth[i]);
						collidersZDepth[i] = EditorGUILayout.FloatField(collidersLayers[i] + " Z Depth", collidersZDepth[i]);
						collidersIsInner[i] = EditorGUILayout.Toggle(collidersLayers[i] + " Is Inner Collisions", collidersIsInner[i]);
					}
					collidersIsTrigger[i] = EditorGUILayout.Toggle(collidersLayers[i] + " Is Trigger", collidersIsTrigger[i]);
				}
				TMComponent.CollidersLayerName = collidersLayers.ToArray();
				TMComponent.CollidersWidth = collidersWidth.ToArray();
				TMComponent.CollidersZDepth = collidersZDepth.ToArray();
				TMComponent.CollidersIsInner = collidersIsInner.ToArray();
				TMComponent.CollidersIsTrigger = collidersIsTrigger.ToArray();
			}
			EditorGUILayout.EndToggleGroup();
		}

		private void DoClearMapButtonGUI(TiledMapComponent TMComponent)
		{
			if (GUILayout.Button("Clear Tile Map"))
			{
				ClearCurrentmap(TMComponent);
				Debug.Log("Map cleared!");
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			TiledMapComponent TMComponent = (TiledMapComponent)target;

			TMComponent.isToLoadOnStart = EditorGUILayout.Toggle("Load this on awake?", TMComponent.isToLoadOnStart);
			TMComponent.MakeUniqueTiles = EditorGUILayout.Toggle("Make unique tiles?", TMComponent.MakeUniqueTiles);

			TMComponent.DefaultSortingOrder = EditorGUILayout.IntField("Default Sorting Order", TMComponent.DefaultSortingOrder);
			//TMComponent.materialDefaultFile = EditorGUILayout.TextField("Default material tile map, don't put .mat extension:", TMComponent.materialDefaultFile);
			TMComponent.materialDefaultFile = (Material)EditorGUILayout.ObjectField("Default material tile map", TMComponent.materialDefaultFile, typeof(Material), true);
			TMComponent.MapTMX = (TextAsset)EditorGUILayout.ObjectField("Map xml:", TMComponent.MapTMX, typeof(TextAsset), true);
			TMComponent.MapTMXPath = EditorGUILayout.TextField("Map path name: ", TMComponent.MapTMXPath);

			DoCollidersGUI(TMComponent);
			DoImportMapButtonGUI(TMComponent);
			DoClearMapButtonGUI(TMComponent);
		}
	}
}
