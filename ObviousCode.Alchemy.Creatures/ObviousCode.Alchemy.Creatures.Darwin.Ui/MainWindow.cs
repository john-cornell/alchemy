using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		LoadEnvironments ();

		Build ();
	}

	void LoadEnvironments ()
	{
		//Dictionary<string, Evaluator<byte>>

		//((ComboBox)_environments).Add(new ComboBoxEntry(
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
