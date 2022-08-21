using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tortoise.HOPPER
{
    public class MovingPlatform : MonoBehaviour
    {        
        [SerializeField] private List<Transform> movePoints;
        [SerializeField] private bool activeOnStart;
        [SerializeField] private float moveDuration;
        [SerializeField] private Ease ease;
        [SerializeField] private float waitTime;

        private Coroutine _moveCoroutine;
        private int _currentIndex;
        private bool _isActive;
        private bool _isMoving;

        private void Start()
        {
            _isActive = activeOnStart;

            foreach (Transform point in movePoints)
            {
                point.parent = null;
            }

            ChangePoint();
        }

        private IEnumerator MoveCoroutine()
        {
            yield return new WaitForSeconds(waitTime);

            _isMoving = true;
            transform.DOMove(movePoints[_currentIndex].position, moveDuration).SetUpdate(UpdateType.Fixed).SetEase(ease).OnComplete(() => ChangePoint());
        }

        private void ChangePoint()
        {
            _isMoving = false;
            _currentIndex++;

            if (_currentIndex >= movePoints.Count)
                _currentIndex = 0;

            if (_isActive)
                Move();   
        }

        public void Move()
        {
            if (_isMoving)
                return;
                
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        public void KeepActive(bool value)
        {
            _isActive = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                player.transform.parent = transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                player.transform.parent = null;
            }
        }
    }
}
