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

namespace X_UniTMX
{
	/// <summary>
	/// An enumerable collection of properties.
	/// </summary>
	public class PropertyCollection : IEnumerable<Property>
	{
		// cheating under the hood :)
		private readonly Dictionary<string, Property> values = new Dictionary<string, Property>();

		/// <summary>
		/// Gets a Property with the given name.
		/// </summary>
		/// <param name="name">The name of the property to retrieve.</param>
		/// <returns>The Property if a matching one is found or null if no Property exists for the given name.</returns>
		public Property this[string name]
		{
			get
			{
				Property p;
				if (values.TryGetValue(name, out p))
					return p;
				return null;
			}
		}

		/// <summary>
		/// Creates a new PropertyCollection.
		/// </summary>
		public PropertyCollection() { }

		/// <summary>
		/// Adds a property to the collection.
		/// </summary>
		/// <param name="property">The property to add.</param>
		public void Add(Property property)
		{
			values.Add(property.Name, property);
		}

		/// <summary>
		/// Attempts to get a property by name.
		/// </summary>
		/// <param name="name">The name of the property to retrieve.</param>
		/// <param name="property">The property that is found, if one matches.</param>
		/// <returns>True if the property was found, false otherwise.</returns>
		public bool TryGetValue(string name, out Property property)
		{
			return values.TryGetValue(name, out property);
		}

		/// <summary>
		/// Removes a property with the given name.
		/// </summary>
		/// <param name="name">The name of the property to remove.</param>
		/// <returns>True if the property was removed, false otherwise.</returns>
		public bool Remove(string name)
		{
			return values.Remove(name);
		}

		// internal constructor because games shouldn't make their own PropertyCollections
		internal PropertyCollection(XElement element)
		{
			List<Property> properties = new List<Property>();

			foreach (XElement property in element.Descendants())
			{
				string name = property.Attribute("name").Value;
				string value = property.Attribute("value").Value;
				bool foundCopy = false;
				/* 
				 * A bug in Tiled will sometimes cause the file to contain identical copies of properties.
				 * I would fix it, but I'd have to dig into the Tiled code. instead, we'll detect exact
				 * duplicates here and log some warnings, failing only if the value is actually different.
				 * 
				 * To repro the bug, create two maps that use the same tileset. Open the first file in Tiled
				 * and set a property on a tile. Then open the second map and open the first back up. Look
				 * at the propertes on the tile. It will have two or three copies of the same property.
				 * 
				 * If you encounter the bug, you can remedy it in Tiled by closing the current file (Ctrl-F4
				 * or use Close from the File menu) and then reopen it. The tile will no longer have the
				 * copies of the property.
				 */
				foreach (var p in properties)
				{
					if (p.Name == name)
					{
						if (p.RawValue == value)
						{
							foundCopy = true;
						}
						else
						{
							throw new Exception(string.Format("Multiple properties of name {0} exist with different values: {1} and {2}", name, value, p.RawValue));
						}
					}
				}

				// we only want to add one copy of any duplicate properties
				if (!foundCopy)
				{
					Property p = new Property(name, value);
					properties.Add(p);
					Add(p);
				}
			}
			//foreach (var p in properties)
			//{
			//	values.Add(p.Name, p);
			//}
		}

		/// <summary>
		/// Gets an enumerator that can be used to iterate over the properties in the collection.
		/// </summary>
		/// <returns>An enumerator over the properties.</returns>
		public IEnumerator<Property> GetEnumerator()
		{
			return values.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return values.Values.GetEnumerator();
		}
	}
}
