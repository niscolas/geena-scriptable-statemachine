using System;
using System.Collections.Generic;
using ScriptableStateMachine.Utilities;
using UnityEngine;

namespace ScriptableStateMachine.Runtime.Graph
{
	[CreateNodeMenu("State Machine/State")]
	[NodeWidth(400)]
	public class StateNode : AStateMachineNode
	{
		[Input(ShowBackingValue.Never)]
		[SerializeField]
		private StateNode input;

		[Output(dynamicPortList = true, connectionType = ConnectionType.Override)]
		[SerializeField]
		private List<StateTransition> transitions;

		[SerializeField]
		private ActionUsage[] actions;

		public StateMachine StateMachine { get; set; }

		public IEnumerable<ActionUsage> Actions => actions;

		public StateTransition[] Transitions => transitions.ToArray();

		public void UpdateActions(StateMachine machine)
		{
			StateMachine = machine;

			foreach (ActionUsage action in Actions)
			{
				if (action.Trigger != StateTrigger.OnStateUpdate)
				{
					continue;
				}

				UpdateAction(action);
			}
		}

		public void CheckTriggers(StateMachine stateMachine, StateTrigger trigger)
		{
			foreach (ActionUsage action in Actions)
			{
				if (action.Trigger != trigger || !action.Logic)
				{
					continue;
				}

				action.Logic.StateMachine = stateMachine;
				action.Logic.OnUpdate();
			}

			foreach (StateTransition transitionNode in Transitions)
			{
				transitionNode.CheckDecisionsTriggers(stateMachine, trigger);
			}
		}

		public void CheckTransitions(StateMachine stateMachine)
		{
			int priority = int.MaxValue;

			for (int i = 0; i < Transitions.Length; i++)
			{
				StateTransition transition = Transitions[i];

				if (!transition.Enabled)
				{
					continue;
				}

				bool finalDecision = transition.CheckDecisions(stateMachine);

				if (finalDecision == stateMachine.PreviousDecisions[i])
				{
					continue;
				}

				stateMachine.PreviousDecisions[i] = finalDecision;

				if (priority > i)
				{
					priority = i;
				}
			}

			if (priority != int.MaxValue)
			{
				StateTransition transition = Transitions[priority];
				bool decision = stateMachine.PreviousDecisions[priority];

				foreach (DecisionAction action in transition.Actions)
				{
					switch (action.Trigger)
					{
						case TransitionTrigger.True:
							if (!decision)
							{
								continue;
							}

							break;

						case TransitionTrigger.False:
							if (decision)
							{
								continue;
							}

							break;

						default:
							throw new NotImplementedException();
					}

					if (!action.Logic)
					{
						continue;
					}

					action.Logic.StateMachine = stateMachine;
					action.Logic.OnUpdate();
				}

				stateMachine.ChangeState(GetNextNode(priority));
			}
		}

		private void UpdateAction(ActionUsage action)
		{
			if (!action.Logic)
			{
				Debug.Log($"{name} (StateNode) has unassigned action.", this);
				return;
			}

			action.Logic.StateMachine = StateMachine;
			action.Logic.OnUpdate();
		}

		private StateNode GetNextNode(int transitionIndex)
		{
			return (StateNode) this.GetDynamicOutputs(nameof(transitions))[transitionIndex].Connection.node;
		}
	}
}