using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [Serializable]
    public class BasicEnemyAnimationData
    {
        public int ActiveParamHash { get; private set; }
        public int MovingParamHash { get; private set; }
        public int AttackingParamHash { get; private set; }
        public int DamageParamHash { get; private set; }

        [Header("Animation Parameters")]
        [field: SerializeField] private string ActiveParam = "IsActive";
        [field: SerializeField] private string MovingParam = "IsMoving";
        [field: SerializeField] private string AttackingParam = "IsAttacking";
        [field: SerializeField] private string DamageParam = "WasDamaged";

        public void Initialize()
        {
            ActiveParamHash = Animator.StringToHash(ActiveParam);
            MovingParamHash = Animator.StringToHash(MovingParam);
            AttackingParamHash = Animator.StringToHash(AttackingParam);
            DamageParamHash = Animator.StringToHash(DamageParam);
        }
    }
}
