using System;
using System.Collections.Generic;
using UnityEngine;

public static class OverLoads
{
	//Array Extentions

	/// <summary>
	/// Joins Two Arrays together to create 1 long one
	/// </summary>
	/// <param name="Array1">The first array to join</param>
	/// <param name="Array2">The second array to join</param>
	/// <typeparam name="Type">The array of the type</typeparam>
	/// <returns>A Array of both joined together</returns>
	public static T[] Concat<T>(this T[] a, T[] b)
	{
		int cond = 0;
		cond |= a == null ? 0 : 0b01;
		cond |= b == null ? 0 : 0b10;

		switch (cond)
		{
			case 0b00:
				return null;
			case 0b01:
				return a;
			case 0b10:
				return b;
			case 0b11:
				T[] result = new T[a.Length + b.Length];
				Array.Copy(a, result, a.Length);
				Array.Copy(b, 0, result, a.Length, b.Length);
				return result;
			default:
				return null;
		}
	}

	/// <summary>
	/// Converts a Array to a different type. One element at a time
	/// </summary>
	/// <param name="converter">The Function to convert from one type to another</param>
	/// <typeparam name="T">The Type from the original Array</typeparam>
	/// <typeparam name="N">The Type for the resulting Array</typeparam>
	/// <returns>A Array of all converted values</returns>
	public static N[] Map<T, N>(this T[] list, Func<T, N> converter)
	{
		if (list == null)return null;
		N[] result = new N[list.Length];
		for (int i = 0; i < result.Length; ++i)
		{
			result[i] = converter(list[i]);
		}
		return result;
	}

	/// <summary>
	/// Filters a Array depending on a Condition
	/// </summary>
	/// <param name="filter">The filter deciding which items to keep</param>
	/// <typeparam name="T">The type of the Array</typeparam>
	/// <returns>A Filtered array with only the elements that pass the filter</returns>
	public static T[] Filter<T>(this T[] list, Func<T, bool> filter)
	{
		if (list == null || list.Length == 0)return list;
		List<T> result = new List<T>();
		foreach (T element in list)
		{
			if (filter(element))
				result.Add(element);
		}
		return result.ToArray();
	}

	/// <summary>
	/// Reduces a Array down to one Value
	/// </summary>
	/// <typeparam name="T">The Type of the array</typeparam>
	/// <typeparam name="N">The output type it should reduce to</typeparam>
	/// <returns>A Single value reduced from all elements</returns>
	public static N Reduce<T, N>(this T[] list, Func<T, N, N> reducer, N initialValue = default(N))
	{
		if (list == null || list.Length == 0)return initialValue;
		N result = initialValue;
		foreach (T element in list)
		{
			result = reducer(element, result);
		}
		return result;
	}

	/// <summary>
	/// Removes the Last element in the Array
	/// </summary>
	/// <returns>The Last element of the Array</returns>
	public static T Pop<T>(this List<T> list)
	{
		if (list == null || list.Count == 0)return default(T);
		var v = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
		return v;
	}

	/// <summary>
	/// Removes the First element in the Array
	/// </summary>
	/// <returns>The First element of the Array</returns>
	public static T Shift<T>(this List<T> list)
	{
		if (list == null || list.Count == 0)return default(T);
		var v = list[0];
		list.RemoveAt(0);
		return v;
	}

	/// <summary>
	/// Returns if the List is Empty
	/// </summary>
	/// <returns>True if the List is empty. False otherwise</returns>
	public static bool IsEmpty<T>(this List<T> list)
	{
		return list?.Count == 0;
	}

	/// <summary>
	/// Finds the First Child on the Specified Layer
	/// </summary>
	/// <param name="layer">The Layer to search on</param>
	/// <returns>The first GameObject on the specified layer</returns>
	public static GameObject FindChildOnLayer(this Transform parent, LayerMask layer)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);

			if ((child.gameObject.layer & layer) > 0)
				return child.gameObject;

			if (child.childCount > 0)
			{
				var res = FindChildOnLayer(child, layer);
				if (res != null)
					return res;
			}

		}
		return null;
	}

	/// <summary>
	/// Finds all Children on a certain Layer under the GameObject
	/// </summary>
	/// <param name="layer">The Layer search on</param>
	/// <returns>A array of all children</returns>
	public static GameObject[] FindChildrenOnLayer(this Transform parent, LayerMask layer)
	{
		List<GameObject> result = new List<GameObject>();

		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if ((child.gameObject.layer & layer) > 0)
				result.Add(child.gameObject);

			if (child.childCount > 0)
				result.AddRange(FindChildrenOnLayer(child, layer));

		}
		return result.ToArray();
	}

	/// <summary>
	/// Finds the First Child with the matching tag
	/// </summary>
	/// <param name="tag">The tag to search for</param>
	/// <returns>The first GameObject with the matching tag</returns>
	public static GameObject FindChildWithTag(this Transform parent, string tag)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);

			if (child.tag == tag)
				return child.gameObject;

			if (child.childCount > 0)
			{
				var res = FindChildWithTag(child, tag);
				if (res != null)
					return res;
			}

		}
		return null;
	}

	/// <summary>
	/// Finds all Children with matching tags under the GameObject
	/// </summary>
	/// <param name="tag">The tag to search for</param>
	/// <returns>A array of all children</returns>
	public static GameObject[] FindChildrenWithTag(this Transform parent, string tag)
	{
		List<GameObject> result = new List<GameObject>();

		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child.tag == tag)
				result.Add(child.gameObject);

			if (child.childCount > 0)
				result.AddRange(FindChildrenWithTag(child, tag));

		}
		return result.ToArray();
	}

	/// <summary>
	/// Collects all Children into a Array
	/// </summary>
	/// <returns>A Array of all Children</returns>
	public static GameObject[] Children(this GameObject obj)
	{
		List<GameObject> result = new List<GameObject>();
		for (var i = 0; i < obj.transform.childCount; ++i)
		{
			result.Add(obj.transform.GetChild(i).gameObject);
		}
		return result.ToArray();
	}

	/// <summary>
	/// Determines if a float is within a min and max
	/// </summary>
	/// <param name="min">The Minimum</param>
	/// <param name="max">The Maximum</param>
	/// <returns>True if the value is between the min and max</returns>
	public static bool IsWithin(this float value, float min, float max)
	{
		return value > min && value < max;
	}

	/// <summary>
	/// Returns a Random Element from a Array
	/// </summary>
	/// <returns>A Random element</returns>
	public static T RandomElement<T>(this T[] arr)
	{
		if (arr == null || arr.Length == 0)
			return default(T);
		return arr[UnityEngine.Random.Range(0, arr.Length)];
	}

	/// <summary>
	/// Checks if a layer is within a specified layer mask
	/// </summary>
	/// <returns>A bool based on result</returns>
	public static bool Includes(this LayerMask mask, int layer)
	{
		return (mask.value & 1 << layer) > 0;
	}

	/// <summary>
	/// Gets the closest element of 'objects' to 'position'
	/// </summary>
	/// <returns>Returns the associated gameobject</returns>
	public static GameObject GetClosestGameObject(Vector3 position, List<GameObject> objects)
	{
		// The result of the method
		GameObject closestObject = null;
		float currentClosestDistance = Mathf.Infinity;

		for (int i = 0; i < objects.Count; i++)
		{
			float distance = Vector3.Distance(position, objects[i].transform.position);
			if (distance < currentClosestDistance)
			{
				currentClosestDistance = distance;
				closestObject = objects[i];
			}
		}

		return closestObject;
	}

	/// <summary>
	/// Gets the closest element of 'transforms' to 'position'
	/// </summary>
	/// <returns>Returns the associated gameobject</returns>
	public static Transform GetClosestTransform(this Transform trans, Vector3 position, List<Transform> transforms)
	{
		// The result of the method
		Transform closestTransform = null;
		float currentClosestDistance = Mathf.Infinity;

		for (int i = 0; i < transforms.Count; i++)
		{
			float distance = Vector3.Distance(position, transforms[i].position);
			if (distance < currentClosestDistance)
			{
				currentClosestDistance = distance;
				closestTransform = transforms[i];
			}
		}

		return closestTransform;
	}
}
