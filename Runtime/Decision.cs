using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableStateMachine.Runtime
{
	[Serializable]
	public class Decision : LogicAndTrigger
	{
		[SerializeField]
		private DecisionTrigger trigger;

		[FormerlySerializedAs("decision")]
		[SerializeField]
		private StateDecision logic;

		public DecisionTrigger Trigger => trigger;
		public StateDecision Logic => logic;
	}
}