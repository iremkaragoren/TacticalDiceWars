using System;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DiceSelected
{
    public class RandomDiceRoll : MonoBehaviour
    {
        [SerializeField] private float torqueMin;
        [SerializeField] private float torqueMax;
        [SerializeField] private float throwStrenght;
        [SerializeField] private DiceTypeHolder diceTypeHolder;
        [SerializeField] private DetectUpperSide detectUpperSide;

        private Rigidbody diceRb;
        private int collisionCount = 0;

        private void Awake()
        {
            diceRb = GetComponent<Rigidbody>();
        }

        private void RandomRoll()
        {
            diceRb.isKinematic = false;
            RotateRandom();
        }

        private void OnEnable()
        {
            ExternalEvents.RunButtonClicked += OnRunButtonClicked;
        }

        private void OnDisable()
        {
            ExternalEvents.RunButtonClicked -= OnRunButtonClicked;
        }

        private void OnRunButtonClicked()
        {
            if (diceTypeHolder.WarriorDiceType != Enums.WarriorType.None)
            {
                RandomRoll();
            }
        }

        private void RotateRandom()
        {
            if (diceTypeHolder.WarriorDiceType == Enums.WarriorType.None)
                return;

            diceRb.AddForce(Vector3.up * throwStrenght, ForceMode.Impulse);

            diceRb.AddTorque(transform.forward * Random.Range(torqueMin, torqueMax) +
                             transform.up * Random.Range(torqueMin, torqueMax) +
                             transform.right * Random.Range(torqueMin, torqueMax));

            StartCoroutine(WaitForStop());
        }

        private IEnumerator WaitForStop()
        {
            yield return new WaitForFixedUpdate();

            while (diceRb.angularVelocity.sqrMagnitude > 0.1)
            {
                yield return new WaitForFixedUpdate();
            }

            detectUpperSide.UpdateUpperNumber();
            InternalEvents.DiceRollEnd?.Invoke(diceTypeHolder.WarriorDiceType, detectUpperSide.UpperSideNumber);
        }
    }

}