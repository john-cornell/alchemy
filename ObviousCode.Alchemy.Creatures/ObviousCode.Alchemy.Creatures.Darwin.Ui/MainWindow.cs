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

	int MaxLength = 8;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();						

		_process = new GenerationRunner ();

		_process.ProcessesStopped += (sender, e) => {
					
			_reset.Sensitive = true;
		
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

	#region Generation Update

	void HandleFitnessSelectionAvailable (object sender, AfterSelectionStateEventArgs e)
	{		
		Invoke (() => {
			
			UpdateSaveAndInjectButtons (true);

			Individual<byte> i1 = e.Selection [0];
			Individual<byte> i2 = e.Selection [1];

			Creature c1 = e.LastPopulation [i1.Uid];
			Creature c2 = e.LastPopulation [i2.Uid];

			UpdateCreature1Details (c1);

			UpdateCreature2Details (c2);
		});
	}

	void UpdateCreature1Details (Creature creature)
	{
		_fitness1.Text = creature == null ? "" : Limit (creature.Fitness.ToString (), MaxLength);
		_energy1.Text = creature == null ? "" : creature.Energy.ToString ();
		_state1.Text = creature == null ? "" : creature.CauseOfDeath.ToString ();
		_dining1.Text = creature == null ? "" : creature.DiningMethod.ToString ();
		_max1.Text = creature == null ? "" : creature.MaximumEnergy.ToString ();
		_dCost1.Text = creature == null ? "" : creature.DigestionCost.ToString ();
		_eCost1.Text = creature == null ? "" : creature.EnzymeProcessCost.ToString ();
		_enzC1.Text = creature == null ? "" : creature.Enzymes.Count.ToString ();
		_ex1.Text = creature == null ? "" : Limit (creature.EnergyExtractionRatio.ToString (), MaxLength);
		_fEnz1.Text = creature == null ? "" : creature.Enzymes [0].ToString ();
		_sEnz1.Text = creature == null ? "" : creature.Enzymes.Count > 1 ? creature.Enzymes [1].ToString () : "N/A";
	}

	void UpdateCreature2Details (Creature creature)
	{
		_fitness2.Text = creature == null ? "" : Limit (creature.Fitness.ToString (), MaxLength);
		_energy2.Text = creature == null ? "" : creature.Energy.ToString ();
		_state2.Text = creature == null ? "" : creature.CauseOfDeath.ToString ();
		_dining2.Text = creature == null ? "" : creature.DiningMethod.ToString ();
		_max2.Text = creature == null ? "" : creature.MaximumEnergy.ToString ();
		_dCost2.Text = creature == null ? "" : creature.DigestionCost.ToString ();
		_eCost2.Text = creature == null ? "" : creature.EnzymeProcessCost.ToString ();
		_enzC2.Text = creature == null ? "" : creature.Enzymes.Count.ToString ();
		_ex2.Text = creature == null ? "" : Limit (creature.EnergyExtractionRatio.ToString (), MaxLength);
		_fEnz2.Text = creature == null ? "" : creature.Enzymes [0].ToString ();
		_sEnz2.Text = creature == null ? "" : creature.Enzymes.Count > 1 ? creature.Enzymes [1].ToString () : "N/A";
	}

	void HandleNextGenerationAvailable (object sender, PopulationEventArgs e)
	{
		Invoke (() => {
			_generationLabel.Text = e.Generation.ToString ();
		});
	}

	public string Limit (string value, int maxLength)
	{
		if (value.Length > maxLength)
			value = value.Substring (0, maxLength);

		return value;
	}

	#endregion

	#region Start Stop

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

		_reset.Sensitive = false;

		_process.IterateGenerations (_currentEvaluator, (int)_generationsToRun.Value);
	}

	#endregion

	#region Environments

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

		UpdateCreature1Details (null);
		UpdateCreature2Details (null);

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

	#endregion

	#region UI Events

	protected void Action_Clicked (object sender, EventArgs e)
	{
		if (_running) {
			StopProcess ();
		} else {
			StartProcess ();
		}
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnResetClicked (object sender, EventArgs e)
	{
		UpdateSaveAndInjectButtons (false);

		LoadCurrentEvaluator ();

		_generationLabel.Text = "0";

		_resetWarning.Visible = false;
	}

	protected void OnCloseClicked (object sender, EventArgs e)
	{
		Destroy ();
	}

	#endregion

	public void Invoke (System.Action action)
	{
		GLib.Idle.Add (new GLib.IdleHandler (() => {
			action ();
			return false;
		}));
	}

	void UpdateSaveAndInjectButtons (bool enable)
	{
		_save1.Sensitive =
		_save2.Sensitive =
		_inject1.Sensitive =
		_inject2.Sensitive =
		enable;
	}

	protected void ShowResetWarning (object o, EventArgs args)
	{
		_resetWarning.Visible = true;
	}
}
	