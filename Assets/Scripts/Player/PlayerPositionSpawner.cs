using EventManager;
using Platform;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerPositionSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject playerGO;


        private void Awake()
        {
           
            ExternalEvents.LevelGO += OnLevelLoaded;
        }

        private void OnDisable()
        {
            ExternalEvents.LevelGO -= OnLevelLoaded;
        }

        private void OnLevelLoaded(GameObject level)
        {
            
            Transform spawnPoint = level.GetComponent<LevelReferenceHolder>().PlayerSpawnPoint;
            playerGO.transform.position = spawnPoint.position;
            playerGO.transform.rotation=Quaternion.identity;
        }
    }
}
