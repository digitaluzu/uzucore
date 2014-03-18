using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Allows weighted-random selection of a list of objects.
	/// </summary>
	public class WeightedRandomSelector
	{
		private FixedList <float> _weights;
		private float _totalWeight;

		public WeightedRandomSelector (FixedList <float> weights)
		{
			Reinitialize (weights);
		}

		/// <summary>
		/// Reinitialize the specified weights for this selector.
		/// This allows the selector object to be re-used without
		/// having to re-allocate a new one.
		/// </summary>
		public void Reinitialize (FixedList <float> weights)
		{
			_weights = weights;
			_totalWeight = 0.0f;

			// Calculate total weight.
			{
				for (int i = 0; i < _weights.Count; i++) {
					_totalWeight += _weights [i];
				}
			}
		}

		/// <summary>
		/// Selected an item using weighted random calculation
		/// and return its index.
		/// </summary>
		public int SelectIndex ()
		{
			// All weights are zero.
			if (Mathf.Approximately(_totalWeight, 0.0f)) {
				return _weights.Count;
			}
			
			float rnd = Random.value * _totalWeight;
			
			// Since Unity RNG sometimes generates 1.0f, we need to handle this case.
			if (Mathf.Approximately (rnd, _totalWeight)) {
				rnd *= 0.5f;
			}
			
			for (int i = 0; i < _weights.Count; i++) {
				rnd -= _weights [i];
				if (rnd < 0.0f) {
					return i;
				}
			}

			// Should never get here.
			Debug.LogWarning ("Unable to calculate weighted random.");
			return _weights.Count;
		}
	}
}