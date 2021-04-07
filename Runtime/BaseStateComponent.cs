using UnityEngine;

namespace ScriptableStateMachine.Runtime
{
	public abstract class BaseStateComponent : ScriptableObject
	{
		public StateMachine StateMachine { get; set; }

		protected GameObject GameObject => StateMachine.gameObject;

		protected Transform Transform => StateMachine.transform;

#if UNITY_EDITOR
		public string DebugInfo { get; protected set; }
#endif
	}
}