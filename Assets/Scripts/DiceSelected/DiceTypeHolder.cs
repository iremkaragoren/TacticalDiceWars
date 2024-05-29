using System;
using UnityEngine;

namespace DiceSelected
{
    public class DiceTypeHolder : MonoBehaviour
    {
        private Rigidbody diceRb;

        private void Awake()
        {
            diceRb = GetComponent<Rigidbody>();
            WarriorDiceType = Enums.WarriorType.None;
        }

        public Enums.WarriorType WarriorDiceType { get; set; }
    }
}
