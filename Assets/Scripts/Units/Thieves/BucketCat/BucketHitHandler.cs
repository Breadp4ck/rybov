using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units
{
    [RequireComponent(typeof(BucketCat))]
    public class BucketHitHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bucketSprite;
        
        [SerializeField] private float _throwBucketAwaySpeed;
        
        private BucketCat _bucketCat;
        
        private Vector2 _bucketSpriteInitialPosition;

        private void Awake()
        {
            _bucketCat = GetComponent<BucketCat>();
            _bucketSpriteInitialPosition = _bucketSprite.transform.localPosition;
        }

        private void OnEnable()
        {
            _bucketCat.BucketHitEvent += OnBucketHit;
        }

        private void OnDisable()
        {
            _bucketCat.BucketHitEvent -= OnBucketHit;
        }

        private void OnBucketHit(BucketCat.BucketHitResult hitResult)
        {
            if (hitResult != BucketCat.BucketHitResult.NotBreached)
            {
                StopAllCoroutines();
            }
            
            switch (hitResult)
            {
                case BucketCat.BucketHitResult.Stagger:
                    StartCoroutine(ShakeBucket((float)_bucketCat.StaggerDuration.TotalSeconds));
                    break;
                case BucketCat.BucketHitResult.Stun:
                    StartCoroutine(ShakeBucket((float)_bucketCat.StunDuration.TotalSeconds));
                    break;
                case BucketCat.BucketHitResult.Breached:
                    StartCoroutine(ThrowBucket());
                    break;
            }
        }
        
        private IEnumerator ShakeBucket(float shakeBucketDurationSeconds)
        {
            var timePassedSeconds = 0f;
                
            while (timePassedSeconds < shakeBucketDurationSeconds)
            {
                _bucketSprite.transform.localPosition = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)) + _bucketSpriteInitialPosition;
                timePassedSeconds += Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator ThrowBucket()
        {
            var timePassedSeconds = 0f;
            Vector3 throwBucketAwayDirection = Random.insideUnitCircle.normalized;

            while (timePassedSeconds < 1)
            {
                _bucketSprite.transform.localPosition += throwBucketAwayDirection * _throwBucketAwaySpeed * Time.deltaTime;
                timePassedSeconds += Time.deltaTime;
                yield return null;
            }
        }
    }

}