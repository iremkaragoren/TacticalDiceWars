using System.Collections;
using System.Collections.Generic;
using EventManager;
using ScriptableObjects;
using UnityEngine;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerData_SO m_playerData;
        [SerializeField] private LevelList_SO m_levelListSO;
        private int enemyCounter;
        
        private HashSet<Enums.EnemyType> listenableEnemyTypes = new HashSet<Enums.EnemyType>
        {
            Enums.EnemyType.EnemyArcher,
            Enums.EnemyType.Minion,
            Enums.EnemyType.Giant
        };

        private void OnEnable()
        {
            ExternalEvents.LevelSucces += OnLevelSucces;
            InternalEvents.EnemyDeath += OnEnemyDeath;
            ExternalEvents.RetryButtonClicked += OnAllWariorsDeath;
        }

        private void OnAllWariorsDeath()
        {
            m_playerData.ResetStackCount();
        }

        private void OnEnemyDeath(Enums.EnemyType type)
        {
            
            if (listenableEnemyTypes.Contains(type))
            {
                enemyCounter++;
                Debug.Log(enemyCounter +"enemy");

                if (enemyCounter >= m_levelListSO.GetCurrentLevelDataSO().EnemyCount)
                {
                    StartCoroutine(DeathDuration());
                }
            }
        }

        IEnumerator DeathDuration()
        {
            yield return new WaitForSeconds(1f);
            InternalEvents.AllEnemyDeath?.Invoke();
        }

        private void OnLevelSucces()
        {
            m_playerData.ResetStackCount();
        }

        private void OnDisable()
        {
            ExternalEvents.LevelSucces -= OnLevelSucces;
            InternalEvents.EnemyDeath -= OnEnemyDeath;
            ExternalEvents.RetryButtonClicked -= OnAllWariorsDeath;
        }
    }
}