using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KeyboardMapper.Properties;

namespace KeyboardMapper
{
	public class ApplicationContext : System.Windows.Forms.ApplicationContext
	{
		private readonly Dictionary<Keys, string> Mappings = new Dictionary<Keys, string>();

		private NotifyIcon trayIcon;
		private GlobalKeyboardHook keyhooker;

		public ApplicationContext()
		{
			LoadMappings();

			InitTrayIcon();

			InitHook();
		}

		private void LoadMappings()
		{
			var mappingsPath = Path.Combine(Process.GetCurrentProcess().StartInfo.WorkingDirectory, "Mappings.cfg");
			foreach (var line in File.ReadAllLines(mappingsPath))
			{
				var parts = line.Split('=');

				if (!Enum.TryParse(parts[0], out Keys key))
				{
					throw new Exception("Cannot parse key: " + line);
				}

				Mappings.Add(key, parts[1]);
			}
		}

		private void InitTrayIcon()
		{
			trayIcon = new NotifyIcon
			{
				Icon = Resources.Icon,
				Visible = true,
				ContextMenu = new ContextMenu(new[]
				{
					new MenuItem("Exit", Exit),
				}),
			};
		}

		private void InitHook()
		{
			keyhooker = new GlobalKeyboardHook(Mappings.Keys.ToArray(), catchAllKeyDowns: Mappings.Count == 0);
			keyhooker.KeyDown += KhOnKeyDown;
			keyhooker.Hook();
		}

		private void KhOnKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;

			if (Mappings.TryGetValue(e.KeyData, out var map))
			{
				SendKeys.Send(map);
			}
			else
			{
				MessageBox.Show($"`{e.KeyData}` has pressed");
			}
		}

		private void Exit(object sender, EventArgs e)
		{
			trayIcon.Visible = false;
			keyhooker.Unhook();

			Application.Exit();
		}
	}
}