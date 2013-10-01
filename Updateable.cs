using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Interface for all updateable objects that are managed by UpdateableMgr.
	/// </summary>
	public interface UpdateableInterface
	{
		void OnUpdate ();
	}
}