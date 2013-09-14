using UnityEngine;
using System;
using System.Collections.Generic;

public struct UzuVectorI3
{
	#region Constants
	public static readonly UzuVectorI3 one = new UzuVectorI3 (1, 1, 1);
	public static readonly UzuVectorI3 zero = new UzuVectorI3 (0, 0, 0);
	public static readonly UzuVectorI3 left = new UzuVectorI3 (-1, 0, 0);
	public static readonly UzuVectorI3 right = new UzuVectorI3 (1, 0, 0);
	public static readonly UzuVectorI3 up = new UzuVectorI3 (0, 1, 0);
	public static readonly UzuVectorI3 top = new UzuVectorI3 (0, 1, 0);
	public static readonly UzuVectorI3 down = new UzuVectorI3 (0, -1, 0);
	public static readonly UzuVectorI3 bottom = new UzuVectorI3 (0, -1, 0);
	public static readonly UzuVectorI3 forward = new UzuVectorI3 (0, 0, 1);
	public static readonly UzuVectorI3 back = new UzuVectorI3 (0, 0, -1);
	#endregion
	
	public int x, y, z;

	public int this [int index] {
		get {
			if (index == 0) {
				return x;
			}
			if (index == 1) {
				return y;
			}
			if (index == 2) {
				return z;
			}

			return 0;
		}
		set {
			if (index == 0) {
				x = value;
			}
			if (index == 1) {
				y = value;
			}
			if (index == 2) {
				z = value;
			}
		}
	}
	
	public UzuVectorI3 (int xyz)
	{
		this.x = xyz;
		this.y = xyz;
		this.z = xyz;
	}

	public UzuVectorI3 (int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public UzuVectorI3 (float x, float y, float z)
	{
		this.x = (int)x;
		this.y = (int)y;
		this.z = (int)z;
	}

	public UzuVectorI3 (UzuVectorI3 vector)
	{
		this.x = vector.x;
		this.y = vector.y;
		this.z = vector.z;
	}

	public UzuVectorI3 (Vector3 vector)
	{
		this.x = (int)vector.x;
		this.y = (int)vector.y;
		this.z = (int)vector.z;
	}

	#region VectorI3 / Vector3 Methods

	public static UzuVectorI3 GridCeil (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Ceil (v.x / roundBy.x) * roundBy.x,
			Mathf.Ceil (v.y / roundBy.y) * roundBy.y,
			Mathf.Ceil (v.z / roundBy.z) * roundBy.z
			));
	}

	public static UzuVectorI3 GridFloor (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Floor (v.x / roundBy.x) * roundBy.x,
			Mathf.Floor (v.y / roundBy.y) * roundBy.y,
			Mathf.Floor (v.z / roundBy.z) * roundBy.z
			));
	}

	public static UzuVectorI3 GridRound (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Round (v.x / roundBy.x) * roundBy.x,
			Mathf.Round (v.y / roundBy.y) * roundBy.y,
			Mathf.Round (v.z / roundBy.z) * roundBy.z
			));
	}

	public static UzuVectorI3 Ceil (Vector3 v)
	{
		return new UzuVectorI3 (Mathf.CeilToInt (v.x), Mathf.CeilToInt (v.y), Mathf.CeilToInt (v.z));
	}

	public static UzuVectorI3 Floor (Vector3 v)
	{
		return new UzuVectorI3 (Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y), Mathf.FloorToInt (v.z));
	}

	public static UzuVectorI3 Round (Vector3 v)
	{
		return new UzuVectorI3 (Mathf.RoundToInt (v.x), Mathf.RoundToInt (v.y), Mathf.RoundToInt (v.z));
	}
	
	public static int ElementSum (UzuVectorI3 v)
	{
		return v.x + v.y + v.z;
	}
	
	public static int ElementProduct (UzuVectorI3 v)
	{
		return v.x * v.y * v.z;
	}
	
	public static int MinElement (UzuVectorI3 v)
	{
		return Mathf.Min (v.z, Mathf.Min (v.x, v.y));
	}
	
	public static int MaxElement (UzuVectorI3 v)
	{
		return Mathf.Max (v.z, Mathf.Max (v.x, v.y));
	}
	
	public static UzuVectorI3 MinPerElement (UzuVectorI3 v0, UzuVectorI3 v1)
	{
		return new UzuVectorI3 (Mathf.Min (v0.x, v1.x), Mathf.Min (v0.y, v1.y), Mathf.Min (v0.z, v1.z));
	}
	
	public static UzuVectorI3 MaxPerElement (UzuVectorI3 v0, UzuVectorI3 v1)
	{
		return new UzuVectorI3 (Mathf.Max (v0.x, v1.x), Mathf.Max (v0.y, v1.y), Mathf.Max (v0.z, v1.z));
	}
	
	public static UzuVectorI3 Clamp (UzuVectorI3 v, UzuVectorI3 min, UzuVectorI3 max)
	{
		return new UzuVectorI3 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
	}

	public int size {
		get { return Size (this); }
	}

	public static int Size (UzuVectorI3 v)
	{
		return v.x * v.y * v.z;
	}
	
	public static bool AnyEqual (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x == b.x || a.y == b.y || a.z == b.z;
	}

	public static bool AnyGreater (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x > b.x || a.y > b.y || a.z > b.z;
	}

	public static bool AllGreater (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x > b.x && a.y > b.y && a.z > b.z;
	}

	public static bool AnyLower (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x < b.x || a.y < b.y || a.z < b.z;
	}

	public static bool AllLower (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x < b.x && a.y < b.y && a.z < b.z;
	}

	public static bool AnyGreaterEqual (UzuVectorI3 a, UzuVectorI3 b)
	{
		return AnyEqual (a, b) || AnyGreater (a, b);
	}

	public static bool AllGreaterEqual (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x >= b.x && a.y >= b.y && a.z >= b.z;
	}

	public static bool AnyLowerEqual (UzuVectorI3 a, UzuVectorI3 b)
	{
		return AnyEqual (a, b) || AnyLower (a, b);
	}

	public static bool AllLowerEqual (UzuVectorI3 a, UzuVectorI3 b)
	{
		return a.x <= b.x && a.y <= b.y && a.z <= b.z;
	}
	#endregion

	#region Advanced
	public static UzuVectorI3 operator - (UzuVectorI3 a)
	{
		return new UzuVectorI3 (-a.x, -a.y, -a.z);
	}

	public static UzuVectorI3 operator - (UzuVectorI3 a, UzuVectorI3 b)
	{
		return new UzuVectorI3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator - (Vector3 a, UzuVectorI3 b)
	{
		return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator - (UzuVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static bool operator != (UzuVectorI3 lhs, UzuVectorI3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static bool operator != (Vector3 lhs, UzuVectorI3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static bool operator != (UzuVectorI3 lhs, Vector3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static Vector3 operator * (float d, UzuVectorI3 a)
	{
		return new Vector3 (d * a.x, d * a.y, d * a.z);
	}

	public static Vector3 operator * (UzuVectorI3 a, float d)
	{
		return new Vector3 (a.x * d, a.y * d, a.z * d);
	}

	public static Vector3 operator / (UzuVectorI3 a, float d)
	{
		return new Vector3 (a.x / d, a.y / d, a.z / d);
	}

	public static float operator / (float d, UzuVectorI3 a)
	{
		d /= a.x;
		d /= a.y;
		d /= a.z;
		return d;
	}

	public static UzuVectorI3 operator * (UzuVectorI3 a, UzuVectorI3 b)
	{
		return new UzuVectorI3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Vector3 operator * (Vector3 a, UzuVectorI3 b)
	{
		return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Vector3 operator * (UzuVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static UzuVectorI3 operator / (UzuVectorI3 a, UzuVectorI3 b)
	{
		return new UzuVectorI3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static Vector3 operator / (Vector3 a, UzuVectorI3 b)
	{
		return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static Vector3 operator / (UzuVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static UzuVectorI3 operator + (UzuVectorI3 a, UzuVectorI3 b)
	{
		return new UzuVectorI3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (Vector3 a, UzuVectorI3 b)
	{
		return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (UzuVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (UzuVectorI3 a, float d)
	{
		return new Vector3 (a.x + d, a.y + d, a.z + d);
	}

	public static bool operator == (UzuVectorI3 lhs, UzuVectorI3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static bool operator == (Vector3 lhs, UzuVectorI3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static bool operator == (UzuVectorI3 lhs, Vector3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static implicit operator UzuVectorI3 (Vector2 v)
	{
		return new UzuVectorI3 (v.x, v.y, 0);
	}

	public static implicit operator UzuVectorI3 (Vector3 v)
	{
		return new UzuVectorI3 (v);
	}

	public static implicit operator UzuVectorI3 (Vector4 v)
	{
		return new UzuVectorI3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector2 (UzuVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector3 (UzuVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector4 (UzuVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator int[] (UzuVectorI3 v)
	{
		return new int[] { v.x, v.y, v.z };
	}

	public override bool Equals (object obj)
	{
		if (obj.GetType () == typeof(UzuVectorI3)) {
			UzuVectorI3 v = (UzuVectorI3)obj;
			return this.x == v.x && this.y == v.y && this.z == v.z;
		}

		return false;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}

	public override string ToString ()
	{
		return "(" + x + ", " + y + ", " + z + ")";
	}
	
	public Vector3 ToVector3 ()
	{
		return new Vector3 (x, y, z);
	}
	#endregion
}