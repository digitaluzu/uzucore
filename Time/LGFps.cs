using UnityEngine;
using System.Collections;

/// <summary>
/// Displays FPS to the screen via a NGUI label object.
/// </summary>
[RequireComponent(typeof(UILabel))]
public class LGFps : MonoBehaviour
{
	// TODO: need to remove this dependency on NGUI
	private UILabel _label;
	private int _frameCount;
	private float _deltaTime;

	protected void Awake ()
	{
		_frameCount = 0;
		_deltaTime = 0;
	}
	
	protected void Start ()
	{
		_label = GetComponent<UILabel>();
		_label.text = "<FPS>";
	}
	
	protected void Update ()
	{
		_deltaTime += Time.deltaTime;
		_frameCount++;
		
		if (_deltaTime > 1.0) {
			_label.text = _frameCount.ToString ();
			_frameCount = 0;
			_deltaTime = 0;
		}
	}
}
