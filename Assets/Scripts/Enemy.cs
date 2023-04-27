using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // reference to the Player class
    public Player player;

    [Header("stats")]
    public int health;
    public int damage;
    public float attackChance = 0.5f;

    public GameObject deathDropPrefab;
    public SpriteRenderer sr;

    public LayerMask moveLayerMask;

    void Start()
    {
        // find the player in the scene
        player = FindObjectOfType<Player>();
    }


    public void TakeDamage (int damageToTake)
    {
        // subtract the damage from the health
        health -= damageToTake;

        // if the health is less than or equal to 0
        if(health <= 0)
        {
            // destroy the enemy
            Destroy(gameObject);

            // instantiate a death drop if it exists
            if(deathDropPrefab != null)
            {
                Instantiate(deathDropPrefab, transform.position, Quaternion.identity);
            }
        }

        // start the DamageFlash coroutine
        StartCoroutine(DamageFlash());

        // If a random value is greater than attackChance,
        if(Random.value > attackChance)
        {
            // deal damage to the player.
            player.TakeDamage(damage);
        }
        
    }

    // coroutine to flash the sprite when taking damage
    IEnumerator DamageFlash ()
    {
        // get the reference to the default sprite color (red)
        Color defaultColor = sr.color;
        // set the color to white
        sr.color = Color.white;

        //wait for 0.05 seconds
        yield return new WaitForSeconds(0.05f);

        // set the color back to the default color
        sr.color = defaultColor;
    }
    public void Move ()
    {
        if(Random.value < 0.5f)
        {
            return;
        }

        Vector3 dir = Vector3.zero;
        bool canMove = false;

        // before moving the enemy, get a random direction to move to
        while(canMove == false)
        {
            dir = GetRandomDirection();
            // cast a ray into the direction.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.0f, moveLayerMask);

            // if the ray hasn't detected any obstacle,
            if (hit.collider == null)
            {
                // we can move
                canMove = true;
            }
        }

        // move towards the direction
        transform.position += dir;
    }

    Vector3 GetRandomDirection ()
    {
        // Get a random number between 0 and 4
        int rand = Random.Range(0, 4);

        // depending on the random number, return a direction
        switch (rand)
        {
            case 0:
                return Vector3.up;
            case 1:
                return Vector3.down;
            case 2:
                return Vector3.left;
            case 3:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }
}
