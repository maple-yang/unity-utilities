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
using UnityEngine;

namespace OrbitalGames.UnityUtilities
{
	/// <summary>
	/// Set of utility functions for working with Unity's Color class.
	/// </summary>
	public static class ColorUtils
	{
		/// <summary>
		/// Generates a random color.
		/// </summary>
		/// <param name="rng">Random number generator to use; a new default instance is created if this is omitted</param>
		/// <returns>Random color with full alpha</returns>
		public static Color GetRandom(System.Random rng = null)
		{
			if (rng == null)
			{
				rng = new System.Random();
			}
			return new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble(), 1.0f);
		}

		/// <summary>
		/// Linearly interpolate between two colors using a vector of interpolants.
		/// </summary>
		/// <param name="a">Starting color</param>
		/// <param name="b">Ending color</param>
		/// <param name="t">Interpolant vector, where each element corresponds to the r, g, b, and a values (respectively)</param>
		/// <returns>Interpolated color</returns>
		public static Color Lerp(Color a, Color b, Vector4 t)
		{
			return new Color(Mathf.Lerp(a.r, b.r, t.x), Mathf.Lerp(a.g, b.g, t.y), Mathf.Lerp(a.b, b.b, t.z), Mathf.Lerp(a.a, b.a, t.w));
		}
	}
}
