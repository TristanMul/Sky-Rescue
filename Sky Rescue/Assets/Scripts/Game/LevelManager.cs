using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private RectTransform levelCompletePanel;
    [SerializeField] private Text amountOfCoins;
    [SerializeField] private Text completeLevelText;
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        amountOfCoins.text = PlayerPrefs.GetInt("Coin").ToString();
    }
    public void AddCoins(int coinsAdded)
    {
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + coinsAdded);
        amountOfCoins.text = PlayerPrefs.GetInt("Coin").ToString();
    }
    public void UpdateLevelCompleteText()
    {
        levelCompletePanel.gameObject.SetActive(true);
        completeLevelText.text = "LEVEL " + PlayerPrefs.GetInt("leveltext").ToString() + " COMPLETE";
    }

    public void LoadNextScene()
    {
        Debug.Log("Going to next level");
        // Increase level.
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        PlayerPrefs.SetInt("leveltext", PlayerPrefs.GetInt("leveltext") + 1);

        // Loop the levels.
        if (PlayerPrefs.GetInt("level") > Loader.totalLevels)
            PlayerPrefs.SetInt("level", 1);

        // Go to next level.
        SceneManager.LoadScene(PlayerPrefs.GetInt("level").ToString());
    }
    public void ReloadCurrentScene()
    {
        Debug.Log("Retrying level");
        SceneManager.LoadScene(PlayerPrefs.GetInt("level").ToString());
    }
}
