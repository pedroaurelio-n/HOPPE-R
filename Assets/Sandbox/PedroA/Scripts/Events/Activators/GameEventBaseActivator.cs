using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class GameEventBaseActivator : MonoBehaviour
    {
        [SerializeField] protected GameEvent gameEvent;
        [SerializeField] protected int itemsNeeded;
        [SerializeField] protected bool willReduceItems;
        [SerializeField] protected UnityEvent onFail;

        protected bool _wasSuccesful;

        public void Raise()
        {
            if (_wasSuccesful || BasicItemCollection.ItemCount >= itemsNeeded)
            {
                gameEvent.RaiseEvent();

                if (willReduceItems && !_wasSuccesful)
                    BasicItemCollection.RemoveItem(itemsNeeded);
                    
                _wasSuccesful = true;
            }
            else
            {
                _wasSuccesful = false;
                onFail?.Invoke();
            }
        }
    }
}
