using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData_SO", menuName = "ThisGame/Variable/EnemyData", order = 0)]
    public class EnemyData_SO : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private GameObject enemyPrefab;

        public GameObject EnemyPrefab => enemyPrefab;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }
    }

}
