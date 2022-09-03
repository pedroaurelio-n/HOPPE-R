using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public abstract class BaseGameEventSubscriber : MonoBehaviour
    {
        [SerializeField] private GameEvent gameEvent;
        [SerializeField] protected float delay;
        [SerializeField] protected bool isOneShot;

        protected bool _wasRaised = false;

        public void OnEventRaised()
        {
            if (_wasRaised)
                return;
            
            if (delay <= 0)
                InvokeEvent();
            else
                StartCoroutine(OnEventDelayed());
        }

        private IEnumerator OnEventDelayed()
        {
            yield return new WaitForSeconds(delay);
            InvokeEvent();
        }

        protected virtual void InvokeEvent()
        {
            if (isOneShot)
                _wasRaised = true;
        }

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);            
        }
    }
}
