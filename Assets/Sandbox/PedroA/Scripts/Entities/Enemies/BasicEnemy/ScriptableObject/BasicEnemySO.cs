using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [CreateAssetMenu(fileName = "New Basic Enemy", menuName = "Enemies/Basic Enemy")]
    public class BasicEnemySO : ScriptableObject
    {
        [field: Header("Agent Params")]
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField] public float StoppingDistance { get; private set; }

        [field: Header("Idle Params")]
        [field: SerializeField] public float IdleDuration { get; private set; }

        [field: Header("Wander Params")]
        [field: SerializeField] public float MinPointDistance { get; private set; }

        [field: Header("Follow Params")]
        [field: SerializeField] public float MinDistanceToFollow { get; private set; }
        [field: SerializeField] public float MaxDistanceFromTarget { get; private set; }
        [field: SerializeField] public float MaxDistanceFromArea { get; private set; }
        [field: SerializeField] public float MaxYDifference { get; private set; }

        [field: Header("Attack Params")]
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float AttackMoveSpeed { get; private set; }
        [field: SerializeField] public float AttackRotationSpeed { get; private set; }
        
        [field: Header("Damage Params")]
        [field: SerializeField] public float DamageCooldown { get; private set; }
        [field: SerializeField] public float KnockbackForce { get; private set; }
    }
}
