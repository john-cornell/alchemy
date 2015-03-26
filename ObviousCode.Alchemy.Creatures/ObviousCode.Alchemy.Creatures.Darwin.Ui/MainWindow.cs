using System;
using Gtk;
using System.Collections.Generic;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Creatures.Darwin;
using System.Linq;
using Evaluator = ObviousCode.Alchemy.Creatures.Darwin.Environment;
using System.Threading.Tasks;
using ObviousCode.Alchemy.Creatures.Darwin.Ui;
using ObviousCode.Alchemy.Creatures;
using ObviousCode.Alchemy.Library.Populous;

public partial class MainWindow: Gtk.Window
{
	GenerationRunner _process;
	Evaluator _currentEvaluator;

	static string _label_random;

	bool _running = false;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();						

		_process = new GenerationRunner ();

		_process.ProcessesStopped += (sender, e) => {

			if (e.ReasonForStopping == ProcessingStoppedEventArgs.HaltReason.Exception) {
				
				Invoke (() => {
					MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, e.Exception.Message);
					md.Run ();
					md.Destroy ();

					_action.Sensitive = false;
					_action.Label = "Exception";

					_environments.Active = -1;
				});

			} else {
				Invoke (() => {
					_generationLabel.Text = _currentEvaluator.Generations.ToString ();

					_running = false;
					_action.Label = "Go";
					_action.Sensitive = true;
				});
			}				
		};

		LoadEnvironments ();

		_environments.Active = 0;
		_resetWarning.Visible = false;
	}

	void HandleFitnessSelectionAvailable (object sender, AfterSelectionStateEventArgs e)
	{
		Invoke (() => {
			
			Individual<byte> i1 = e.Selection [0];
			Individual<byte> i2 = e.Selection [1];

			Creature c1 = e.LastPopulation [i1.Uid];
			Creature c2 = e.LastPopulation [i2.Uid];

			int MaxLength = 8;

			_fitness1.Text = Limit (c1.Fitness.ToString (), MaxLength);
			_fitness2.Text = Limit (c2.Fitness.ToString (), MaxLength);

			_energy1.Text = c1.Energy.ToString ();
			_state1.Text = c1.CauseOfDeath.ToString ();
			_max1.Text = c1.MaximumEnergy.ToString ();
			_dCost1.Text = c1.DigestionCost.ToString ();
			_eCost1.Text = c1.EnzymeProcessCost.ToString ();
			_enzC1.Text = c1.Enzymes.Count.ToString ();
			_ex1.Text = Limit (c1.EnergyExtractionRatio.ToString (), MaxLength);
			_fEnz1.Text = c1.Enzymes [0].ToString ();
			_sEnz1.Text = c1.Enzymes.Count > 1 ? c1.Enzymes [1].ToString () : "N/A";

			_energy2.Text = c2.Energy.ToString ();
			_state2.Text = c2.CauseOfDeath.ToString ();
			_max2.Text = c2.MaximumEnergy.ToString ();
			_dCost2.Text = c2.DigestionCost.ToString ();
			_eCost2.Text = c2.EnzymeProcessCost.ToString ();
			_enzC2.Text = c2.Enzymes.Count.ToString ();
			_ex2.Text = Limit (c2.EnergyExtractionRatio.ToString (), MaxLength);
			_fEnz2.Text = c2.Enzymes [0].ToString ();
			_sEnz2.Text = c2.Enzymes.Count > 1 ? c2.Enzymes [1].ToString () : "N/A";
		});
	}

	public string Limit (string value, int maxLength)
	{
		if (value.Length > maxLength)
			value = value.Substring (0, maxLength);

		return value;
	}

	void HandleNextGenerationAvailable (object sender, PopulationEventArgs e)
	{
		Invoke (() => {
			_generationLabel.Text = e.Generation.ToString ();
		});
	}

	void StopProcess ()
	{
		_running = false;

		_action.Sensitive = false;

		_process.RequestStop ();

		_action.Label = "Stopping ...";
	}

	void StartProcess ()
	{
		_running = true;

		_action.Label = "Stop";

		_currentEvaluator.LifetimeIterations = (int)_lifetimeIterations.Value;

		_process.IterateGenerations (_currentEvaluator, (int)_generationsToRun.Value);
	}

	void LoadEnvironments ()
	{
		_label_random = new Environment_RandomNumber ().Label;

		_environments.Model = new ListStore (typeof(string));

		(_environments.Model as ListStore)
			.AppendValues (
			new string[] {
				_label_random
			}
		);

		_environments.Changed += (object sender, EventArgs e) => {
			_action.Sensitive = !string.IsNullOrEmpty ((_environments as ComboBox).ActiveText);

			if (_action.Sensitive) {
				LoadCurrentEvaluator ();
			}
		};
	}

	void LoadCurrentEvaluator ()
	{
		if (_currentEvaluator != null) {
			UnloadCurrentEvaluator ();
		}

		if (!string.IsNullOrEmpty ((_environments as ComboBox).ActiveText)) {
			string request = ((_environments as ComboBox).ActiveText);

			if (request == _label_random) {
				
				int population = (int)_population.Value;
				int genomeLength = (int)_genomeLength.Value;

				_currentEvaluator = new Environment_RandomNumber ((e) => {
					e.Setup (population, genomeLength);
				});						
			}
		}

		if (_currentEvaluator != null) {
			_currentEvaluator.AfterSelectionStateAvailable += HandleFitnessSelectionAvailable;
			_currentEvaluator.NextGenerationAvailable += HandleNextGenerationAvailable;

			_action.Label = "Go";
		}
	}

	void UnloadCurrentEvaluator ()
	{
		_currentEvaluator.AfterSelectionStateAvailable -= HandleFitnessSelectionAvailable;
		_currentEvaluator.NextGenerationAvailable -= HandleNextGenerationAvailable;

		_action.Label = "Choose Environment";
	}

	protected void Action_Clicked (object sender, EventArgs e)
	{
		try {
			if (_running) {
				StopProcess ();
			} else {
				StartProcess ();
			}
		} catch (Exception ex) {

		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		LoadCurrentEvaluator ();

		_generationLabel.Text = "0";

		_resetWarning.Visible = false;
	}

	public void Invoke (System.Action action)
	{
		GLib.Idle.Add (new GLib.IdleHandler (() => {
			action ();
			return false;
		}));
	}

	protected void OnButton5Clicked (object sender, EventArgs e)
	{
		Destroy ();
	}

	protected void ShowResetWarning (object o, EventArgs args)
	{
		_resetWarning.Visible = true;
	}
}
	