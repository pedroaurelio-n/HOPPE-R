using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [CreateAssetMenu(fileName = "New Basic Enemy", menuName = "Enemies/Basic Enemy")]
    public class BasicEnemySO : ScriptableObject
    {
        [field: SerializeField] public float IdleDuration { get; private set; }
    }
}
