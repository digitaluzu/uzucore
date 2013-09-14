using UnityEngine;
using System.Collections;

/// <summary>
/// A basic sphere representation.
/// </summary>
public struct UzuSphere
{
	/// <summary>
	/// Gets or sets the center position of the sphere.
	/// </summary>
	public Vector3 Center {
		get { return _center; }
		set { _center = value; }
	}
	
	/// <summary>
	/// Gets or sets the radius of the sphere.
	/// </summary>
	public float Radius {
		get { return _radius; }
		set { _radius = value; }
	}
	
	public UzuSphere (Vector3 center, float radius)
	{
		_center = center;
		_radius = radius;
	}
	
	#region Implementation.
	private Vector3 _center;
	private float _radius;
	#endregion
}
