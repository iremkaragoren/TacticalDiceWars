using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventManager
{
    public class InternalEvents : MonoBehaviour
    {
        public static UnityAction<Enums.ObjectType, int,Transform> ObjectTriggeredAction;
        public static UnityAction<bool> BattleArmyArea;
        public static UnityAction<Enums.WarriorType, int> DiceRollEnd;
        public static UnityAction WarriorDeath;
        public static UnityAction RetryAction;
        public static UnityAction<Enums.EnemyType> EnemyDeath;
        public static UnityAction AllEnemyDeath;
        public static UnityAction BossFightTriggered;
        public static UnityAction BossDeath;
        public static UnityAction BossSpawned;
        public static UnityAction<Vector2,Vector3> DiceThrowAreaSize;
        public static UnityAction<List<Transform>> BossFightDiceOnGround;
        public static UnityAction<GameObject, int> AttackerSpawned;
        public static UnityAction BulletSpawned;
        public static UnityAction<int,int> AttackerDiceCount;
        public static UnityAction ProgressbarFiiled;
        public static UnityAction<int> AttackerSprite;
        

    }
}
