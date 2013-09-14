using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base plugin class.
/// Not implemented as an interface, because we don't
/// want to force usage of all the defined methods.
/// </summary>
public class LGPlugin
{
	public virtual void OnStart ()
	{
	}

	public virtual void OnUpdate ()
	{
	}
}

/// <summary>
/// Interface for a collection of plugins.
/// </summary>
public interface LGIPluginHub
{
	void AddPlugin (LGPlugin plugin);

	T FindPlugin<T> () where T : LGPlugin;
	
	void OnStartPlugins ();

	void OnUpdatePlugins ();
}

/// <summary>
/// Basic plugin hub implementation helper.
/// </summary>
public sealed class LGPluginHubHelper
{
	private List<LGPlugin> _plugins = new List<LGPlugin> ();
	
	/// <summary>
	/// Add a plugin.
	/// </summary>
	public void AddPlugin (LGPlugin plugin)
	{
		_plugins.Add (plugin);
	}
	
	/// <summary>
	/// Find plugin based on type T.
	/// </summary>
	public T FindPlugin<T> () where T : LGPlugin
	{
		foreach (LGPlugin plugin in _plugins) {
			if (plugin is T) {
				return plugin as T;
			}
		}
		return null;
	}
	
	public void OnStartPlugins ()
	{
		foreach (LGPlugin plugin in _plugins) {
			plugin.OnStart ();
		}
	}
	
	public void OnUpdatePlugins ()
	{
		foreach (LGPlugin plugin in _plugins) {
			plugin.OnUpdate ();
		}
	}
}