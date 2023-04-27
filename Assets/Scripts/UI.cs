using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{   
    public GameObject[] hearts;
    public TextMeshProUGUI coinText;
    public GameObject keyIcon;
    public TextMeshProUGUI levelText;
    public RawImage map;

    public static UI instance;

    public void Awake()
    {
        instance = this;
    }

    // called when we take damage or collect a health pickup
    public void UpdateHealth (int health)
    {
        // for each heart inside the hearts list
        for(int x = 0; x < hearts.Length; x++)
        {
            // activate to match the current HP
            hearts[x].SetActive(x < health);
        }
    }

    // called when we collect a coin
    public void UpdateCoinText (int coins)
    {
        // update the coin text
        coinText.text = coins.ToString();
    }

    // called when we collect a key
    public void ToogleKeyIcon (bool toggle)
    {
        // activate the key icon
        keyIcon.SetActive(toggle);
    }

    // update the level text
    public void UpdateLevelText (int level)
    {
        levelText.text = "Level " + level;
    }



}   
