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
	/// Set of utility functions for working with Unity's GameObject class.
	/// </summary>
	public static class GameObjectUtils
	{
		/// <summary>
		/// Enumerates through a GameObject's children.
		/// </summary>
		/// <param name="container">GameObject to enumerate</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="container" /> is null</exception>
		/// <returns>Enumerable sequence of Transform components, one for each child of container</returns>
		/// <remarks>This method is implemented using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
		public static IEnumerable<Transform> ImmediateChildren(GameObject container)
		{
			if (container == null)
			{
				throw new ArgumentException("container is null", "container");
			}
			for (int i = 0; i < container.transform.childCount; ++i)
			{
				yield return container.transform.GetChild(i);
			}
		}

		/// <summary>
		/// Selects all the components of the given type contained in the given sequence of GameObjects.
		/// </summary>
		/// <param name="source">Sequence of GameObjects containing the component type to query</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="source" /> is null</exception>
		/// <returns>Enumerable sequence of MonoBehaviour instances that were found attached to the GameObjects in <paramref name="source" /></returns>
		/// <remarks>This method is implemented using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
		public static IEnumerable<TResult> ComponentsIn<TResult>(IEnumerable<GameObject> source) where TResult : MonoBehaviour
		{
			if (source == null)
			{
				throw new ArgumentException("source is null", "source");
			}
			foreach (GameObject go in source)
			{
				if (go != null)
				{
					TResult component = go.GetComponent<TResult>();
					if (component != null)
					{
						yield return component;
					}
				}
			}
		}

		/// <summary>
		/// Selects all the GameObject instances that own the components in the given sequence.
		/// </summary>
		/// <param name="source">Sequence of MonoBehaviour instances</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="source" /> is null</exception>
		/// <returns>Enumerable sequence of GameObject instances that were found owning the MonoBehaviours in <paramref name="source" /> (duplicates are preserved)</returns>
		/// <remarks>This method is implemented using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
		public static IEnumerable<GameObject> GameObjectsOf<TSource>(IEnumerable<TSource> source) where TSource : MonoBehaviour
		{
			if (source == null)
			{
				throw new ArgumentException("source is null", "source");
			}
			foreach (MonoBehaviour component in source)
			{
				if (component != null)
				{
					yield return component.gameObject;
				}
			}
		}

		/// <summary>
		/// Instantiates a new GameObject.
		/// </summary>
		/// <param name="name">Name to give the new instance</param>
		/// <param name="parent">Parent GameObject to which the instance will be attached</param>
		/// <param name="components">Components to initially attach</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="name" /> is null</exception>
		/// <returns>New GameObject instance</returns>
		/// <remarks>The resulting instance's transform will be set to identity values.</remarks>
		public static GameObject InstantiateNew(string name, GameObject parent, params Type[] components)
		{
			if (name == null)
			{
				throw new ArgumentException("name is null", "name");
			}
			var instance = new GameObject(name, components);
			if (parent != null)
			{
				instance.transform.parent = parent.transform;
			}
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localRotation = Quaternion.identity;
			instance.transform.localScale = Vector3.one;
			return instance;
		}

		/// <summary>
		/// Instantiates a new GameObject with a single attached component.
		/// </summary>
		/// <param name="name">Name to give the new instance</param>
		/// <param name="parent">Parent GameObject to which the instance will be attached</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="name" /> is null</exception>
		/// <returns>MonoBehaviour instance of the requested type</returns>
		/// <remarks>The resulting instance's transform will be set to identity values.</remarks>
		public static TResult InstantiateNewSingle<TResult>(string name, GameObject parent) where TResult : MonoBehaviour
		{
			return InstantiateNew(name, parent, typeof(TResult)).GetComponent<TResult>();
		}

		/// <summary>
		/// Instantiates a prefab.
		/// </summary>
		/// <param name="prefab">Prefab to instantiate</param>
		/// <param name="parent">Parent GameObject to which the instance will be attached</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="prefab" /> is null</exception>
		/// <returns>Instantiated prefab</returns>
		/// <remarks>The resulting instance's transform will be set to match the prefab's local-space values</remarks>
		public static GameObject InstantiateChild(GameObject prefab, GameObject parent)
		{
			if (prefab == null)
			{
				throw new ArgumentException("prefab is null", "prefab");
			}
			var instance = UnityEngine.Object.Instantiate(prefab) as GameObject;
			if (parent != null)
			{
				instance.transform.SetParent(parent.transform, true);
			}
			instance.transform.localPosition = prefab.transform.localPosition;
			instance.transform.localRotation = prefab.transform.localRotation;
			instance.transform.localScale = prefab.transform.localScale;
			return instance;
		}

		/// <summary>
		/// Instantiates a prefab.
		/// </summary>
		/// <param name="prefab">Component owned by the prefab to instantiate</param>
		/// <param name="parent">Parent GameObject to which the instance will be attached</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="prefab" /> is null</exception>
		/// <returns>Component of given type attached to the instantiated prefab</returns>
		/// <remarks>The resulting instance's transform will be set to match the prefab's local-space values</remarks>
		public static TResult InstantiateChild<TResult>(TResult prefab, GameObject parent) where TResult : MonoBehaviour
		{
			if (prefab == null)
			{
				throw new ArgumentException("prefab is null", "prefab");
			}
			return InstantiateChild(prefab.gameObject, parent).GetComponent<TResult>();
		}

		/// <summary>
		/// Instantiates a prefab.
		/// </summary>
		/// <param name="prefab">Prefab to instantiate</param>
		/// <param name="parent">Sibling GameObject to whose parent the instance will be attached</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="prefab" /> is null</exception>
		/// <returns>Instantiated prefab</returns>
		/// <remarks>The resulting instance's transform will be set to match the prefab's local-space values</remarks>
		public static GameObject InstantiateSibling(GameObject prefab, GameObject sibling)
		{
			return InstantiateChild(prefab, sibling != null ? sibling.transform.parent.gameObject : null);
		}


		/// <summary>
		/// Instantiates a prefab.
		/// </summary>
		/// <param name="prefab">Component owned by the prefab to instantiate</param>
		/// <param name="parent">Sibling GameObject to whose parent the instance will be attached</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="prefab" /> is null</exception>
		/// <returns>Component of given type attached to the instantiated prefab</returns>
		/// <remarks>The resulting instance's transform will be set to match the prefab's local-space values</remarks>
		public static T InstantiateSibling<T>(T prefab, GameObject sibling) where T : MonoBehaviour
		{
			return InstantiateChild<T>(prefab, sibling != null ? sibling.transform.parent.gameObject : null);
		}
	}
}
