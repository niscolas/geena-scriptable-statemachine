using ScriptableStateMachine.Runtime.Graph;
using ScriptableStateMachine.Utilities;
using UnityEngine;

namespace ScriptableStateMachine.Runtime
{
	public partial class StateMachine : MonoBehaviour
	{
		public const string StateMachineAssetMenuPathBase = "State Machine/";
		public const string StateActionsAssetMenuPath = StateMachineAssetMenuPathBase + "Actions/";
		public const string StateDecisionsAssetMenuPath = StateMachineAssetMenuPathBase + "Decisions/";
		public const string StateEventsAssetMenuPath = StateMachineAssetMenuPathBase + "Events/";

		[SerializeField]
		private StateMachineGraph stateMachineGraph;

		private bool wasInitialized;

		public StateNode CurrentStateNode { get; private set; }

		public bool[] PreviousDecisions { get; private set; }

		protected void Initialize()
		{
			if (!CurrentStateNode)
			{
				if (stateMachineGraph)
				{
					CurrentStateNode = stateMachineGraph.TryGetNoInputNode<StateNode>();
				}

				if (!CurrentStateNode)
				{
					return;
				}
			}

			InitializeTransitionsData(CurrentStateNode);
			CurrentStateNode.CheckTriggers(this, StateTrigger.OnMachineStart);
			CurrentStateNode.CheckTriggers(this, StateTrigger.OnStateEnter);

			wasInitialized = true;
		}

		private void InitializeTransitionsData(StateNode stateNode)
		{
			PreviousDecisions = new bool[stateNode.Transitions.Length];
		}

		protected void Run()
		{
			if (!CurrentStateNode || !wasInitialized)
			{
				return;
			}

			CurrentStateNode.StateMachine = this;
			CurrentStateNode.UpdateActions(this);
			CurrentStateNode.CheckTransitions(this);
		}

		public void ChangeState(StateNode nextStateNode)
		{
			if (!nextStateNode)
			{
				return;
			}

			CurrentStateNode.CheckTriggers(this, StateTrigger.OnStateExit);

			CurrentStateNode = nextStateNode;

			InitializeTransitionsData(nextStateNode);
			nextStateNode.CheckTriggers(this, StateTrigger.OnStateEnter);
		}
	}
}