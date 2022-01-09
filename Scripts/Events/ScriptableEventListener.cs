using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace anogamelib
{
    [System.Serializable]
    public abstract class ScriptableEventListener<T> : MonoBehaviour
    {
        protected abstract ScriptableEvent<T> ScriptableEvent { get; }

        protected abstract UnityEvent<T> Action { get; }

        public void Dispatch(T parameter)
        {
            //Debug.Log(gameObject.name);
            Action.Invoke(parameter);
        }

        public void OnEnable()
        {
            //Debug.Log(gameObject.name);
            ScriptableEvent.AddListener(this);
        }

        public void OnDisable()
        {
            ScriptableEvent.RemoveListener(this);
        }

        /// <summary>
        /// Dispatch the last known parameter again
        /// </summary>
        public void ReDispatch()
        {
            if (ScriptableEvent.HasParameter)
            {
                Dispatch(ScriptableEvent.LastParameter);
            }
        }
    }
}
