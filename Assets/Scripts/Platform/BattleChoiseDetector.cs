using EventManager;
using UnityEngine;

namespace Platform
{
    public class BattleChoiseDetector : MonoBehaviour
    {
        private void OnEnable()
        {
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
        }
        
        private void OnAllEnemyDeath()
        {
             Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
        }
    }
}
