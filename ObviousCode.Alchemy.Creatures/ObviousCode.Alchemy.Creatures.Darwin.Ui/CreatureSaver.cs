using System;
using Gtk;
using System.IO;

namespace ObviousCode.Alchemy.Creatures.Darwin.Ui
{
	public static class CreatureSaver
	{
		public static void Save (Creature creature, string suggestedName)
		{
			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog ("Select save file",
					null,
					FileChooserAction.Save,
					"Save", ResponseType.Accept,
					"Cancel", ResponseType.Cancel);

			filechooser.CurrentName = suggestedName;

			if (filechooser.Run () == (int)ResponseType.Accept) {
				File.WriteAllBytes (filechooser.Filename, creature.Code);
			}

			filechooser.Destroy ();
		}

	}
}

