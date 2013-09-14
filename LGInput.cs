using UnityEngine;
using System.Collections;

using UEInput = UnityEngine.Input;

/// <summary>
/// Input related services.
/// 
/// SHYAM_TODO: I'd like ALL of our input code to have a layer of indirection
/// 			between the Unity API and the application. This will allow us to
/// 			change platforms easier. Need to look into how to do this best...
/// </summary>
public class LGInput
{
	private static readonly int LMB = 0;
	private static readonly int RMB = 1;
//	private static readonly int MMB = 2;
	
	/// <summary>
	/// Is there a touch event (or LMB event)?
	/// </summary>
	public static bool IsTouch()
	{
		Vector3 pos;
		return GetTouch(out pos);
	}

	/// <summary>
	/// Gets the position of a touch event (or LMB event).
	/// Looks at all phases of the touch event.
	/// </summary>
	public static bool GetTouch (out Vector3 position)
	{
		// SHYAM_TODO: we also have access to UNITY_EDITOR and UNITY_IPHONE,
		//			   but unfortunately they are both defined at this time.
		
		if (UEInput.touches.Length == 1) {
			Touch touch0 = UEInput.GetTouch (0);
			position = touch0.position;
			return true;
		} else if (UEInput.GetMouseButtonDown (LMB) || UEInput.GetMouseButton (LMB)) {
			position = UEInput.mousePosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}

	/// <summary>
	/// Same as GetTouch, but returns a 2d coordinate instead.
	/// </summary>
	public static bool GetTouch2d (out Vector2 position)
	{
		Vector3 pos;
		bool res = GetTouch (out pos);
		position = new Vector2 (pos.x, pos.y);
		return res;
	}

	/// <summary>
	/// Gets the position of a touch event (or LMB).
	/// Only looks at "TouchPhase.Began" events.
	/// </summary>
	public static bool GetTouchDown (out Vector3 position)
	{
		if (UEInput.touches.Length == 1) {
			Touch touch0 = UEInput.GetTouch (0);
			if (touch0.phase == TouchPhase.Began) {
				position = touch0.position;
				return true;
			}
		} else if (UEInput.GetMouseButtonDown (LMB)) {
			position = UEInput.mousePosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}

	/// <summary>
	/// Same as GetTouchDown, but returns a 2d coordinate instead.
	/// </summary>
	public static bool GetTouchDown2d (out Vector2 position)
	{
		Vector3 pos;
		bool res = GetTouchDown (out pos);
		position = new Vector2 (pos.x, pos.y);
		return res;
	}

	/// <summary>
	/// Gets the position of a two-finger touch event (or RMB event).
	/// Looks at all phases of the touch event.
	/// </summary>
	public static bool GetTouchDouble (out Vector3 position)
	{
		// SHYAM_TODO: iphone
		
//		if (UEInput.touches.Length == 1) {
//			Touch touch0 = UEInput.GetTouch (0);
//			position = touch0.position;
//			return true;
//		} else
		if (UEInput.GetMouseButtonDown (RMB) || UEInput.GetMouseButton (RMB)) {
			position = UEInput.mousePosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}

	/// <summary>
	/// Same as GetDoubleTouch, but returns a 2d coordinate instead.
	/// </summary>
	public static bool GetTouchDouble2d (out Vector2 position)
	{
		Vector3 pos;
		bool res = GetTouchDouble (out pos);
		position = new Vector2 (pos.x, pos.y);
		return res;
	}

	/// <summary>
	/// Gets the position of a two-finger touch event (or RMB).
	/// Only looks at "TouchPhase.Began" events.
	/// </summary>
	public static bool GetTouchDoubleDown (out Vector3 position)
	{
//		if (UEInput.touches.Length == 1) {
//			Touch touch0 = UEInput.GetTouch (0);
//			if (touch0.phase == TouchPhase.Began) {
//				position = touch0.position;
//				return true;
//			}
//		} else 
		if (UEInput.GetMouseButtonDown (RMB)) {
			position = UEInput.mousePosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}

	/// <summary>
	/// Same as GetTouchDoubleDown, but returns a 2d coordinate instead.
	/// </summary>
	public static bool GetTouchDoubleDown2d (out Vector2 position)
	{
		Vector3 pos;
		bool res = GetTouchDoubleDown (out pos);
		position = new Vector2 (pos.x, pos.y);
		return res;
	}
}
