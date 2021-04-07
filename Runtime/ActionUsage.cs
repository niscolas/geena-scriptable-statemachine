using System;
using UnityEngine;

namespace ScriptableStateMachine.Runtime
{
	[Serializable]
	public class ActionUsage : LogicAndTrigger
	{
		[SerializeField]
		private StateTrigger trigger;

		[SerializeField]
		private StateAction logic;

		public StateTrigger Trigger => trigger;

		public StateAction Logic => logic;
	}
}