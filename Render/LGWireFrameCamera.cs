using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this script to a camera, this will make it render in wireframe.
/// </summary>
public class LGWireFrameCamera : LGBehaviour
{
	private void OnPreRender ()
	{
		GL.wireframe = true;
	}

	private void OnPostRender ()
	{
		GL.wireframe = false;
	}
}
