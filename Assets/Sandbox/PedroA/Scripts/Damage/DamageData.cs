using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public struct DamageData
    {
        public int Amount;
        public GameObject Damager;
        public Vector3 WorldSource;
        public Vector3 Direction;
        public float KnockbackForce;

        public DamageData(int damageAmount, GameObject damager, Vector3 worldSource, Vector3 direction, float knockbackForce)
        {
            Amount = damageAmount;
            Damager = damager;
            WorldSource = worldSource;
            Direction = direction;
            KnockbackForce = knockbackForce;
        }
    }
}
