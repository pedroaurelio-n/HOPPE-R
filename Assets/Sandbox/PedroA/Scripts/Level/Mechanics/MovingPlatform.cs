using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tortoise.HOPPER
{
    public class MovingPlatform : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        
        [SerializeField] private List<Transform> movePoints;
        [SerializeField] private float moveDuration;
        [SerializeField] private Ease ease;
        [SerializeField] private float waitTime;

        private Coroutine _moveCoroutine;
        private int _currentIndex;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            foreach (Transform point in movePoints)
            {
                point.parent = null;
            }

            ChangePoint();
        }

        private IEnumerator MoveCoroutine()
        {
            yield return new WaitForSeconds(waitTime);

            transform.DOMove(movePoints[_currentIndex].position, moveDuration).SetUpdate(UpdateType.Fixed).SetEase(ease).OnComplete(() => ChangePoint());

            // var moveDirection = movePoints[_currentIndex].position - transform.position;
            // moveDirection.Normalize();

            // Rigidbody.velocity = moveDirection * moveVelocity;
        }

        private void ChangePoint()
        {
            _currentIndex++;

            if (_currentIndex >= movePoints.Count)
                _currentIndex = 0;
            
            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                player.transform.parent = transform;
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player player))
            {
                player.transform.parent = null;
            }
        }
    }
}
