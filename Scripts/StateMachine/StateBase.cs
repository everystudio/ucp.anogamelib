using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anogamelib
{
	public abstract class StateBase<T> where T : StateMachineBase<T>
	{
		protected T machine;
		public StateBase(T _machine)
		{
			machine = _machine;
		}

		public virtual void OnEnterState()
		{
		}
		public virtual IEnumerator OnEnterStateEnumerator()
		{
			yield break;
		}
		public virtual void OnUpdateState()
		{
		}
		public virtual void OnFixedUpdateState()
		{
		}
		public virtual void OnExitState()
		{
		}
	}
}