using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class Damageable : MonoBehaviour
    {
        public bool IsInvincible 
        {
            get => _isInvincible;
            set => _isInvincible = value;
        }

        public int MaxHealth { get => maxHealth; }
        public int Health { get => _currentHealth; }

        [SerializeField] private IntEvent intEvent;
        [SerializeField] private int maxHealth;
        [SerializeField] private float knockbackMultiplier;
        [SerializeField] private float invincibleTime;

        [SerializeField] private UnityEvent onDeath;
        [SerializeField] private UnityEvent onDamage;
        [SerializeField] private UnityEvent onHealthIncrease;
        [SerializeField] private UnityEvent onInvincibleHit;
        [SerializeField] private UnityEvent onInvincibilityStart;
        [SerializeField] private UnityEvent onInvincibilityEnd;

        private int _currentHealth;
        private bool _isInvincible;

        private Coroutine _invincibilityCoroutine;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            TryGetComponent<Rigidbody>(out _rigidbody);
            
            _currentHealth = maxHealth;
            _isInvincible = false;
        }

        private void Start()
        {
            intEvent?.RaiseEvent(_currentHealth);            
        }

        public void IncreaseHealth(int value)
        {
            // if (_currentHealth <= 0)
            //     return;

            _currentHealth += value;

            onHealthIncrease?.Invoke();

            intEvent?.RaiseEvent(_currentHealth);
        }

        public void ApplyDamage(DamageData data)
        {
            if (gameObject == data.Damager)
                return;

            if (_currentHealth <= 0)
                return;
            
            if (_isInvincible)
            {
                onInvincibleHit?.Invoke();
                return;
            }

            if (invincibleTime > 0f)
                StartInvincibility();

            _currentHealth -= data.Amount;
            intEvent?.RaiseEvent(_currentHealth);
            
            ApplyKnockback(data);

            if (_currentHealth <= 0)
                onDeath?.Invoke();
            else
                onDamage?.Invoke();
        }

        public void StartInvincibility()
        {
            _invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());
        }

        private void ApplyKnockback(DamageData data)
        {
            if (_rigidbody == null)
                return;
            
            _rigidbody.isKinematic = false;

            var direction = data.Direction;
            direction.y = 0f;
            _rigidbody.AddForce(direction.normalized * data.KnockbackForce * knockbackMultiplier, ForceMode.VelocityChange);
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
