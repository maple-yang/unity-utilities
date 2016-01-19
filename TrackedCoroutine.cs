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
using System.Collections;
using UnityEngine;

namespace OrbitalGames.UnityUtilities
{
	/// <summary>
	/// Class used to add subscribable events to coroutines as well as spawn container objects for one-off coroutines.
	/// </summary>
	public class TrackedCoroutine : MonoBehaviour
	{
		[SerializeField] private bool _destroyOnComplete;
		[SerializeField] private bool _destroyOnStop;
		private bool _running;

		/// <summary>Delegate type to use as a valid coroutine</summary>
		public delegate IEnumerator Coroutine();

		/// <summary>Event fired when the coroutine is started</summary>
		public event EventHandler CoroutineStarted = delegate { };
		/// <summary>Event fired when the host object is destroyed</summary>
		public event EventHandler CoroutineDestroyed = delegate { };
		/// <summary>Event fired when the coroutine is explicitly stopped</summary>
		public event EventHandler CoroutineStopped = delegate { };
		/// <summary>Event fired when the coroutine is completed</summary>
		public event EventHandler CoroutineCompleted = delegate { };

		/// <summary>
		/// Whether or not the host object should be destroyed when the coroutine is completed
		/// </summary>
		public bool DestroyOnComplete
		{
			get { return _destroyOnComplete; }
			set { _destroyOnComplete = value; }
		}

		/// <summary>
		/// Whether or not the host object should be destroyed when the coroutine is stopped
		/// </summary>
		public bool DestroyOnStop
		{
			get { return _destroyOnStop; }
			set { _destroyOnStop = value; }
		}

		public Coroutine Routine { get; set; }

		/// <summary>
		/// Creates a TrackedCoroutine attached to a new container object that will destroy itself when finished.
		/// </summary>
		/// <param name="routine">Coroutine to run; if arguments are needed, pass an anonymous wrapper</param>
		/// <param name="name">Name of the container object</param>
		/// <param name="parent">Parent of the container object</param>
		/// <returns>Created TrackedCoroutine instance</returns>
		public static TrackedCoroutine Create(Coroutine routine, string name = "TrackedCoroutine", GameObject parent = null)
		{
			var instance = GameObjectUtils.InstantiateNewSingle<TrackedCoroutine>(name, parent);
			instance.Routine = routine;
			instance.DestroyOnComplete = true;
			return instance;
		}

		/// <summary>
		/// Begins the tracked coroutine.
		/// </summary>
		public void Run()
		{
			if (Routine == null)
			{
				return;
			}
			StartCoroutine(WrapCoroutine());
		}

		/// <summary>
		/// Stops the tracked coroutine.
		/// </summary>
		public void Stop()
		{
			StopAllCoroutines();
			_running = false;
			CoroutineStopped(this, EventArgs.Empty);
			if (DestroyOnStop)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		private IEnumerator WrapCoroutine()
		{
			_running = true;
			CoroutineStarted(this, EventArgs.Empty);
			yield return StartCoroutine(Routine());
			_running = false;
			CoroutineCompleted(this, EventArgs.Empty);
			if (DestroyOnComplete)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		private void OnDestroy()
		{
			if (_running)
			{
				_running = false;
				CoroutineDestroyed(this, EventArgs.Empty);
			}
		}
	}
}
