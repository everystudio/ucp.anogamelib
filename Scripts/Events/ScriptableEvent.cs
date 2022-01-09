using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

#pragma warning disable 649
namespace anogamelib
{
    public abstract class ScriptableEvent<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private bool dispatchLastStateOnAdd;

        private List<ScriptableEventListener<T>> eventListeners = new List<ScriptableEventListener<T>>();
        private List<System.Action<T>> scriptEventListeners = new List<System.Action<T>>();

#if UNITY_EDITOR


#endif


        [System.NonSerialized]
        private T lastParameter;
        public T LastParameter
        {
            get { return lastParameter; }
        }
        [System.NonSerialized]
        private bool m_bFirstAdd = true;

        [System.NonSerialized]
        private bool hasParameter;
        public bool HasParameter
        {
            get { return hasParameter; }
        }

        public void Invoke(T param)
        {
            //Debug.Log(param);
            for (int i = scriptEventListeners.Count - 1; i >= 0; i--)
            {
                scriptEventListeners[i].Invoke(param);
            }

            //Debug.Log(eventListeners.Count);
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                //Debug.Log(eventListeners[i].gameObject.name);
                eventListeners[i].Dispatch(param);
            }

            if (dispatchLastStateOnAdd)
            {
                lastParameter = param;
                hasParameter = true;
            }
        }

        public void AddListener(System.Action<T> listener)
        {
            scriptEventListeners.Add(listener);

            if (dispatchLastStateOnAdd && hasParameter)
            {
                listener.Invoke(lastParameter);
            }
        }

        public void RemoveListener(System.Action<T> listener)
        {
            scriptEventListeners.Remove(listener);
        }

        public void AddListener(ScriptableEventListener<T> listener)
        {
            if(m_bFirstAdd == true)
			{
                eventListeners.Clear();
                m_bFirstAdd = false;
            }
            //Debug.Log(listener.gameObject.name);
            //Debug.Log(eventListeners.Count);

            if (!eventListeners.Contains(listener))
            {
                //Debug.Log(listener.gameObject.name);
                eventListeners.Add(listener);

                if (dispatchLastStateOnAdd && hasParameter)
                {
                    listener.Dispatch(lastParameter);
                }
            }
        }

        public void RemoveListener(ScriptableEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }
    }
}