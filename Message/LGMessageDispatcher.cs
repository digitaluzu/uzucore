using UnityEngine;

/// <summary>
/// Message dispatcher interface.
/// Implement this interface to become a message hub, and process
/// message dispatching.
/// </summary>
public interface LGIMessageDispatcher
{
	/// <summary>
	/// Subscribe to messages of a certain type T.
	/// </summary>
	void AddSubscriber<T> (LGIMessageSubscriber subscriber);

	/// <summary>
	/// Send a message.
	/// </summary>
	void SendMessage (LGMessage message);
}
