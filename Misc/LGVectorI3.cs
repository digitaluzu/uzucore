using UnityEngine;
using System;
using System.Collections.Generic;

public struct LGVectorI3
{
	#region Constants
	public static readonly LGVectorI3 one = new LGVectorI3 (1, 1, 1);
	public static readonly LGVectorI3 zero = new LGVectorI3 (0, 0, 0);
	public static readonly LGVectorI3 left = new LGVectorI3 (-1, 0, 0);
	public static readonly LGVectorI3 right = new LGVectorI3 (1, 0, 0);
	public static readonly LGVectorI3 up = new LGVectorI3 (0, 1, 0);
	public static readonly LGVectorI3 top = new LGVectorI3 (0, 1, 0);
	public static readonly LGVectorI3 down = new LGVectorI3 (0, -1, 0);
	public static readonly LGVectorI3 bottom = new LGVectorI3 (0, -1, 0);
	public static readonly LGVectorI3 forward = new LGVectorI3 (0, 0, 1);
	public static readonly LGVectorI3 back = new LGVectorI3 (0, 0, -1);
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
	
	public LGVectorI3 (int xyz)
	{
		this.x = xyz;
		this.y = xyz;
		this.z = xyz;
	}

	public LGVectorI3 (int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public LGVectorI3 (float x, float y, float z)
	{
		this.x = (int)x;
		this.y = (int)y;
		this.z = (int)z;
	}

	public LGVectorI3 (LGVectorI3 vector)
	{
		this.x = vector.x;
		this.y = vector.y;
		this.z = vector.z;
	}

	public LGVectorI3 (Vector3 vector)
	{
		this.x = (int)vector.x;
		this.y = (int)vector.y;
		this.z = (int)vector.z;
	}

	#region VectorI3 / Vector3 Methods

	public static LGVectorI3 GridCeil (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Ceil (v.x / roundBy.x) * roundBy.x,
			Mathf.Ceil (v.y / roundBy.y) * roundBy.y,
			Mathf.Ceil (v.z / roundBy.z) * roundBy.z
			));
	}

	public static LGVectorI3 GridFloor (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Floor (v.x / roundBy.x) * roundBy.x,
			Mathf.Floor (v.y / roundBy.y) * roundBy.y,
			Mathf.Floor (v.z / roundBy.z) * roundBy.z
			));
	}

	public static LGVectorI3 GridRound (Vector3 v, Vector3 roundBy)
	{
		return Round (new Vector3 (
			Mathf.Round (v.x / roundBy.x) * roundBy.x,
			Mathf.Round (v.y / roundBy.y) * roundBy.y,
			Mathf.Round (v.z / roundBy.z) * roundBy.z
			));
	}

	public static LGVectorI3 Ceil (Vector3 v)
	{
		return new LGVectorI3 (Mathf.CeilToInt (v.x), Mathf.CeilToInt (v.y), Mathf.CeilToInt (v.z));
	}

	public static LGVectorI3 Floor (Vector3 v)
	{
		return new LGVectorI3 (Mathf.FloorToInt (v.x), Mathf.FloorToInt (v.y), Mathf.FloorToInt (v.z));
	}

	public static LGVectorI3 Round (Vector3 v)
	{
		return new LGVectorI3 (Mathf.RoundToInt (v.x), Mathf.RoundToInt (v.y), Mathf.RoundToInt (v.z));
	}
	
	public static int ElementSum (LGVectorI3 v)
	{
		return v.x + v.y + v.z;
	}
	
	public static int ElementProduct (LGVectorI3 v)
	{
		return v.x * v.y * v.z;
	}
	
	public static int MinElement (LGVectorI3 v)
	{
		return Mathf.Min (v.z, Mathf.Min (v.x, v.y));
	}
	
	public static int MaxElement (LGVectorI3 v)
	{
		return Mathf.Max (v.z, Mathf.Max (v.x, v.y));
	}
	
	public static LGVectorI3 MinPerElement (LGVectorI3 v0, LGVectorI3 v1)
	{
		return new LGVectorI3 (Mathf.Min (v0.x, v1.x), Mathf.Min (v0.y, v1.y), Mathf.Min (v0.z, v1.z));
	}
	
	public static LGVectorI3 MaxPerElement (LGVectorI3 v0, LGVectorI3 v1)
	{
		return new LGVectorI3 (Mathf.Max (v0.x, v1.x), Mathf.Max (v0.y, v1.y), Mathf.Max (v0.z, v1.z));
	}
	
	public static LGVectorI3 Clamp (LGVectorI3 v, LGVectorI3 min, LGVectorI3 max)
	{
		return new LGVectorI3 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
	}

	public int size {
		get { return Size (this); }
	}

	public static int Size (LGVectorI3 v)
	{
		return v.x * v.y * v.z;
	}
	
	public static bool AnyEqual (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x == b.x || a.y == b.y || a.z == b.z;
	}

	public static bool AnyGreater (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x > b.x || a.y > b.y || a.z > b.z;
	}

	public static bool AllGreater (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x > b.x && a.y > b.y && a.z > b.z;
	}

	public static bool AnyLower (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x < b.x || a.y < b.y || a.z < b.z;
	}

	public static bool AllLower (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x < b.x && a.y < b.y && a.z < b.z;
	}

	public static bool AnyGreaterEqual (LGVectorI3 a, LGVectorI3 b)
	{
		return AnyEqual (a, b) || AnyGreater (a, b);
	}

	public static bool AllGreaterEqual (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x >= b.x && a.y >= b.y && a.z >= b.z;
	}

	public static bool AnyLowerEqual (LGVectorI3 a, LGVectorI3 b)
	{
		return AnyEqual (a, b) || AnyLower (a, b);
	}

	public static bool AllLowerEqual (LGVectorI3 a, LGVectorI3 b)
	{
		return a.x <= b.x && a.y <= b.y && a.z <= b.z;
	}
	#endregion

	#region Advanced
	public static LGVectorI3 operator - (LGVectorI3 a)
	{
		return new LGVectorI3 (-a.x, -a.y, -a.z);
	}

	public static LGVectorI3 operator - (LGVectorI3 a, LGVectorI3 b)
	{
		return new LGVectorI3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator - (Vector3 a, LGVectorI3 b)
	{
		return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static Vector3 operator - (LGVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
	}

	public static bool operator != (LGVectorI3 lhs, LGVectorI3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static bool operator != (Vector3 lhs, LGVectorI3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static bool operator != (LGVectorI3 lhs, Vector3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	public static Vector3 operator * (float d, LGVectorI3 a)
	{
		return new Vector3 (d * a.x, d * a.y, d * a.z);
	}

	public static Vector3 operator * (LGVectorI3 a, float d)
	{
		return new Vector3 (a.x * d, a.y * d, a.z * d);
	}

	public static Vector3 operator / (LGVectorI3 a, float d)
	{
		return new Vector3 (a.x / d, a.y / d, a.z / d);
	}

	public static float operator / (float d, LGVectorI3 a)
	{
		d /= a.x;
		d /= a.y;
		d /= a.z;
		return d;
	}

	public static LGVectorI3 operator * (LGVectorI3 a, LGVectorI3 b)
	{
		return new LGVectorI3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Vector3 operator * (Vector3 a, LGVectorI3 b)
	{
		return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static Vector3 operator * (LGVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x * b.x, a.y * b.y, a.z * b.z);
	}

	public static LGVectorI3 operator / (LGVectorI3 a, LGVectorI3 b)
	{
		return new LGVectorI3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static Vector3 operator / (Vector3 a, LGVectorI3 b)
	{
		return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static Vector3 operator / (LGVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x / b.x, a.y / b.y, a.z / b.z);
	}

	public static LGVectorI3 operator + (LGVectorI3 a, LGVectorI3 b)
	{
		return new LGVectorI3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (Vector3 a, LGVectorI3 b)
	{
		return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (LGVectorI3 a, Vector3 b)
	{
		return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
	}

	public static Vector3 operator + (LGVectorI3 a, float d)
	{
		return new Vector3 (a.x + d, a.y + d, a.z + d);
	}

	public static bool operator == (LGVectorI3 lhs, LGVectorI3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static bool operator == (Vector3 lhs, LGVectorI3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static bool operator == (LGVectorI3 lhs, Vector3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static implicit operator LGVectorI3 (Vector2 v)
	{
		return new LGVectorI3 (v.x, v.y, 0);
	}

	public static implicit operator LGVectorI3 (Vector3 v)
	{
		return new LGVectorI3 (v);
	}

	public static implicit operator LGVectorI3 (Vector4 v)
	{
		return new LGVectorI3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector2 (LGVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector3 (LGVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator Vector4 (LGVectorI3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	public static implicit operator int[] (LGVectorI3 v)
	{
		return new int[] { v.x, v.y, v.z };
	}

	public override bool Equals (object obj)
	{
		if (obj.GetType () == typeof(LGVectorI3)) {
			LGVectorI3 v = (LGVectorI3)obj;
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