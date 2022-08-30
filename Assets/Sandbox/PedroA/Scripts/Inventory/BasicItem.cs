using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Player>(out Player player))
                return;

            BasicItemCollection.AddItem();
            gameObject.SetActive(false);
        }
    }
}
