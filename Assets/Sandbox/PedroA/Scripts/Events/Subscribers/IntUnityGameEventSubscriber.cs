using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class IntUnityGameEventSubscriber : BaseGameEventSubscriber
    {
        [SerializeField] private IntUnityEvent response;

        public void OnEventRaised(int value)
        {
            if (_wasRaised)
                return;
            
            if (delay <= 0)
                InvokeEvent(value);
            else
                StartCoroutine(OnEventDelayed(value));
        }

        private IEnumerator OnEventDelayed(int value)
        {
            yield return new WaitForSeconds(delay);
            InvokeEvent();
        }

        protected virtual void InvokeEvent(int value)
        {
            response.Invoke(value);

            if (isOneShot)
                _wasRaised = true;
        }
    }
}
