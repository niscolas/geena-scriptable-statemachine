namespace ScriptableStateMachine.Runtime
{
	public abstract class StateDecision : BaseStateComponent
	{
		public abstract bool Decide();

		public virtual void OnEnter() { }

		public virtual void AfterDecision(bool decided) { }

		public virtual void OnExit() { }

		protected enum DecisionMethod
		{
			OnEnter,
			Decide,
			AfterDecision
		}
	}
}