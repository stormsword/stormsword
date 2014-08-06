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
using System.Xml;

namespace X_UniTMX
{
	/// <summary>
	/// A simple key-value property pair.
	/// </summary>
	public class Property
	{
		// cached values to avoid parsing multiple times
		private float? cachedFloat;
		private int? cachedInt;
		private bool? cachedBool;

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the raw value string of the property.
		/// </summary>
		public string RawValue { get; private set; }

		/// <summary>
		/// Creates a new Property with a given name and initial value.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="value">The initial value of the property.</param>
		public Property(string name, string value)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException(null, "name");

			Name = name;
			RawValue = value ?? string.Empty;
		}

		/// <summary>
		/// Sets the value of the property as an integer.
		/// </summary>
		/// <param name="value">The new value of the property.</param>
		public void SetValue(int value)
		{
			RawValue = value.ToString(CultureInfo.InvariantCulture);
			cachedInt = value;
			cachedFloat = null;
			cachedBool = null;
		}

		/// <summary>
		/// Sets the value of the property as a floating point number.
		/// </summary>
		/// <param name="value">The new value of the property.</param>
		public void SetValue(float value)
		{
			RawValue = value.ToString(CultureInfo.InvariantCulture);
			cachedInt = null;
			cachedFloat = value;
			cachedBool = null;
		}

		/// <summary>
		/// Sets the value of the property as a boolean.
		/// </summary>
		/// <param name="value">The new value of the property.</param>
		public void SetValue(bool value)
		{
			RawValue = value.ToString();
			cachedInt = null;
			cachedFloat = null;
			cachedBool = value;
		}

		/// <summary>
		/// Sets the value of the property as a string.
		/// </summary>
		/// <param name="value">The new value of the property.</param>
		public void SetValue(string value)
		{
			RawValue = value;
			cachedInt = null;
			cachedFloat = null;
			cachedBool = null;
		}

		/*
		 * define some explicit conversion operators that just reference our methods.
		 * this is very much based on the way XDocument handles attributes through
		 * explicit casts rather than parsing string values.
		 * 
		 * this lets us do things like: 
		 *	  Property p = ...  
		 *	  int value = (int)p;
		 * instead of 
		 *    Property p = ...
		 *    int value = p.AsInt();
		 *    
		 * The other benefit is that we can also cast to other types that can be
		 * cast from these three types. for instance we can do this:
		 *    Property p = ...
		 *    byte value = (byte)p;
		 *    double value = (double)p;
		 *    
		 * and so on. I'm removing the AsX methods because these are cleaner, have
		 * more functions, and I want to keep redundant code to a minimum.
		 */

		public static explicit operator int(Property prop)
		{
			if (!prop.cachedInt.HasValue)
				prop.cachedInt = int.Parse(prop.RawValue, CultureInfo.InvariantCulture);
			return prop.cachedInt.Value;
		}

		public static explicit operator float(Property prop)
		{
			if (!prop.cachedFloat.HasValue)
				prop.cachedFloat = float.Parse(prop.RawValue, CultureInfo.InvariantCulture);
			return prop.cachedFloat.Value;
		}

		public static explicit operator bool(Property prop)
		{
			if (!prop.cachedBool.HasValue)
				prop.cachedBool = bool.Parse(prop.RawValue);
			return prop.cachedBool.Value;
		}

		public static explicit operator string(Property prop)
		{
			return prop.RawValue;
		}
	}
}
