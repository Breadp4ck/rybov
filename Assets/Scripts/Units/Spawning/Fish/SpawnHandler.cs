using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fishing.Fish
{
    public class SpawnHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _fishPrefab;
        [SerializeField] private FishSpawner _fishSpawner;

        public void SpawnFish()
        {
            _fishSpawner.Spawn(_fishPrefab);
        }
    }
}