using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class GameEventInteractActivator : GameEventBaseActivator
    {
        [SerializeField] private bool disableTriggersAfterUsage;
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;

        private bool _isPlayerInside;
        private bool _disabled = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_disabled)
            {
                _isPlayerInside = true;
                onEnter?.Invoke();
            }
        }

        private void Interact()
        {
            if (!_disabled && _isPlayerInside)
            {
                Raise();

                if (!_wasSuccesful)
                    return;

                if (disableTriggersAfterUsage)
                {
                    DisableEvent();
                    _disabled = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            DisableEvent();
        }

        private void DisableEvent()
        {
            if (!_disabled)
            {
                onExit?.Invoke();
                _isPlayerInside = false;
            }
        }

        private void OnEnable()
        {
            Player.onInteractInput += Interact;
        }

        private void OnDisable()
        {
            Player.onInteractInput -= Interact;            
        }
    }
}
