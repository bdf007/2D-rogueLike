using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    public static EnemyManager instance;

    void Awake()
    {
        instance = this;
    }

    public void OnPlayerMove ()
    {
        // start the MoveEnemies coroutine
        StartCoroutine(MoveEnemies());
    }

    // coroutine
    IEnumerator MoveEnemies ()
    {
        yield return new WaitForFixedUpdate();

        // loop through all of the enemies in the scene
        foreach(Enemy enemy in enemies)
        {
            // if the enemy is not dead
            if(enemy != null)
            {
                // move the enemy
                enemy.Move();
            }
        }
    }
}
