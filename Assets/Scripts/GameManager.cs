using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{  
    public int level;
    public int baseSeed;

    private int prevRoomPlayerHealth;
    private int prevRoomPlayerCoins;

    // reference to the player class
    private Player player;

    // singleton
    public static GameManager instance;

    void Awake ()
    {
        // if there is already an (original) instance of this script existing, which is not 'this' instance
        if(instance != null && instance != this)
        {
            // destroy this gameObject(clone)
            Destroy(gameObject);
            return;
        }

        // if this is not a clone, don't destroy on load.
        instance= this;
        DontDestroyOnLoad(gameObject);
    }

    // this can replace the Start function of Generation.cs
    void Start ()
    {
        level = 1;
        baseSeed = PlayerPrefs.GetInt("seed");
        Random.InitState(baseSeed);
        Generation.instance.Generate();  
        UI.instance.UpdateLevelText(level);

        player = FindObjectOfType<Player>();

        // subscribe to the sceneLoaded event which gets called whenever a scene has been loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void GoToNextLevel ()
    {
        // save the player's health and coins before loading the next level
        prevRoomPlayerHealth = player.curHp;
        prevRoomPlayerCoins = player.coins;

        // load the next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded (Scene scene, LoadSceneMode mode)
    {
        // if this is the main menu scene, destroy this gameObject to prevent a duplicate
        if(scene.name != "Game")
        {   
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
            return;
        }

        player = FindObjectOfType<Player>();
        level++;
        // get a random int for the seed
        baseSeed = Random.Range(0, 9999999);
        // baseSeed++;

        // generate a new level with the updated baseSeed
        Generation.instance.Generate();

        // transfer the previous game data to the new level
        player.curHp = prevRoomPlayerHealth;
        player.coins = prevRoomPlayerCoins;

        // update the UI
        UI.instance.UpdateHealth(prevRoomPlayerHealth);
        UI.instance.UpdateCoinText(prevRoomPlayerCoins);
        UI.instance.UpdateLevelText(level);
    }

    

}
