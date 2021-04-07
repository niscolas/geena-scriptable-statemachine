using System;
using UnityEngine;

namespace ScriptableStateMachine.Runtime
{
	[Serializable]
	public class DecisionAction : LogicAndTrigger
	{
		[SerializeField]
		private TransitionTrigger trigger;

		[SerializeField]
		private StateAction logic;

		public TransitionTrigger Trigger => trigger;

		public StateAction Logic => logic;
	}
}