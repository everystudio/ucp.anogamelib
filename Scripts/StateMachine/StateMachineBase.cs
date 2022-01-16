using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace anogamelib
{
	public abstract class StateMachineBase<T> : MonoBehaviour where T : StateMachineBase<T>
	{
		protected StateBase<T> stateCurrent;
		protected StateBase<T> stateNext;
		/*
		 * しばらく使わないのでカット
		public UnityEvent OnBegin = new UnityEvent();
		public UnityEvent OnEnd = new UnityEvent();
		*/
		protected virtual void OnUpdatePrev() { }
		protected virtual void OnUpdateAfter() { }

		protected virtual void OnFixedUpdatePrev() { }
		protected virtual void OnFixedUpdateAfter() { }

		public void SetState(StateBase<T> _state)
		{
			stateNext = _state;
			/*
			if (stateCurrent != null)
			{
				stateCurrent.OnExitState();
			}
			stateCurrent = _state;
			stateCurrent.OnEnterState();
			StartCoroutine(stateCurrent.OnEnterStateEnumerator());
			*/
		}
		private void Update()
		{
			OnUpdatePrev();
			if( stateCurrent != stateNext)
			{
				if(stateCurrent != null)
				{
					stateCurrent.OnExitState();
				}
				stateCurrent = stateNext;
				if (stateCurrent != null)
				{
					stateCurrent.OnEnterState();
					StartCoroutine(stateCurrent.OnEnterStateEnumerator());
				}
			}

			if (stateCurrent != null)
			{
				stateCurrent.OnUpdateState();
			}
			OnUpdateAfter();
		}

		private void FixedUpdate()
		{
			OnFixedUpdatePrev();
			if (stateCurrent != null)
			{
				stateCurrent.OnFixedUpdateState();
			}
			OnFixedUpdateAfter();
		}

	}
}