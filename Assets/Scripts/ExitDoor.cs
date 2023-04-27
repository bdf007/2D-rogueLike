using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the exit door has collided with the player,
        if (collision.CompareTag("Player"))
        {
            // if the player has a key,
            if (collision.GetComponent<Player>().hasKey)
            {
                // load the next scene
                GameManager.instance.GoToNextLevel();
            }
        }
    }
}
