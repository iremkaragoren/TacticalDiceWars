using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DiceSelected;
using EventManager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
namespace General
{
    public class StackHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip diceTriggeredClip;
        [SerializeField] private PlayerData_SO playerDataSO;
        [SerializeField] private int maxHorizontalStack = 3;
        [SerializeField] private float horizontalSpacing;
        [SerializeField] private float verticalSpacing;
        [SerializeField] private float moveDuration;
        [SerializeField] private float scaleDuration;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private float torqueForce = 5f;
        [SerializeField] private float throwForce = 5f;

        private List<Transform> diceList = new();
        private bool playerStop;

        private void OnEnable()
        {
            ExternalEvents.WarriorsButtonAction += OnWarriorButtonTriggered;
            InternalEvents.ObjectTriggeredAction += OnDiceTriggered;
            ExternalEvents.RetryButtonClicked += OnRetry;
            InternalEvents.BattleArmyArea += OnBattleChoiserTriggered;
            InternalEvents.DiceThrowAreaSize += OnDiceThrowArea;
            InternalEvents.ProgressbarFiiled += OnDestroyAllChild;
            InternalEvents.BossFightTriggered += OnBossFightTriggered;
        }

        private void OnDisable()
        {
            ExternalEvents.WarriorsButtonAction -= OnWarriorButtonTriggered;
            InternalEvents.ObjectTriggeredAction -= OnDiceTriggered;
            ExternalEvents.RetryButtonClicked -= OnRetry;
            InternalEvents.BattleArmyArea -= OnBattleChoiserTriggered;
            InternalEvents.DiceThrowAreaSize -= OnDiceThrowArea;
            InternalEvents.ProgressbarFiiled -= OnDestroyAllChild;
            InternalEvents.BossFightTriggered -= OnBossFightTriggered;
        }

        private void OnBossFightTriggered()
        {
            playerStop = true;

            if (transform.childCount == 0)
            {
                StartCoroutine(RetryDuration());
            }
            else
            {
                ExternalEvents.BossFightHandle?.Invoke();
            }
        }

        private void OnDestroyAllChild()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator ThrowDiceCoroutine(Vector2 areaSize, Vector3 panelWorldPosition)
        {
            List<Tween> tweens = new List<Tween>();

            foreach (Transform dice in diceList)
            {
                Rigidbody rb = dice.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                Vector3 randomPosition = new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    0.3f,
                    Random.Range(-areaSize.y / 2, areaSize.y / 2)
                ) + panelWorldPosition;

                Tween moveTween = dice.DOMove(randomPosition, moveDuration).SetEase(Ease.OutQuad);
                Tween rotateTween = dice.DORotate(360 * Random.insideUnitSphere * torqueForce, moveDuration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

                tweens.Add(moveTween);
                tweens.Add(rotateTween);
            }

            foreach (var tween in tweens)
            {
                yield return tween.WaitForCompletion();
            }

            foreach (Transform dice in diceList)
            {
                Rigidbody rb = dice.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }

            playerDataSO.StackableCount -= diceList.Count;
            ExternalEvents.DiceDecreaser?.Invoke();

            StartCoroutine(DiceDuration());
        }

        IEnumerator DiceDuration()
        {
            yield return new WaitForSeconds(1.5f);
            InternalEvents.BossFightDiceOnGround?.Invoke(diceList);
        }

        private void OnDiceThrowArea(Vector2 areaSize, Vector3 panelWorldPosition)
        {
            StartCoroutine(ThrowDiceCoroutine(areaSize, panelWorldPosition));
        }

        private void OnBattleChoiserTriggered(bool triggered)
        {
            if (triggered)
            {
                playerStop = true;

                if (transform.childCount == 0)
                {
                    StartCoroutine(RetryDuration());
                }
            }
        }

        IEnumerator RetryDuration()
        {
            yield return new WaitForSeconds(3f);
            InternalEvents.RetryAction?.Invoke();
        }

        private void OnRetry()
        {
            foreach (Transform dice in diceList)
            {
                Destroy(dice.gameObject);
            }
            diceList.Clear();
            playerDataSO.StackableCount = 0;
        }

        private void OnWarriorButtonTriggered(Transform diceTransform, bool isMinus, Enums.WarriorType warriorType)
        {
            if (diceList.Count == 0) return;

            Transform lastDice = diceList[diceList.Count - 1];
            DiceTypeHolder diceTypeHolder = lastDice.GetComponent<DiceTypeHolder>();

            if (!isMinus)
            {
                lastDice.DOMove(diceTransform.position, moveDuration)
                    .OnComplete(() =>
                    {
                        playerDataSO.StackableCount--;

                        lastDice.transform.parent = diceTransform;

                        lastDice.transform.DOScale(targetScale, scaleDuration).SetLink(lastDice.gameObject);

                        ExternalEvents.IndexIncreaseAction?.Invoke();

                        diceTypeHolder.WarriorDiceType = warriorType;
                    });

                diceList.RemoveAt(diceList.Count - 1);
            }
        }

        private void OnDiceTriggered(Enums.ObjectType type, int amount, Transform diceTransform)
        {
            if (type == Enums.ObjectType.Dice)
            {
                PlayDiceTriggeredSound();
                diceTransform.GetComponent<DiceTypeHolder>().WarriorDiceType = Enums.WarriorType.None;
                diceTransform.parent = transform;
                playerDataSO.StackableCount++;
                AnimateDiceToPosition(diceTransform, playerDataSO.StackableCount);
                diceTransform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
                diceList.Add(diceTransform);
                ExternalEvents.DiceCollection?.Invoke();
            }

            if (type == Enums.ObjectType.CircularSaw || type == Enums.ObjectType.Saw)
            {
                ExternalEvents.DiceDecreaser?.Invoke();

                if (diceList.Count == 0) return;

                Transform lastDiceTransform = diceList[diceList.Count - 1];
                diceList.RemoveAt(diceList.Count - 1);
                Destroy(lastDiceTransform.gameObject);
                playerDataSO.StackableCount--;
            }
        }

        private void PlayDiceTriggeredSound()
        {
            if (audioSource != null && diceTriggeredClip != null)
            {
                audioSource.PlayOneShot(diceTriggeredClip);
            }
        }

        private void AnimateDiceToPosition(Transform dice, int totalDice)
        {
            int horizontalIndex = (totalDice - 1) % maxHorizontalStack;
            int verticalLayer = (totalDice - 1) / maxHorizontalStack;
            Vector3 newPosition = CalculatePosition(horizontalIndex, verticalLayer);
            dice.DOLocalMove(newPosition, moveDuration).SetEase(Ease.InOutQuad);
        }

        private Vector3 CalculatePosition(int horizontalIndex, int verticalLayer)
        {
            float xPosition = horizontalIndex * horizontalSpacing - horizontalSpacing * (maxHorizontalStack - 1) / 2;
            float yPosition = verticalLayer * verticalSpacing;
            return new Vector3(xPosition, yPosition, 0);
        }
    }
}
