
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData_SO", menuName = "ThisGame/Variable/PlayerData", order = 1)]
    public class PlayerData_SO : ScriptableObject
    {
        [SerializeField] private Vector3 playerPosition;
        [SerializeField] private int stackableCount;
        [SerializeField] private int levelIndex;

        public int StackableCount
        {
            get => stackableCount;
            set => stackableCount = value;
        }

        public int LevelIndex
        {
            get => levelIndex;
            set => levelIndex = value;
        }

        public Vector3 PlayerPosition
        {
            get => playerPosition;
            set => playerPosition = value;
        }

        public void ResetStackCount()
        {
            stackableCount = 0;
            playerPosition = Vector3.zero;
        }
    }

}


    


