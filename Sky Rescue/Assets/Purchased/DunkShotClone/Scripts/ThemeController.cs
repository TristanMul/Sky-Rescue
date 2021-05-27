using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeController : MonoBehaviour {

    public GameObject[] ThemeCheck;

    public SpriteRenderer BgRenderer;
    public SpriteRenderer WallRenderer;

    private void OnEnable()
    {
        CheckTheme();
        UpdateTheme();
    }

    private void CheckTheme() {
        for (int i = 0; i < ThemeCheck.Length; i++)
        {
            ThemeCheck[i].SetActive(false);
        }
        ThemeCheck[PlayerPrefs.GetInt("ThemeId")].SetActive(true);
    }
	
    public void SelectTheme(int id)
    {
        PlayerPrefs.SetInt("ThemeId", id);
        CheckTheme();
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        int themeId = PlayerPrefs.GetInt("ThemeId");
        BgRenderer.sprite = Resources.Load<Sprite>("Themes/" + themeId  + "/" + "bg_" + themeId);
        WallRenderer.sprite = Resources.Load<Sprite>("Themes/" + themeId + "/" + "wall_" + themeId);
    }
}
