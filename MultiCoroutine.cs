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

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace OrbitalGames.UnityUtilities
{
	/// <summary>
	/// Class used to run multiple simultaneous coroutines and wait until all are finished.
	/// </summary>
	public class MultiCoroutine
	{
		private ICollection<IEnumerator> _coroutines;
		private MonoBehaviour _host;
		private int _remaining;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="coroutines">Coroutines to run</param>
		/// <param name="host">MonoBehaviour used to run each coroutine</param>
		public MultiCoroutine(ICollection<IEnumerator> coroutines, MonoBehaviour host)
		{
			_coroutines = coroutines;
			_host = host;
		}

		/// <summary>
		/// Runs each given coroutine simultaneously and yields until all are finished.
		/// </summary>
		/// <param name="coroutines">Coroutines to run</param>
		/// <param name="host">MonoBehaviour used to run each coroutine</param>
		/// <returns>IEnumerator suitable to execute as its own coroutine<returns>
		public static IEnumerator YieldMultiple(ICollection<IEnumerator> coroutines, MonoBehaviour host)
		{
			return (new MultiCoroutine(coroutines, host)).StartAll();
		}

		/// <summary>
		/// Begins execution of all coroutines.
		/// </summary>
		public IEnumerator StartAll()
		{
			if (_remaining > 0)
			{
				yield break;
			}
			_remaining = _coroutines.Count;
			foreach (var coroutine in _coroutines)
			{
				_host.StartCoroutine(StartSingle(coroutine));
			}
			while (_remaining > 0)
			{
				yield return 0;
			}
			yield break;
		}

		/// <summary>
		/// Starts a single coroutine and decrements the remaining count afterwards.
		/// </summary>
		/// <param name="coroutine">Coroutine to run</param>
		/// <returns>IEnumerator to pass to StartCoroutine()</returns>
		private IEnumerator StartSingle(IEnumerator coroutine)
		{
			yield return _host.StartCoroutine(coroutine);
			--_remaining;
		}
	}
}
