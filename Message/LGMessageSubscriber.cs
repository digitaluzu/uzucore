using UnityEngine;

/// <summary>
/// Message subscriber interface.
/// Implement this interface to receive messages.
/// </summary>
public interface LGIMessageSubscriber
{
	/// <summary>
	/// Receive a message and process it.
	/// </summary>
	void OnReceiveMessage (LGMessage message);
}
