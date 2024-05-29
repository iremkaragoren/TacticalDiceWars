using System;
using EventManager;
using UnityEngine;
using ScriptableObjects;
using UnityEngine.SceneManagement;

namespace Prefabs.Managers
{
    public class LevelHandler : MonoBehaviour
    {
        [SerializeField] private PlayerData_SO m_playerDataSO;
        [SerializeField] private LevelList_SO m_levelList_SO;
        [SerializeField] private Transform levelHolder;
        private GameObject m_levelGO;
        private int m_currentLevel = 0;

        private void Start()
        {
            
            LoadLevel();
        }

        private void OnEnable()
        {
            ExternalEvents.NextLevelButtonClicked += OnNextLevelButtonClicked;
            ExternalEvents.RetryButtonClicked += OnRetryClicked;
        }

        private void OnDisable()
        {
           
            ExternalEvents.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            ExternalEvents.RetryButtonClicked -= OnRetryClicked;
        }

        private void OnRetryClicked()
        {
             RetryLevel();
        }

        private void OnNextLevelButtonClicked()
        {
            m_playerDataSO.LevelIndex++;
            LoadLevel();
        }

     

        private void LoadLevel()
        {
           
            if (m_levelGO) Destroy(m_levelGO); 

            m_playerDataSO.ResetStackCount();
            
            LevelData_SO levelData = m_levelList_SO.GetCurrentLevelDataSO();
            
            m_levelGO = Instantiate(levelData.LevelPrefab, Vector3.zero, Quaternion.identity);
           
            m_levelGO.transform.SetParent(levelHolder, false); 
            
            if(ExternalEvents.LevelGO != null)
            {
               
                ExternalEvents.LevelGO.Invoke(m_levelGO);
            }
            else
            {
                Debug.Log("LevelGO no subscribers");
            }
        }
        

        private void RetryLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}