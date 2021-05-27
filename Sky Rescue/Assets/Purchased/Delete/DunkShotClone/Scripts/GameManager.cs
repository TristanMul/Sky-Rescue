using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] gameItems;

    public GameObject menuPanel;
    public GameObject gameOverPanel;

    public static bool isStart = false;

    public void Play()
    {
        menuPanel.SetActive(false);
        foreach (var item in gameItems)
        {
            item.SetActive(true);
        }
    }
    
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        ScoreManager.Instance.UpdateBestScoreUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Demo");
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
