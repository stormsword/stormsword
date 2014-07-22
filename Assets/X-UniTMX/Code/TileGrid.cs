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
using System.Text;

namespace X_UniTMX
{
	/// <summary>
	/// A 2D grid of Tile objects.
	/// </summary>
	public class TileGrid
	{
		private readonly Tile[,] rawTiles;

		/// <summary>
		/// Gets or sets a Tile at a given index.
		/// </summary>
		/// <param name="x">The X index.</param>
		/// <param name="y">The Y index.</param>
		/// <returns></returns>
		public Tile this[int x, int y]
		{
			get { return rawTiles[x, y]; }
			set { rawTiles[x, y] = value; }
		}

		/// <summary>
		/// Gets the width of the grid.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the height of the grid.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Creates a new TileGrid.
		/// </summary>
		/// <param name="width">The width of the grid.</param>
		/// <param name="height">The height of the grid.</param>
		public TileGrid(int width, int height)
		{
			rawTiles = new Tile[width, height];
			Width = width;
			Height = height;
		}
	}
}
