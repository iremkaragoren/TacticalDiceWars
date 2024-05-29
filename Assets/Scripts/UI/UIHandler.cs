using System;
using EventManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject startScene;
        [SerializeField] private GameObject inGame;
        [SerializeField] private GameObject successScene;
        [SerializeField] private GameObject retryScene;

        private void OnEnable()
        {
            InternalEvents.BossDeath += OnBossDeath;
            InternalEvents.RetryAction += OnAllWarriorsDeath;
            ExternalEvents.RetryButtonClicked += OnRetryButtonClicked;
            ExternalEvents.NextLevelButtonClicked += OnNextLevelClicked;
        }
        private void OnDisable()
        {
            InternalEvents.BossDeath -= OnBossDeath;
            InternalEvents.RetryAction -= OnAllWarriorsDeath;
            ExternalEvents.RetryButtonClicked -= OnRetryButtonClicked;
            ExternalEvents.NextLevelButtonClicked -= OnNextLevelClicked;
        }

        private void OnNextLevelClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnRetryButtonClicked()
        {
            retryScene.SetActive(false);
            startScene.SetActive(true);
        }

        private void OnAllWarriorsDeath()
        {
            retryScene.SetActive(true);
            ExternalEvents.RetrySceneActivated?.Invoke();
        }

        private void OnBossDeath()
        {
            successScene.SetActive(true);
        }

        public void LevelStart()
        {
            ExternalEvents.LevelStart?.Invoke();
            startScene.SetActive(false);
            inGame.SetActive(true);
        }
        
    }
}
