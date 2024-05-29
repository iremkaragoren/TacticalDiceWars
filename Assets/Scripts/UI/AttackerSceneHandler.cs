using System;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AttackerSceneHandler : MonoBehaviour
    {
        // [SerializeField] private GameObject m_attackScene;
        // private int m_maxDiceCount;
        //
        // private void OnEnable()
        // {
        //     InternalEvents.ProgressbarFiiled += OnProgressbarFiiled;
        //     // InternalEvents.AttackerSprite += OnAttackSprite;
        // }
        //
        // private void OnDisable()
        // {
        //     InternalEvents.ProgressbarFiiled -= OnProgressbarFiiled;
        //     // InternalEvents.AttackerSprite -= OnAttackSprite;
        // }
        //
        //
        //
        // private void OnProgressbarFiiled()
        // {
        //     StartCoroutine(AttackSceneDuration());
        // }
        //
        // IEnumerator AttackSceneDuration()
        // {
        //     m_attackScene.SetActive(true);
        //     
        //     yield return new WaitForSeconds(2f);
        //     
        //     
        //     InternalEvents.AttackerSpriteClosed?.Invoke();
        //     m_attackScene.SetActive(false);
        //     
        //     
        // }
    }
}
