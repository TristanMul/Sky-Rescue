using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpriteChanger : MonoBehaviour {

    public Store CurrentStore;
    public SpriteRenderer CurrentSprite;

    private void OnEnable()
    {
        CurrentSprite.sprite = Resources.Load<Sprite>("Balls/" + PlayerPrefs.GetInt("BallId"));
    }
}
