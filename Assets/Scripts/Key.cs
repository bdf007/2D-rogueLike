using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the key object has collided with the player,
        if (collision.CompareTag("Player"))
        {
            // add a key to the player's inventory
            collision.GetComponent<Player>().hasKey = true;
            // Update the UI
            UI.instance.ToogleKeyIcon(true);
            // destroy the key object
            Destroy(gameObject);
        }
    }
}
