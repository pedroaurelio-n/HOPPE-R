using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Tortoise.HOPPER
{
    public class Shield : MonoBehaviour
    {
        public bool IsShieldActive { get; private set; }
        public bool IsShieldBroken { get; private set; }
        
        [Header("Burst Params")]
        [SerializeField] private float burstRadius;
        [SerializeField] private float burstDuration;
        [SerializeField] private Color burstColor;
        [SerializeField] private Ease burstInEase;
        [SerializeField] private Ease burstOutEase;

        [Header("Shield")]
        [SerializeField] private float normalRadius;
        [SerializeField] private Color normalColor;
        [SerializeField] private float disableDuration;
        [SerializeField] private Ease disableEase;

        private Vector3 _burstScale;
        private Vector3 _normalScale;

        private Coroutine _enableCoroutine;
        private Coroutine _disableCoroutine;
        private WaitForSeconds _waitForHalfBurst;
        private WaitForSeconds _waitForDisable;

        private Sequence _burstToNormalSequence;

        private bool _isInputActive;
        private bool _canDisableShield;
        private bool _isShieldOnBurst;

        private Renderer _renderer;
        private Collider _collider;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();

            transform.localScale = Vector3.zero;

            _renderer.enabled = false;
            _collider.enabled = false;

            _burstScale = new Vector3(burstRadius * 2f, burstRadius * 2f, burstRadius * 2f);
            _normalScale = new Vector3(normalRadius * 2f, normalRadius * 2f, normalRadius * 2f);

            _waitForHalfBurst = new WaitForSeconds(burstDuration * 0.5f);
            _waitForDisable = new WaitForSeconds(disableDuration);
        }

        private void Update()
        {
            if (!_isInputActive)
            {
                if (!_canDisableShield)
                    return;
                    
                DisableShield();
            }
        }

        private void EnableShield()
        {
            _isInputActive = true;

            if (IsShieldActive || IsShieldBroken)
                return;

            if (_disableCoroutine != null)
            {
                StopCoroutine(_disableCoroutine);
                _disableCoroutine = null;
            }

            _enableCoroutine = StartCoroutine(EnableCoroutine());
        }

        private IEnumerator EnableCoroutine()
        {
            _isShieldOnBurst = true;
            IsShieldActive = true;
            _canDisableShield = false;

            _renderer.enabled = true;
            _collider.enabled = true;

            _renderer.material.color = burstColor;

            transform.DOScale(_burstScale, burstDuration * 0.5f).SetEase(burstInEase);
            yield return _waitForHalfBurst;

            _isShieldOnBurst = false;
            _canDisableShield = true;

            _burstToNormalSequence = DOTween.Sequence();
            _burstToNormalSequence.Append(transform.DOScale(_normalScale, burstDuration * 0.5f).SetEase(burstOutEase));
            _burstToNormalSequence.Insert(0, _renderer.material.DOColor(normalColor, burstDuration).SetEase(burstOutEase));
            _burstToNormalSequence.Play();
            yield return _waitForHalfBurst;

            _enableCoroutine = null;
        }

        private void DisableShield()
        {
            if (_enableCoroutine != null)
            {
                StopCoroutine(_enableCoroutine);
                _enableCoroutine = null;
            }

            if (_burstToNormalSequence != default)
                _burstToNormalSequence.Kill();

            _disableCoroutine = StartCoroutine(DisableCoroutine());
        }

        private IEnumerator DisableCoroutine()
        {
            _canDisableShield = false;
            transform.DOScale(Vector3.zero, disableDuration).SetEase(disableEase);
            yield return _waitForDisable;

            IsShieldActive = false;

            _renderer.enabled = false;
            _collider.enabled = false;

            _disableCoroutine = null;

            transform.localScale = Vector3.zero;
        }

        private void OnEnable()
        {
            PlayerInputHandler.onShieldPerformed += EnableShield;
            PlayerInputHandler.onShieldCanceled += () => _isInputActive = false;
        }

        private void OnDisable()
        {
            PlayerInputHandler.onShieldPerformed -= EnableShield;
            PlayerInputHandler.onShieldCanceled -= () => _isInputActive = false;;
        }

        // UnityEvent Reference
        public void BreakShield(bool value)
        {
            IsShieldBroken = value;

            if (value)
                DisableShield();
        }
    }
}