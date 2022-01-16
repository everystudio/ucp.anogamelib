using UnityEngine;
using System.Collections;
using UnityEngine.Events;
#pragma warning disable CS0649
namespace anogamelib
{
    [AddComponentMenu("Events/EventBoolListener")]
    public class EventBoolListener : ScriptableEventListener<bool>
    {
        [SerializeField]
        protected EventBool eventObject;

        private UnityEventBool eventAction = new UnityEventBool();

        [SerializeField]
        private UnityEventBool onResult;

        [SerializeField]
        private UnityEvent onTrue;

        [SerializeField]
        private UnityEvent onFalse;

        private void Awake()
        {
            eventAction.AddListener((_state) =>
            {
                onResult.Invoke(_state);
                if (_state == true)
                {
                    onTrue.Invoke();
                }
                else
                {
                    onFalse.Invoke();
                }
            });
        }

        protected override ScriptableEvent<bool> ScriptableEvent
        {
            get
            {
                return eventObject;
            }
        }

        protected override UnityEvent<bool> Action
        {
            get
            {
                return eventAction;
            }
        }
    }
}
