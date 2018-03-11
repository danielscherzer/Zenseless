using Microsoft.Win32;
using OpenTK;
using System;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// 
	/// </summary>
	public static class RegistryLoader
	{
		/// <summary>
		/// Loads the layout.
		/// </summary>
		/// <param name="window">The window.</param>
		public static void LoadLayout(this GameWindow window)
		{
			RegistryKey keyApp = RegistryLoaderForm.GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey("GameWindow");
			if (key is null) return;
			window.WindowState = (WindowState)Convert.ToInt32(key.GetValue("WindowState", (int)window.WindowState));
			window.Visible = Convert.ToBoolean(key.GetValue("visible", window.Visible));
			window.Width = Convert.ToInt32(key.GetValue("Width", window.Width));
			window.Height = Convert.ToInt32(key.GetValue("Height", window.Height));
			window.X = Convert.ToInt32(key.GetValue("X", window.X));
			window.Y = Convert.ToInt32(key.GetValue("Y", window.Y));
		}

		/// <summary>
		/// Saves the layout.
		/// </summary>
		/// <param name="window">The window.</param>
		public static void SaveLayout(this GameWindow window)
		{
			RegistryKey keyApp = RegistryLoaderForm.GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey("GameWindow");
			if (key is null) return;
			key.SetValue("WindowState", (int)window.WindowState);
			key.SetValue("visible", window.Visible);
			key.SetValue("Width", window.Width);
			key.SetValue("Height", window.Height);
			key.SetValue("X", window.X);
			key.SetValue("Y", window.Y);
		}

		/// <summary>
		/// Loads the value.
		/// </summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static object LoadValue(string keyName, string name, object defaultValue)
		{
			RegistryKey keyApp = RegistryLoaderForm.GetAppKey();
			if (keyApp is null) return null;
			var key = keyApp.CreateSubKey(keyName);
			if (key is null) return null;
			return key.GetValue(name, defaultValue);
		}

		/// <summary>
		/// Saves the value.
		/// </summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public static void SaveValue(string keyName, string name, object value)
		{
			RegistryKey keyApp = RegistryLoaderForm.GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey(keyName);
			if (key is null) return;
			key.SetValue(name, value);
		}
	}
}
