using UnityEngine;
using UnityEngine.Events;

namespace EventManager
{
    public static class ExternalEvents
    {
        public static UnityAction LevelStart;
        public static UnityAction LevelSucces;
        public static UnityAction NextLevelButtonClicked;
        public static UnityAction WarriorsSpawned;
        public static UnityAction RunButtonClicked;
        public static UnityAction RetrySceneActivated;
        public static UnityAction<Transform,bool,Enums.WarriorType> WarriorsButtonAction;
        public static UnityAction RetryButtonClicked;
        public static UnityAction DiceCollection;
        public static UnityAction DiceDecreaser;
        public static UnityAction IndexIncreaseAction;
        public static UnityAction FightTextAssigned;
        public static UnityAction DiceTotalCountDone;
        public static UnityAction<GameObject> LevelGO;
        public static UnityAction BossFightHandle;


    }
}
