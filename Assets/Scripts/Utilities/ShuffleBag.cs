using System;
using UnityEngine;

public enum ShuffleBagOption
{
	Default = 1 << 0,
	NonDepletable = 1 << 1,
	NoRandom = 1 << 2
}

public class ShuffleBag<T>
{
	ShuffleBagOption options;
	T[] items;
	uint itemIndex;
	bool empty = false;

	static class Util
	{
		public static void Swap(ref object a, ref object b)
		{
			object c = a;
			a = b;
			b = c;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}
	}

	public bool Empty
	{
		get
		{
			return empty;
		}
	}

	public T[] Rest
	{
		get
		{
			T[] res = new T[itemIndex + 1];
			Array.Copy(items, res, itemIndex + 1);
			return res;
		}
	}

	public ShuffleBag(T[] initalValues, ShuffleBagOption settings = ShuffleBagOption.Default)
	{
		options = settings;

		items = new T[initalValues.Length];

		Array.Copy(initalValues, items, initalValues.Length);

		itemIndex = items.Length == 0 ? 0 : (uint)(items.Length - 1);
		shuffle();
	}

	public T GetItem()
	{
		if (empty)throw new System.Exception("No Items Left. Use ShuffleBag.Empty to Make sure this Doesnt Happen");
		T item = items[itemIndex];

		//Shuffle Bag is Empty
		if (itemIndex == 0)
		{
			empty = true;
			return item;
		}

		if (!((options & ShuffleBagOption.NonDepletable) > 0))
			items[itemIndex--] = default(T);

		shuffle();
		return item;
	}

	public void shuffle()
	{
		if ((options & ShuffleBagOption.NoRandom) > 0)return;
		if (itemIndex == 0)return;

		for (int i = 0; i <= itemIndex; ++i)
		{
			Util.Swap(ref items[i], ref items[UnityEngine.Random.Range(0, (int)itemIndex - 1)]);
		}
	}
}
