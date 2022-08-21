using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            BasicItemCollection.AddItem();
            gameObject.SetActive(false);
        }
    }
}
