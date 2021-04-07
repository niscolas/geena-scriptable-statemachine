using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptableStateMachine.Runtime
{
	public abstract class StateAction : BaseStateComponent
	{
		[SerializeField]
		private bool hasDelay;

		[SerializeField]
		private float delay;

		public async void OnUpdate()
		{
			if (hasDelay && delay > 0)
			{
				await Task.Delay(TimeSpan.FromSeconds(delay));
				Perform();
			}
			else
			{
				Perform();
			}
		}

		protected abstract void Perform();
	}
}