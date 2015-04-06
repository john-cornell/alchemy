using System;
using Gtk;
using System.IO;

namespace ObviousCode.Alchemy.Creatures.Darwin.Ui
{
	public static class CreatureLoader
	{
		public static byte[] Load ()
		{
			byte[] loaded = null;

			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog ("Select a creature dna file to restore",
					null,
					FileChooserAction.Open,
					"Load", ResponseType.Accept,
					"Cancel", ResponseType.Cancel);

			if (filechooser.Run () == (int)ResponseType.Accept) {
				loaded = File.ReadAllBytes(filechooser.Filename);
			}

			filechooser.Destroy ();

			return loaded;
		}
	}
}

