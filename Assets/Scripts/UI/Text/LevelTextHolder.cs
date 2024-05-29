using EventManager;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextHolder : MonoBehaviour
{
    [SerializeField] private Text levelText;
    private const string LevelKey = "CurrentLevel";

    private void Start()
    {
        LoadLevel();
        UpdateLevelText();
    }

    private void OnEnable()
    {
        InternalEvents.BossDeath += OnBossDeath;
    }

    private void OnDisable()
    {
        InternalEvents.BossDeath -= OnBossDeath;
    }

    private void OnBossDeath()
    {
        IncrementLevel();
        UpdateLevelText();
    }

    private void LoadLevel()
    {
        if (!PlayerPrefs.HasKey(LevelKey))
        {
            PlayerPrefs.SetInt(LevelKey, 1);
            PlayerPrefs.Save();
        }
    }

    private void IncrementLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(LevelKey, 1);
        currentLevel++;

        if (currentLevel > 5)
        {
            currentLevel = currentLevel % 5;
        }

        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    private void UpdateLevelText()
    {
        int currentLevel = PlayerPrefs.GetInt(LevelKey, 1);
        if (levelText != null)
        {
            levelText.text = currentLevel.ToString();
        }
    }
}

