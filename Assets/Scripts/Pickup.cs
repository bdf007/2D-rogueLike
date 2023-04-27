using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PickupType 
{
    Coin, 
    Health
}

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the pickup object has collided with the player,
        if(collision.CompareTag("Player"))
        {
            // if the pickup type is 'Coin'
            if(type == PickupType.Coin)
            {
                collision.GetComponent<Player>().AddCoins(value);
                Destroy(gameObject);
            }

            // if the pickup type is 'Health'
            if(type == PickupType.Health)
            {
                if (collision.GetComponent<Player>().curHp == collision.GetComponent<Player>().maxHp)
                {
                    return;
                }
                collision.GetComponent<Player>().AddHealth(value);
                Destroy(gameObject);
            }
        }
    }
}
