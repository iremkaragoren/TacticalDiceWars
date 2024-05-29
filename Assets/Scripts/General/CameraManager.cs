using Cinemachine;
using EventManager;
using UnityEngine;


namespace General
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera starterCamera;
        [SerializeField] private CinemachineVirtualCamera inGameCamera;
        [SerializeField] private CinemachineVirtualCamera warriorChoosingCamera;
        [SerializeField] private CinemachineVirtualCamera bossFightCamera;
        [SerializeField] private CinemachineVirtualCamera bossDiceCounterCamera;

        private void OnEnable()
        {
            InternalEvents.BattleArmyArea += OnBattleChooserTriggered;
            ExternalEvents.WarriorsSpawned += OnReadyButtonClicked;
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
            ExternalEvents.RetryButtonClicked += OnRetryClicked;
            InternalEvents.BossFightTriggered += OnBossFightTriggered;
            InternalEvents.ProgressbarFiiled += OnProgressbarFiiled;
        }

        private void OnProgressbarFiiled()
        {
             ActivateCamera(bossFightCamera);
        }

        private void OnBossFightTriggered()
        {
           ActivateCamera(bossDiceCounterCamera);
        }

        private void OnRetryClicked()
        {
            ActivateCamera(starterCamera);
        }

        private void OnAllEnemyDeath()
        {
            ActivateCamera(starterCamera);
        }

        private void OnDisable()
        {
            InternalEvents.BattleArmyArea -= OnBattleChooserTriggered;
            ExternalEvents.WarriorsSpawned -= OnReadyButtonClicked;
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
            ExternalEvents.RetryButtonClicked -= OnRetryClicked;
            InternalEvents.BossFightTriggered -= OnBossFightTriggered;
            InternalEvents.ProgressbarFiiled -= OnProgressbarFiiled;
        }

        private void Start()
        {
            ActivateCamera(starterCamera);
        }

        private void OnBattleChooserTriggered(bool isActive)
        {
            if (isActive)
            {
                ActivateCamera(warriorChoosingCamera);
            }
        }

        private void OnReadyButtonClicked()
        {
            ActivateCamera(inGameCamera);
        }

        private void ActivateCamera(CinemachineVirtualCamera cameraToActivate)
        {
            starterCamera.gameObject.SetActive(false);
            inGameCamera.gameObject.SetActive(false);
            warriorChoosingCamera.gameObject.SetActive(false);
            bossDiceCounterCamera.gameObject.SetActive(false);
            bossFightCamera.gameObject.SetActive(false);
        
            cameraToActivate.gameObject.SetActive(true);
        }
    }
}