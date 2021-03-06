﻿/*
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
using System.Linq;
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
		/// Destroys a GameObject's children.
		/// </summary>
		/// <param name="container">GameObject whose children are to be destroyed</param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="container" /> is null</exception>
		public static void DestroyChildren(GameObject container, float delay = 0.0f)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			var transform = container.transform;
			for (int i = container.transform.childCount - 1; i >= 0; --i)
			{
				UnityEngine.Object.Destroy(transform.GetChild(i).gameObject, delay);
			}
		}

		/// <summary>
		/// Selects all the components of the given type contained in the given sequence of GameObjects.
		/// </summary>
		/// <param name="source">Sequence of GameObjects containing the component type to query</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="source" /> is null</exception>
		/// <returns>Enumerable sequence of Component instances that were found attached to the GameObjects in <paramref name="source" /></returns>
		/// <remarks>This method is implemented using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
		public static IEnumerable<TResult> ComponentsIn<TResult>(IEnumerable<GameObject> source) where TResult : Component
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
		/// <param name="source">Sequence of Component instances</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="source" /> is null</exception>
		/// <returns>Enumerable sequence of GameObject instances that were found owning the Components in <paramref name="source" /> (duplicates are preserved)</returns>
		/// <remarks>This method is implemented using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its GetEnumerator method directly or by using foreach.</remarks>
		public static IEnumerable<GameObject> GameObjectsOf<TSource>(IEnumerable<TSource> source) where TSource : Component
		{
			if (source == null)
			{
				throw new ArgumentException("source is null", "source");
			}
			foreach (var component in source)
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
		/// <returns>Component instance of the requested type</returns>
		/// <remarks>The resulting instance's transform will be set to identity values.</remarks>
		public static TResult InstantiateNewSingle<TResult>(string name, GameObject parent) where TResult : Component
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
			instance.transform.localRotation = prefab.transform.localRotation;
			instance.transform.localScale = prefab.transform.localScale;
			var asRect = instance.transform as RectTransform;
			if (asRect != null)
			{
				var prefabRect = (prefab.transform as RectTransform);
				asRect.sizeDelta = prefabRect.sizeDelta;
				asRect.anchorMin = prefabRect.anchorMin;
				asRect.anchorMax = prefabRect.anchorMax;
				asRect.anchoredPosition3D = prefabRect.anchoredPosition3D;
			}
			else
			{
				instance.transform.localPosition = prefab.transform.localPosition;
			}
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
		public static TResult InstantiateChild<TResult>(TResult prefab, GameObject parent) where TResult : Component
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
		public static TResult InstantiateSibling<TResult>(TResult prefab, GameObject sibling) where TResult : Component
		{
			return InstantiateChild<TResult>(prefab, sibling != null ? sibling.transform.parent.gameObject : null);
		}

		/// <summary>
		/// Finds a component in the scene attached to an object with the given tag.
		/// </summary>
		/// <param name="name">Tag name</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="name" /> is null or empty</exception>
		/// <returns>Component of given type attached to the object with the given tag, or null if not found</returns>
		public static TResult FindWithTag<TResult>(string name) where TResult : Component
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Tag name is invalid", "name");
			}
			var obj = GameObject.FindWithTag(name);
			return obj != null ? obj.GetComponent<TResult>() : null;
		}
	}
}
