using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Uzu
{
	#if UNITY_4_5
	/// <summary>
	/// Adds alpha-numeric sorting to editor hierarchy window.
	/// This is the pre-Unity 4.5 default behaviour.
	/// </summary>
	public class AlphaNumericSort : BaseHierarchySort
	{
		public override int Compare(GameObject lhs, GameObject rhs)
		{
			if (lhs == rhs) return 0;
			if (lhs == null) return -1;
			if (rhs == null) return 1;
			return EditorUtility.NaturalCompare(lhs.name, rhs.name);
		}
	}
	#endif
}
