using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Zenseless.ExampleFramework
{
	/// <summary>
	/// 
	/// </summary>
	public static class RegistryLoaderForm
	{
		/// <summary>
		/// Gets the application key.
		/// </summary>
		/// <returns></returns>
		public static RegistryKey GetAppKey()
		{
			return System.Windows.Forms.Application.UserAppDataRegistry;
		}

		/// <summary>
		/// Loads the layout.
		/// </summary>
		/// <param name="form">The form.</param>
		public static void LoadLayout(this Form form)
		{
			RegistryKey keyApp = GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey(form.Name);
			if (key is null) return;
			form.WindowState = (FormWindowState)Convert.ToInt32(key.GetValue("WindowState", (int)form.WindowState));
			form.Visible = Convert.ToBoolean(key.GetValue("visible", form.Visible));
			form.Width = Convert.ToInt32(key.GetValue("Width", form.Width));
			form.Height = Convert.ToInt32(key.GetValue("Height", form.Height));
			var top = Convert.ToInt32(key.GetValue("Top", form.Top));
			var left = Convert.ToInt32(key.GetValue("Left", form.Left));
			if (FormTools.IsPartlyOnScreen(new Rectangle(left + 10, top + 10, 200, 10))) //check if part of the windows title bar is visible
			{
				form.Top = top;
				form.Left = left;
			}
			form.TopMost = Convert.ToBoolean(key.GetValue("TopMost", form.TopMost));
		}

		/// <summary>
		/// Saves the layout.
		/// </summary>
		/// <param name="form">The form.</param>
		public static void SaveLayout(this Form form)
		{
			RegistryKey keyApp = GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey(form.Name);
			if (key is null) return;
			key.SetValue("WindowState", (int)form.WindowState);
			key.SetValue("visible", form.Visible);
			key.SetValue("Width", form.Width);
			key.SetValue("Height", form.Height);
			key.SetValue("Top", form.Top);
			key.SetValue("Left", form.Left);
			key.SetValue("TopMost", form.TopMost);
		}

		/// <summary>
		/// Loads a value from the registry key of the application.
		/// </summary>
		/// <param name="keyName">Name of the registry key.</param>
		/// <param name="name">Name of the registry entry.</param>
		/// <param name="defaultValue">The default value of the entry.</param>
		/// <returns>value as <see cref="object"/></returns>
		public static object LoadValue(string keyName, string name, object defaultValue)
		{
			RegistryKey keyApp = GetAppKey();
			if (keyApp is null) return null;
			var key = keyApp.CreateSubKey(keyName);
			if (key is null) return null;
			return key.GetValue(name, defaultValue);
		}

		/// <summary>
		/// Saves the value to the registry key of the application.
		/// </summary>
		/// <param name="keyName">Name of the registry key.</param>
		/// <param name="name">Name of the registry entry.</param>
		/// <param name="value">The value to be stored.</param>
		public static void SaveValue(string keyName, string name, object value)
		{
			RegistryKey keyApp = GetAppKey();
			if (keyApp is null) return;
			var key = keyApp.CreateSubKey(keyName);
			if (key is null) return;
			key.SetValue(name, value);
		}
	}
}
