using System.Collections.Generic;
using EventManager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    // public class EnemyHolder : MonoBehaviour
    // {
    //     public List<Transform> m_enemyHolderList;
    //
    //     private void OnEnable()
    //     {
    //         InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
    //     }
    //     private void OnDisable()
    //     {
    //         InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
    //     }
    //
    //     private void OnAllEnemyDeath()
    //     {
    //         foreach (Transform enemyTransform in m_enemyHolderList)
    //         {
    //             Destroy(enemyTransform.gameObject);
    //         }
    //     }
    //
    //
    //     [Button]
    //     public void GetAllChildren()
    //     {
    //         for (int i = 0; i <transform.childCount; i++)
    //         {
    //             m_enemyHolderList.Add(transform.GetChild(i));
    //         }
    //     }
    // }
}
