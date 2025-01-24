using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;
using Mib;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static partial class ExtensionMethods
{
	public static string ToHexString(this Color color)
	{
		Color32 color32 = color;
		return ToHexString(color32);
	}
	
	public static string ToHexString(this Color32 color32)
	{
		
		return ZString.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
			color32.r,
			color32.g,
			color32.b,
			color32.a);
	}
	
	public static Color ChangeAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		for (int i = 0; i < list.Count; ++i)
		{
			int targetIndex = Random.Range(0, list.Count);
			(list[i], list[targetIndex]) = (list[targetIndex], list[i]);
		}
	}

	public static GameObject Go(this Object obj) =>
		obj switch
		{
			GameObject go => go,
			MonoBehaviour mb => mb.gameObject,
			_ => throw new ArgumentOutOfRangeException()
		};

	public static Transform Tr(this Object obj) =>
		obj switch
		{
			GameObject go => go.transform,
			MonoBehaviour mb => mb.transform,
			_ => throw new ArgumentOutOfRangeException()
		};

	public static T RandByProb<T>(this IList<T> list, IList<int> probList)
	{
		if (probList == null || list.Count != probList.Count)
		{
			Debug.LogError("has Not same length");
			return default;
		}

		int totalProb = probList.Sum();
		int rand = Random.Range(0, totalProb);

		for (int i = 0; i < list.Count; ++i)
		{
			rand -= probList[i];
			if (rand <= 0.0f)
			{
				return list[i];
			}
		}

		Debug.LogError("Something Wrong!!");
		return default;
	}

	public static bool IsEmpty<T>(this ICollection<T> collection)
	{
		return collection.Count <= 0;
	}

	public static bool IsEmpty<T>(this Queue<T> queue)
	{
		return queue.Count <= 0;
	}

	// [minInclusive..maxExclusive)
	public static void Shuffle<T>(this IList<T> list, int minInclusive, int maxExclusive)
	{
		if (minInclusive >= maxExclusive || minInclusive < 0 || maxExclusive > list.Count)
		{
			Debug.LogError($"Invalid index begin({minInclusive.ToString()}) end({maxExclusive.ToString()})");
			return;
		}

		for (int i = minInclusive; i < maxExclusive; ++i)
		{
			int targetIndex = Random.Range(minInclusive, maxExclusive);
			(list[i], list[targetIndex]) = (list[targetIndex], list[i]);
		}
	}

	public static bool TryPop<T>(this IList<T> list, out T item)
	{
		if (list.Count <= 0)
		{
			item = default;
			return false;
		}

		item = list.LastOrDefault();
		list.RemoveAt(list.Count - 1);
		return true;
	}

	public static T Pop<T>(this IList<T> list)
	{
		TryPop(list, out T item);
		return item;
	}

	public static string AsString<T>(this IEnumerable<T> enumerable)
	{
		using Utf16ValueStringBuilder builder = ZString.CreateStringBuilder(true);
		foreach (T t in enumerable)
		{
			builder.AppendFormat("{0}\t", t.ToString());
		}

		return builder.ToString();
	}

	public static T Rand<T>(this IEnumerable<T> enumerable) where T : class
	{
		// 시간나면 orderBy말고 unsafe같은걸로 Span에 stackalloc해서 포인터 저장해뒀다가 리턴해주는 걸로 바꾸자. 
		return enumerable.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
	}

	public static T Rand<T>(this IList<T> list)
	{
		if (list.Count <= 0)
		{
			return default;
		}

		int targetIndex = Random.Range(0, list.Count);
		return list[targetIndex];
	}

	public static int RandomRange(this IList<int> list)
	{
		if (list.Count == 1)
		{
			return list[0];
		}

		if (list.Count == 2)
		{
			return Random.Range(list[0], list[1]);
		}
		
		Logg.Error("Invalid listCount");
		return 0;
	}

	public static HexaCoord.Direction Opposite(this HexaCoord.Direction direction)
	{
		return direction switch
		{
			HexaCoord.Direction.None => HexaCoord.Direction.All,
			HexaCoord.Direction.Self => HexaCoord.Direction.Around,
			HexaCoord.Direction.Up => HexaCoord.Direction.Down,
			HexaCoord.Direction.UpperRight => HexaCoord.Direction.LowerLeft,
			HexaCoord.Direction.LowerRight => HexaCoord.Direction.UpperLeft,
			HexaCoord.Direction.Down => HexaCoord.Direction.Up,
			HexaCoord.Direction.LowerLeft => HexaCoord.Direction.UpperRight,
			HexaCoord.Direction.UpperLeft => HexaCoord.Direction.LowerRight,
			HexaCoord.Direction.Around => HexaCoord.Direction.Self,
			HexaCoord.Direction.All => HexaCoord.Direction.None,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}
}