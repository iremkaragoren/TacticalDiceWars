using System;
using EventManager;
using Platform;
using UnityEngine;

namespace Player
{
    public class PlayerDetector : MonoBehaviour
    {
        private Rigidbody playerRb;
        private void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
        }

        private void OnAllEnemyDeath()
        {
            playerRb.isKinematic = false;
        }

        private void OnDisable()
        {
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out Interactable interactable))
            {
                interactable.Interact();
            }
            
            if (other.TryGetComponent(out BattleChoiseDetector _))
            {
                Vector3 newPosition = new Vector3(0, transform.position.y, transform.position.z);
                transform.position = newPosition;
                playerRb.isKinematic = true;
                InternalEvents.BattleArmyArea?.Invoke(true);
            }

            if (other.TryGetComponent(out BossFightDetector _))
            {
                Vector3 newPosition = new Vector3(0, transform.position.y, transform.position.z);
                transform.position = newPosition;
                playerRb.isKinematic = true;
                InternalEvents.BossFightTriggered?.Invoke();
            }
        }
    }
}
