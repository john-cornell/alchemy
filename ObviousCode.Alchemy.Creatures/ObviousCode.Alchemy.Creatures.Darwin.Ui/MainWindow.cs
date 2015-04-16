using System;
using Gtk;
using ObviousCode;
using System.Collections.Generic;
using ObviousCode.Alchemy.Library;
using ObviousCode.Alchemy.Creatures.Darwin;
using System.Linq;
using Evaluator = ObviousCode.Alchemy.Creatures.Darwin.Environment;
using System.Threading.Tasks;
using ObviousCode.Alchemy.Creatures.Darwin.Ui;
using ObviousCode.Alchemy.Creatures;
using ObviousCode.Alchemy.Library.Populous;
using ObviousCode.Alchemy.Creatures.DecisionProcessing;

public sealed partial class MainWindow: Gtk.Window
{
	GenerationRunner _process;
	Evaluator _currentEvaluator;

	static string _label_random;
	static string _label_slowlyReplenish;

	bool _running = false;

	const int MaxLength = 8;


	Creature _lastFittest1;
	Creature _lastFittest2;
	byte[] _loadedForSimulation;

	Simulator _simulator;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();						

		_process = new GenerationRunner ();

		_process.ProcessesStopped += (sender, e) => {
					
			_reset.Sensitive = true;
			UpdateControlEnablement (true);
		
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
			
			Individual<byte> i1 = e.Selection [0];
			Individual<byte> i2 = e.Selection [1];

			_lastFittest1 = e.LastPopulation [i1.Uid];
			_lastFittest2 = e.LastPopulation [i2.Uid];

			UpdateCreature1Details ();

			UpdateCreature2Details ();
		});
	}

	void UpdateCreature1Details ()
	{
		Creature creature = _lastFittest1;

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
		_ep1.Text = creature == null ? "" : creature.EatDecision.Type.ToString ();
	}

	void UpdateCreature2Details ()
	{
		Creature creature = _lastFittest2;

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
		_ep2.Text = creature == null ? "" : creature.EatDecision.Type.ToString ();
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
		_label_random = new Environment_RandomFood ().Label;
		_label_slowlyReplenish = new Environment_FoodSlowlyReplenish ().Label;

		_environments.Model = new ListStore (typeof(string));

		(_environments.Model as ListStore)
			.AppendValues (
			new string[] {
				_label_random
			}
		);

		(_environments.Model as ListStore)
			.AppendValues (
			new string[] {
				_label_slowlyReplenish
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

		_lastFittest1 = null;
		_lastFittest2 = null;

		UpdateCreature1Details ();
		UpdateCreature2Details ();

		if (!string.IsNullOrEmpty ((_environments as ComboBox).ActiveText)) {
			string request = ((_environments as ComboBox).ActiveText);

			int population = (int)_population.Value;
			int genomeLength = (int)_genomeLength.Value;

			if (request == _label_random) {
							
				_currentEvaluator = new Environment_RandomFood ((e) => e.Setup (population, genomeLength));						
			} else if (request == _label_slowlyReplenish) {

				_currentEvaluator = new Environment_FoodSlowlyReplenish ((e) => e.Setup (population, genomeLength));						
			}
		}

		if (_currentEvaluator != null) {
			_currentEvaluator.AfterSelectionStateAvailable += HandleFitnessSelectionAvailable;
			_currentEvaluator.NextGenerationAvailable += HandleNextGenerationAvailable;

			if (_currentEvaluator is IRemainingFoodReporter) {
				
				Action<int> handleRemainingFood = (f) => Invoke (() => _foodLabel.Text = f.ToString ());

				(_currentEvaluator as IRemainingFoodReporter).ReportRemainingFood = handleRemainingFood;
			}

			_action.Label = "Go";
		}
	}

	void UnloadCurrentEvaluator ()
	{
		_currentEvaluator.AfterSelectionStateAvailable -= HandleFitnessSelectionAvailable;
		_currentEvaluator.NextGenerationAvailable -= HandleNextGenerationAvailable;

		_currentEvaluator.Reset ();

		_action.Label = "Choose Environment";
	}

	#endregion

	#region UI Events


	private void OnActionClicked (object sender, EventArgs e)
	{
		UpdateControlEnablement (false);

		if (_running) {
			StopProcess ();
		} else {
			StartProcess ();
		}
	}

	private void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	private void OnResetClicked (object sender, EventArgs e)
	{
		UpdateControlEnablement (false);

		LoadCurrentEvaluator ();

		_generationLabel.Text = "0";

		_resetWarning.Visible = false;
	}

	private void OnCloseClicked (object sender, EventArgs e)
	{
		Destroy ();
	}

	private void OnSave1Clicked (object sender, EventArgs e)
	{
		CreatureSaver.Save (_lastFittest1, "creature.dna");
	}

	private void OnSave2Clicked (object sender, EventArgs e)
	{
		CreatureSaver.Save (_lastFittest2, "creature.dna");
	}

	private void OnInject1Clicked (object sender, EventArgs e)
	{
		Incubator incubator = new Incubator ();

		byte[] dna = CreatureLoader.Load ();

		if (dna != null) {

			_currentEvaluator.RequestInjection (dna, 0);

			_lastFittest1 = incubator.Incubate (dna);

			UpdateCreature1Details ();
		}
	}

	private void OnInject2Clicked (object sender, EventArgs e)
	{
		Incubator incubator = new Incubator ();

		byte[] dna = CreatureLoader.Load ();

		if (dna != null) {

			_currentEvaluator.RequestInjection (dna, 1);

			_lastFittest2 = incubator.Incubate (dna);

			UpdateCreature2Details ();
		}
	}

	#endregion

	public void Invoke (System.Action action)
	{
		GLib.Idle.Add (new GLib.IdleHandler (() => {
			action ();
			return false;
		}));
	}

	void UpdateControlEnablement (bool enable)
	{
		_save1.Sensitive =
		_save2.Sensitive =
		_inject1.Sensitive =
		_inject2.Sensitive =
		_decisionProcessing.Sensitive =
		enable;

		if (_lastFittest1 != null && _decisionProcessing.Sensitive) {
			_selection1rb.Active = true;
			LoadEatDecision (_lastFittest1);
		}
	}

	private void ShowResetWarning (object o, EventArgs args)
	{
		_resetWarning.Visible = true;
	}

	private void OnSelection1rbClicked (object sender, EventArgs e)
	{
		LoadEatDecision (_lastFittest1);
	}

	private void OnSelection2rbClicked (object sender, EventArgs e)
	{
		LoadEatDecision (_lastFittest2);
	}

	void LoadEatDecision (Creature creature)
	{		
		Decisions decisions = creature.DecisionsLookup [Creature.DecisionTypes.Eat];

		decisions.ClearTransientValues ();
		decisions.LoadTransientValue (new Value (creature.Energy, "Current Energy"));

		_eatDecisionText.Buffer.Text = decisions.GetDecisionProvider (creature.DecisionPredicateIndex_Eat).Describe ();
	}

	private void OnButton1Clicked (object sender, EventArgs e)
	{
		Simulate (1);
	}

	bool _continueSimulation = true;

	void Simulate (int iterations)
	{
		if (_loadedForSimulation == null) {
			DisplayDialog ("No simulation loaded");
			return;
		}

		_continueSimulation = true;

		_simDescription.Buffer.Clear ();	

		SimWrite ("Start ...");

		for (int i = 0; i < iterations; i++) {
			SimWrite ("Iteration '{0}' ...", i);
			_simulator.RunTurn (1);
			if (!_continueSimulation)
				break;
		}

		SimWrite ("... Completed");
	}

	private void OnLoadSim1Clicked (object sender, EventArgs e)
	{
		

		_loadedForSimulation = _lastFittest1.Code;

		_loadedSimulatorLabel.Text = "Selection 1";

		PrepareSimulator ();	
	}

	private void OnLoadSim2Clicked (object sender, EventArgs e)
	{
		_loadedForSimulation = _lastFittest2.Code;
	
		_loadedSimulatorLabel.Text = "Selection 2";

		PrepareSimulator ();
	}

	void PrepareSimulator ()
	{
		_simulator = new Simulator (_currentEvaluator, _loadedForSimulation);

		_simulate1Turn.Sensitive = true;

		_simulator.ActualEnergyExtracted += (sender, e) => SimWrite ("Actual Energy Extracted = '{0}'", e.Item);
		_simulator.CreatureAboutToDigest += (sender, e) => SimWrite ("Attempt to Digest '{0}' ({1})", e.Item, Convert.ToString (e.Item, 2));
		_simulator.CreatureDigestionCostRemoved += (sender, e) => SimWrite ("Digestion Cost '{0}', Energy = '{1}'", e.Item, _simulator.SimulatedCreature.Energy);
		_simulator.CreatureDied += (sender, e) => {
			SimWrite ("Died. Cause '{0}'", _simulator.SimulatedCreature.CauseOfDeath);
			_continueSimulation = false;
		};
		_simulator.CreatureEnzymeCostRemoved += (sender, e) => SimWrite ("Enzyme Process Cost '{0}', Energy = '{1}'", e.Item, _simulator.SimulatedCreature.Energy);
		_simulator.EatDecisionPredicatePrepared += (sender, e) => {
			SimWrite ("Eat Decision Predicate Prepared. '{0}'", e.Item.Type);
			SimWrite (e.Item.Describe ());
		};

		_simulator.EatDecisionResolved += (sender, e) => SimWrite ("Decision To Eat: '{0}'", e.Item);
		_simulator.EnzymeEvaluated += (sender, e) => SimWrite ("Enzyme evaluated: Enzyme '{0}' ({1}), extracts '{2}' ({3}). Success '{4}'", e.Item1, Convert.ToString (e.Item1, 2), e.Item2, Convert.ToString (e.Item2, 2), e.Item3);
		_simulator.EnzymeProcessCompleted += (sender, e) => SimWrite ("Enzyme complete, food remaining '{0}' ({1}), Energy '{2}'", e.Item, Convert.ToString (e.Item, 2), _simulator.SimulatedCreature.Energy);
		_simulator.PotentialExtractionCalcaulated += (sender, e) => SimWrite ("Potential Energy Extracted = '{0}' ({1})", e.Item, Convert.ToString (e.Item, 2));
	}

	private void SimWrite (string message, params object[] items)
	{
		_simDescription.Buffer.Text += string.Format (message, items);
		_simDescription.Buffer.Text += System.Environment.NewLine;
	}

	void DisplayDialog (string message)
	{
		System.Action action = () => {
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, message);
			md.Run ();
			md.Destroy ();
		};

		Invoke (action);
	}
}
	