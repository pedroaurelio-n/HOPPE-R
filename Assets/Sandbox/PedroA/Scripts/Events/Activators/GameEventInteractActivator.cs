using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class GameEventInteractActivator : GameEventBaseActivator
    {
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;
        [SerializeField] private bool disableAfterUsage;

        private bool _disabled = false;

        private Player _player;

        private void OnTriggerEnter(Collider other)
        {
            if (!_disabled)
            {
                _player = other.GetComponent<Player>();
                onEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_disabled && _player.Input.PlayerActions.Interact.IsPressed())
            {
                Raise();

                if (disableAfterUsage)
                {
                    OnTriggerExit(other);
                    _disabled = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_disabled)
            {
                onExit?.Invoke();
                _player = null;
            }
        }
    }
}
