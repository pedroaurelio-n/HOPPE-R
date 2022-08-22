using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private float invincibleTime;

        [SerializeField] private UnityEvent onDeath;
        [SerializeField] private UnityEvent onDamage;
        [SerializeField] private UnityEvent onInvincibleHit;
        [SerializeField] private UnityEvent onInvincibilityStart;
        [SerializeField] private UnityEvent onInvincibilityEnd;

        private int _currentHealth;
        private bool _isInvincible;

        private Coroutine _invincibilityCoroutine;

        private void Awake()
        {
            _currentHealth = maxHealth;
            _isInvincible = false;
        }

        public void ApplyDamage(int damage)
        {
            if (_currentHealth <= 0)
                return;
            
            if (_isInvincible)
            {
                onInvincibleHit?.Invoke();
                return;
            }

            StartInvincibility();

            _currentHealth -= damage;

            if (_currentHealth <= 0)
                onDeath?.Invoke();
            else
                onDamage?.Invoke();
        }

        public void StartInvincibility()
        {
            _invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());
        }

        private IEnumerator InvincibilityCoroutine()
        {
            var elapsedTime = 0f;

            onInvincibilityStart?.Invoke();
            _isInvincible = true;

            while (elapsedTime <= invincibleTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isInvincible = false;
            onInvincibilityEnd?.Invoke();
        }
    }
}
