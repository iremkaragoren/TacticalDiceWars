using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "ThisGame/Predefined/LevelList", order=1)]
    public class LevelList_SO:ScriptableObject
    {
        [SerializeField] private List<LevelData_SO> levelData;
        [SerializeField] private int currentLevelEnemyCount;
        [SerializeField] private PlayerData_SO playerData;

        public LevelData_SO GetLevelWithIndex(int currentLevel)
        {
            if (levelData.Count <= currentLevel)
                return levelData[currentLevel % levelData.Count];

            return levelData[currentLevel];
        }


        public LevelData_SO GetCurrentLevelDataSO()
        {
            var currentLevel = playerData.LevelIndex;
            if (levelData.Count <= currentLevel)
                return levelData[currentLevel % levelData.Count];

            return levelData[currentLevel];
        }
 
    }
}
