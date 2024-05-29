using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Enums
{
   
   public enum ButtonType
   {
      Minus,
      Plus
   }

   public enum ObjectType
   {
      CircularSaw,
      Saw,
      Dice
   }

   public enum WarriorType
   {
      Swordsmen,
      Archer,
      Shield,
      None
   }

   public enum EnemyType
   {
      Minion,
      EnemyArcher,
      Giant
   }
}
