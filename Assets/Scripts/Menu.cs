using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_InputField seedInput;
   
    public void Start()
    {
        // load the seed value and display it in the main menu
        seedInput.text = PlayerPrefs.GetInt("seed").ToString();
    }

    public void OnUpdateSeed()
    {
        // save the seed value into PlayerPrefs
        PlayerPrefs.SetInt("seed", int.Parse(seedInput.text));
    }

    public void OnPlayButton()
    {
        // load the game scene
        SceneManager.LoadScene("Game");
    }


}
