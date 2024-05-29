using System;
using EventManager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private Transform bossPrefab; 
        [SerializeField] private Transform spawnPoint;
        
        private void OnEnable()
        {
            InternalEvents.BossFightTriggered += OnBossInstantiate;
        }

        private void OnDisable()
        {
            InternalEvents.BossFightTriggered -= OnBossInstantiate;
        }

        private void OnBossInstantiate()
        {
            if (bossPrefab != null && spawnPoint != null)
            {
                Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
               
                InternalEvents.BossSpawned?.Invoke();
            }
        }
        
    }
}
