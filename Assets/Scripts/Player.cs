using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public int curHp;
    public int maxHp;
    public int coins;

    public bool hasKey;

    public SpriteRenderer sr;

    // layer to avoid (mask)
    public LayerMask moveLayerMask;

    void Move(Vector2 dir)
    {
        // cast a ray from the player's position to the direction of movement, and detect colliders that are layered as "moveLayerMask".
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.0f, moveLayerMask);

        // if there is no moveLayerMask detected in front of us
        if (hit.collider == null)
        {
            // move forward
            transform.position += new Vector3(dir.x, dir.y, 0);
            // move enemies
            EnemyManager.instance.OnPlayerMove();
            // update minimap
            Generation.instance.OnPlayerMove();
        }
    }


    public void OnMoveUp(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if(context.phase == InputActionPhase.Performed)
        {
            // move up
            Move(Vector2.up);
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // move down
            Move(Vector2.down);
        }

        
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // move left
            Move(Vector2.left);
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // move right
            Move(Vector2.right);
        }
    }

    public void OnAttackUp(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // attack up
            TryAttack(Vector2.up);
        }
    }

    public void OnAttackDown(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // attack down
            TryAttack(Vector2.down);
        }
    }

    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key? 
        if (context.phase == InputActionPhase.Performed)
        {
            // attack left
            TryAttack(Vector2.left);
        }
    }

    public void OnAttackRight(InputAction.CallbackContext context)
    {
        // have we pressed down the corresponding key?
        if (context.phase == InputActionPhase.Performed)
        {
            // attack right
            TryAttack(Vector2.right);
        }
    }

    public void TakeDamage (int damageToTake)
    {
        curHp -= damageToTake;
        // update the UI
        UI.instance.UpdateHealth(curHp);

        StartCoroutine(DamageFlash());

        // load the main menu scene if the player's hp is 0 or less
        if (curHp <= 0)
        {
            SceneManager.LoadScene(0);
        }

    }

    void TryAttack (Vector2 dir)
    {
        // ignore the layer 1 to layer 8. (only detect Layer 9: Enemy)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.0f, 1 << 9);

        // if the ray has hit an enemy (collider),
        if (hit.collider != null)
        {
            // the enemy takes damage
            hit.collider.GetComponent<Enemy>().TakeDamage(1);
        }
    }

    // coroutine to flash the sprite when taking damage
    IEnumerator DamageFlash()
    {
        // get the reference to the default sprite color (green)
        Color defaultColor = sr.color;
        // set the color to white
        sr.color = Color.white;

        //wait for 0.05 seconds
        yield return new WaitForSeconds(0.05f);

        // set the color back to the default color
        sr.color = defaultColor;
    }

    public void AddCoins (int amount)
    {
        coins += amount;
        // update the UI
        UI.instance.UpdateCoinText(coins);
    }

    public bool AddHealth(int amount)
    {
        if(curHp + amount <= maxHp)
        {
            curHp += amount;
            // update the UI
            UI.instance.UpdateHealth(curHp);
            return true;
        }
        return false;
    }
}
