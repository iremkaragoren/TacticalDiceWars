using EventManager;
using UnityEngine;

namespace General
{
    public class DestroyOnBossFight : MonoBehaviour
    {
        private void OnEnable()
        {
            InternalEvents.BossFightTriggered += OnDestroyOnBossFight;
        }

        private void OnDisable()
        {
            InternalEvents.BossFightTriggered -= OnDestroyOnBossFight;
        }

        private void OnDestroyOnBossFight()
        {
            Destroy(this.gameObject);
        }
    }
}
