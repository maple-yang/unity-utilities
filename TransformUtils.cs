/*
The MIT License (MIT)

Copyright (c) 2015 Orbital Games, LLC.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OrbitalGames.UnityUtilities
{
	/// <summary>
	/// Set of utility functions for working with Unity's Transform class.
	/// </summary>
	public static class TransformUtils
	{
		/// <summary>
		/// Sets the X component of the given transform's position.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New X</param>
		/// <param name="local">Specify true to apply the value in local space</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform SetX(this Transform transform, float value, bool local = false)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			if (local)
			{
				transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
			}
			else
			{
				transform.position = new Vector3(value, transform.position.y, transform.position.z);
			}
			return transform;
		}

		/// <summary>
		/// Sets the Y component of the given transform's position.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New Y</param>
		/// <param name="local">Specify true to apply the value in local space</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform SetY(this Transform transform, float value, bool local = false)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			if (local)
			{
				transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
			}
			else
			{
				transform.position = new Vector3(transform.position.x, value, transform.position.z);
			}
			return transform;
		}

		/// <summary>
		/// Sets the Z component of the given transform's position.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New Z</param>
		/// <param name="local">Specify true to apply the value in local space</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform SetZ(this Transform transform, float value, bool local = false)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			if (local)
			{
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
			}
			else
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, value);
			}
			return transform;
		}

		/// <summary>
		/// Sets the X component of the given transform's local scale.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New X</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform ScaleX(this Transform transform, float value)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
			return transform;
		}

		/// <summary>
		/// Sets the Y component of the given transform's local scale.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New Y</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform ScaleY(this Transform transform, float value)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
			return transform;
		}

		/// <summary>
		/// Sets the Z component of the given transform's local scale.
		/// </summary>
		/// <param name="transform">Transform to modify</param>
		/// <param name="value">New Z</param>
		/// <returns>Given transform to allow for chaining</returns>
		public static Transform ScaleZ(this Transform transform, float value)
		{
			if (transform == null)
			{
				throw new ArgumentException("transform is null", "transform");
			}
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
			return transform;
		}
	}
}
