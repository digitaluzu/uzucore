using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Base plugin interface.
	/// </summary>
	public interface PluginInterface
	{
		void OnStart ();

		void OnUpdate ();
	}
	
	/// <summary>
	/// Interface for a collection of plugins.
	/// </summary>
	public interface PluginHubInteface
	{
		void AddPlugin (PluginInterface plugin);
	
		T FindPlugin<T> () where T : PluginInterface;
		
		void OnStartPlugins ();
	
		void OnUpdatePlugins ();
	}
	
	/// <summary>
	/// Basic plugin hub implementation helper.
	/// </summary>
	public sealed class PluginHubHelper
	{
		private List<PluginInterface> _plugins = new List<PluginInterface> ();
		
		/// <summary>
		/// Add a plugin.
		/// </summary>
		public void AddPlugin (PluginInterface plugin)
		{
			_plugins.Add (plugin);
		}
		
		/// <summary>
		/// Find plugin based on type T.
		/// </summary>
		public T FindPlugin<T> () where T : class, PluginInterface
		{
			for (int i = 0; i < _plugins.Count; i++) {
				T plugin = _plugins [i] as T;
				if (plugin != null) {
					return plugin;
				}
			}
			return null;
		}
		
		public void OnStartPlugins ()
		{
			foreach (PluginInterface plugin in _plugins) {
				plugin.OnStart ();
			}
		}
		
		public void OnUpdatePlugins ()
		{
			foreach (PluginInterface plugin in _plugins) {
				plugin.OnUpdate ();
			}
		}
	}
}