using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ThisGame/Predefined/LevelData", order=0)]
public class LevelData_SO : UnityEngine.ScriptableObject
{
   [SerializeField] private GameObject levelPrefab;
   [SerializeField] private int enemyCount;

   public GameObject LevelPrefab => levelPrefab;
   public int EnemyCount
   {
      get => enemyCount;
      set => enemyCount = value;
   }
}
