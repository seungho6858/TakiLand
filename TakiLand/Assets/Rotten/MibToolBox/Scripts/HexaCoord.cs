using System;
using System.Collections.Generic;
using EnumsNET;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mib
{
	public struct HexaCoord
	{
		[Flags]
		public enum Direction
		{
			None = 0,
			Up = 1 << 0,
			UpperRight = 1 << 1,
			LowerRight = 1 << 2,
			Down = 1 << 3,
			LowerLeft = 1 << 4,
			UpperLeft = 1 << 5,
			Self = 1 << 6,
			Around = Up | UpperRight | LowerRight | Down | LowerLeft | UpperLeft,
			All = Around | Self
		}

		public static HexaCoord Zero = new(0, 0);
		public static HexaCoord Max = new(int.MaxValue, int.MaxValue);
		public static HexaCoord Min = new(int.MinValue, int.MinValue);

		public static HexaCoord Up = new(Vector2Int.up * 2);
		public static HexaCoord UpperRight = new(Vector2Int.up + Vector2Int.right);
		public static HexaCoord LowerRight = new(Vector2Int.down + Vector2Int.right);
		public static HexaCoord Down = new(Vector2Int.down * 2);
		public static HexaCoord LowerLeft = new(Vector2Int.down + Vector2Int.left);
		public static HexaCoord UpperLeft = new(Vector2Int.up + Vector2Int.left);

		[SerializeField, HideInInspector]
		private Vector2Int _position;

		public HexaCoord(Vector2Int position)
		{
			_position = position;
		}

		public HexaCoord(int x, int y)
		{
			_position = new Vector2Int(x, y);
		}

		public int X
		{
			get => _position.x;
			set => _position.x = value;
		}

		public int Y
		{
			get => _position.y;
			set => _position.y = value;
		}

		public override bool Equals(object obj)
		{
			return obj is HexaCoord other && Equals(other);
		}

		public override int GetHashCode()
		{
			return _position.GetHashCode();
		}

		public override string ToString()
		{
			return $"[{X.ToString()}, {Y.ToString()}]";
		}

		public static Direction RandDireciton()
		{
			IReadOnlyList<EnumMember<Direction>> list = Enums.GetMembers<Direction>();
			return list[Random.Range(1, list.Count - 1)].Value;
		}

		public bool Equals(HexaCoord other)
		{
			return _position.Equals(other._position);
		}

		public static HexaCoord operator +(HexaCoord a)
		{
			return a;
		}

		public static HexaCoord operator -(HexaCoord a)
		{
			return new HexaCoord(-a._position);
		}

		public static bool operator ==(HexaCoord a, HexaCoord b)
		{
			return a._position == b._position;
		}

		public static bool operator !=(HexaCoord a, HexaCoord b)
		{
			return !(a == b);
		}

		public static HexaCoord operator +(HexaCoord a, HexaCoord b)
		{
			return new HexaCoord(a._position + b._position);
		}

		public static HexaCoord operator -(HexaCoord a, HexaCoord b)
		{
			return new HexaCoord(a._position - b._position);
		}

		public static HexaCoord operator *(HexaCoord a, HexaCoord b)
		{
			return new HexaCoord(a._position * b._position);
		}

		public static HexaCoord operator /(HexaCoord a, HexaCoord b)
		{
			if ((b._position.x == 0) || (b._position.y == 0))
			{
				throw new DivideByZeroException();
			}

			return new HexaCoord(a._position.x / b._position.x, a._position.y / b._position.y);
		}

		// direction의 좌우를 리턴
		public static Direction GetSideDirection(Direction direction)
		{
			if (direction == Direction.None)
			{
				return Direction.None;
			}

			IReadOnlyList<EnumMember<Direction>> list = Enums.GetMembers<Direction>();
			for (int i = 1; i < list.Count - 1; i++)
			{
				EnumMember<Direction> enumMember = list[i];
				if (enumMember.Value == direction)
				{
					Direction right = list[i].Value == Direction.UpperLeft
						? Direction.Up
						: list[i + 1].Value;

					Direction left = list[i].Value == Direction.Up
						? Direction.UpperLeft
						: list[i - 1].Value;

					return right.CombineFlags(left);
				}
			}

			return Direction.None;
		}

		public bool IsNeighbor(HexaCoord other)
		{
			Vector2Int delta = other._position - _position;
			var deltaAbs = new Vector2Int(Math.Abs(delta.x), Math.Abs(delta.y));
			if ((deltaAbs.x > 1) || (deltaAbs.y > 2) || (delta == Vector2Int.zero))
			{
				return false;
			}

			return true;
		}

		public bool IsVertical(HexaCoord other)
		{
			return X == other.X;
		}

		public bool IsHorizontal(HexaCoord other)
		{
			return Y - other.Y <= 1;
		}

		public Direction GetDirection(HexaCoord target)
		{
			// if (!IsNeighbor(target))
			// {
			// 	return Direction.None;
			// }

			Vector2Int delta = target._position - _position;
			return delta switch
			{
				{ x: 0, y: >= 2 } => Direction.Up,
				{ x: >= 1, y: >= 1 } => Direction.UpperRight,
				{ x: >= 1, y: <= -1 } => Direction.LowerRight,
				{ x: 0, y: <= -2 } => Direction.Down,
				{ x: <= -1, y: <= -1 } => Direction.LowerLeft,
				{ x: <= -1, y: >= 1 } => Direction.UpperLeft,
				_ => Direction.Around
			};
		}

		public int Distance(HexaCoord other)
		{
			return Distance(this, other);
		}

		public static int Distance(HexaCoord source, HexaCoord target)
		{
			int dcol = Math.Abs(target.X - source.X);
			int drow = Math.Abs(target.Y - source.Y);
			return dcol + Math.Max(0, (drow - dcol) / 2);
		}

		public int Magnitude()
		{
			return Distance(Zero, this);
		}

		public IEnumerable<HexaCoord> GetPositions(Direction direction)
		{
			IReadOnlyList<EnumMember<Direction>> list = Enums.GetMembers<Direction>();
			for (int i = 1; i < list.Count - 1; i++)
			{
				EnumMember<Direction> enumMember = list[i];
				if (direction.HasAnyFlags(enumMember.Value))
				{
					yield return GetPosition(enumMember.Value);
				}
			}
		}

		public HexaCoord GetPosition(Direction direction)
		{
			var origin = new HexaCoord(_position);
			HexaCoord delta = direction switch
			{
				Direction.Up => Up,
				Direction.UpperRight => UpperRight,
				Direction.LowerRight => LowerRight,
				Direction.Down => Down,
				Direction.LowerLeft => LowerLeft,
				Direction.UpperLeft => UpperLeft,
				_ => Zero,
			};

			return origin + delta;
		}

		public class HexaTileComparer : IEqualityComparer<HexaCoord>
		{
			public bool Equals(HexaCoord x, HexaCoord y)
			{
				return (x.X == y.X) && (x.Y == y.Y);
			}

			public int GetHashCode(HexaCoord obj)
			{
				return HashCode.Combine(obj.X, obj.Y);
			}
		}
	}
}