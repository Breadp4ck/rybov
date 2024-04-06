using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Fishing.Handlers
{
    public class FishMinigame : MonoBehaviour
    {
        [SerializeField] public Transform FishTransform;
        
        public Vector3 FishDirectionPull;
        
        private float _maxDeviation = 0.2f;
        private float _deviationFrequency = 10f;

        private float _nextDeviationTime = 0f;
        private Vector3 _deviationVector = Vector3.zero;

        public void ChangeFishDirectionPull()
        {
            FishDirectionPull = Random.insideUnitCircle / 3f;
            FishDirectionPull.z = 0f;
            print(FishDirectionPull);
        }

        public void Update()
        {
            if (Time.time >= _nextDeviationTime)
            {
                _deviationVector = Random.insideUnitSphere * _maxDeviation;
        
                _nextDeviationTime = Time.time + 1f / _deviationFrequency;
            }
            
            transform.position += (FishDirectionPull + _deviationVector) * (0.5f * Time.deltaTime);
            //transform.Translate(FishDirectionPull * (0.01f * Time.deltaTime));
        }
    }
}