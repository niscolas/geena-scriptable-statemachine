using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableStateMachine.Runtime.Graph
{
	[Serializable]
	public class StateTransition
	{
		private static readonly StateTrigger[] UnusedDecisionsStateTriggers =
			{StateTrigger.OnMachineStart, StateTrigger.OnStateUpdate};

		[SerializeField]
		private bool enabled = true;

		[SerializeField]
		private Decision[] decisions;

		[SerializeField]
		private DecisionAction[] actions;

		public bool Enabled => enabled;

		private IEnumerable<Decision> Decisions => decisions;

		public IEnumerable<DecisionAction> Actions => actions;

		public void CheckDecisionsTriggers(StateMachine stateMachine, StateTrigger trigger)
		{
			if (UnusedDecisionsStateTriggers.Contains(trigger))
			{
				return;
			}

			foreach (Decision decision in Decisions)
			{
				if (!decision.Logic)
				{
					continue;
				}

				decision.Logic.StateMachine = stateMachine;

				switch (trigger)
				{
					case StateTrigger.OnStateEnter:
						decision.Logic.OnEnter();
						break;

					case StateTrigger.OnStateExit:
						decision.Logic.OnExit();
						break;
				}
			}
		}

		public bool CheckDecisions(StateMachine stateMachine)
		{
			bool finalDecision = true;

#if UNITY_EDITOR
			string debugText = string.Empty;
#endif

			foreach (Decision decision in Decisions)
			{
				StateDecision stateDecision = decision.Logic;
				if (!stateDecision)
				{
					continue;
				}

				stateDecision.StateMachine = stateMachine;
				bool decided = stateDecision.Decide();

#if UNITY_EDITOR
				debugText += $"[{stateDecision.name}: {decided} - {stateDecision.DebugInfo}]\n";
#endif

				switch (decision.Trigger)
				{
					case DecisionTrigger.When:
						break;

					case DecisionTrigger.Not:
						decided = !decided;
						break;

					default:
						throw new NotImplementedException();
				}

				if (!decided)
				{
					finalDecision = false;
				}
			}

			foreach (Decision decision in Decisions)
			{
				StateDecision state = decision.Logic;
				if (!state)
				{
					continue;
				}

				state.AfterDecision(finalDecision);
			}

			return finalDecision;
		}
	}
}