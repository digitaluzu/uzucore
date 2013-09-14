using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Message processing and subscriber handling implementation.
/// </summary>
public sealed class LGMessenger
{
	private Dictionary<System.Type, List<LGIMessageSubscriber>> _subscribers = new Dictionary<System.Type, List<LGIMessageSubscriber>> ();

	/// <summary>
	/// Subscribe to messages of a certain type T.
	/// </summary>
	public void AddSubscriber<T> (LGIMessageSubscriber subscriber)
	{
		if (subscriber == null) {
			return;
		}
		
		System.Type messageType = typeof(T);
		
		List<LGIMessageSubscriber> subscriberList;
		
		if (_subscribers.TryGetValue (messageType, out subscriberList)) {
			// Check for duplicates.
			if (subscriberList.Contains (subscriber)) {
				return;
			}
			
			// Add subscriber to existing list.
			subscriberList.Add (subscriber);
		} else {
			// List doesn't exist yet, so create it and add subscriber.
			subscriberList = new List<LGIMessageSubscriber> ();
			subscriberList.Add (subscriber);
			
			_subscribers.Add (messageType, subscriberList);
		}
	}

	/// <summary>
	/// Send a message.
	/// </summary>
	public void SendMessage (LGMessage message)
	{
		if (message == null) {
			return;
		}
		
		System.Type messageType = message.GetType ();
		
		List<LGIMessageSubscriber> subscriberList;
		bool wasReceived = false;
		
		if (_subscribers.TryGetValue (messageType, out subscriberList)) {
			foreach (LGIMessageSubscriber subscriber in subscriberList) {
				// Dispatch message.
				subscriber.OnReceiveMessage (message);
				
				// Received by at least one subscriber.
				wasReceived = true;
			}
		}
		
		// Receiver verification.
		if (message.DoesRequireReceiver && !wasReceived) {
			Debug.LogError ("Message of type [" + messageType + "] requires receiver.");
		}
	}
}
