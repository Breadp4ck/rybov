using System.Collections;
using Units.Movement.Fish;
using UnityEngine;

namespace Units.Spawning
{
    public class FishSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _radius;

        [SerializeField] private float _moveToSpawnPointSpeed;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_spawnPoint.position, _radius);
        }

        public void Spawn(GameObject prefab)
        {
            // LSP Sorry!
            if (prefab.TryGetComponent(out StealableFish fish) == false)
            {
                Debug.LogError("Prefab does not have StealableFish component.");
                return;
            }
            
            Debug.Log($"Spawned: {fish.name}");
            StealableFish spawnedFish = Instantiate(fish, transform.position, Quaternion.identity);

            StartCoroutine(LerpFishToSpawnPoint(spawnedFish, GetRandomSpawnPoint()));
        }
        
        private Vector2 GetRandomSpawnPoint()
        {
            Vector2 randomPoint = Random.insideUnitCircle * _radius;
            randomPoint += (Vector2) _spawnPoint.position;
            return randomPoint;
        }

        private IEnumerator LerpFishToSpawnPoint(StealableFish fish, Vector2 spawnPoint)
        {
            float distance = Vector2.Distance(fish.transform.position, spawnPoint);
            float lerpTime = distance / _moveToSpawnPointSpeed;
            var elapsedTime = 0f;
            
            fish.StateMachine.TryChangeState<CarriedState>();
            
            while (Vector2.Distance(fish.transform.position, spawnPoint) > 0.1f)
            {
                elapsedTime += Time.deltaTime;
                fish.transform.position = Vector2.Lerp(transform.position, _spawnPoint.position, elapsedTime / lerpTime);
                yield return null;
            }
            
            fish.StateMachine.TryChangeState<FidgetingCooldownState>();
        }
    }
}