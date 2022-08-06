using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        [SerializeField] private Player Player;

        private void OnTriggerEnter(Collider other)
        {
            Player.EnterTrigger(other);
        }

        private void OnTriggerExit(Collider other)
        {
            Player.ExitTrigger(other);            
        }
    }
}
