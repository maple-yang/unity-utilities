// MultiCoroutine.cs
// Copyright (c) 2015 Orbital Games, LLC.

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace OrbitalGames.UnityUtilities
{
	public class MultiCoroutine
	{
		private ICollection<IEnumerator> _coroutines;
		private MonoBehaviour _host;
		private int _remaining;

		public MultiCoroutine(ICollection<IEnumerator> coroutines, MonoBehaviour host)
		{
			_coroutines = coroutines;
			_host = host;
		}

		public static IEnumerator YieldMultiple(ICollection<IEnumerator> coroutines, MonoBehaviour host)
		{
			return (new MultiCoroutine(coroutines, host)).StartAll();
		}

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

		private IEnumerator StartSingle(IEnumerator coroutine)
		{
			yield return _host.StartCoroutine(coroutine);
			--_remaining;
		}
	}
}
