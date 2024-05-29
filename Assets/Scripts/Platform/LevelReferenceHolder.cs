using UnityEngine;

namespace Platform
{
    public class LevelReferenceHolder : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawnPoint;
        
        public Transform PlayerSpawnPoint => playerSpawnPoint;
    }
}
