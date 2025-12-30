using UnityEngine;
using System.Collections;


public class Enemy : MovingObject
{
    public int playerDamage;

    
    private Transform target;
    private bool skipMove;
    protected override void Start()
    {
        gameManager.instance.AddEnemyToList(this);
       
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }


    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        


        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }


    public void MoveEnemy()
    {
        if (target == null)
            return;

        int enemyX = Mathf.RoundToInt(transform.position.x);
        int enemyY = Mathf.RoundToInt(transform.position.y);
        int playerX = Mathf.RoundToInt(target.position.x);
        int playerY = Mathf.RoundToInt(target.position.y);

        int xDiff = playerX - enemyX;
        int yDiff = playerY - enemyY;
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(xDiff) + Mathf.Abs(yDiff) == 1)
        {
            AttemptMove<Player>(xDiff, yDiff);
            return;
        }
        if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            xDir = xDiff > 0 ? 1 : -1;

            if (IsPathBlocked(xDir, 0))
            {
                if (yDiff != 0)
                {
                    xDir = 0;
                    yDir = yDiff > 0 ? 1 : -1;
                }
            }
        }
        else
        {
            yDir = yDiff > 0 ? 1 : -1;

            if (IsPathBlocked(0, yDir))
            {
                if (xDiff != 0)
                {
                    yDir = 0;
                    xDir = xDiff > 0 ? 1 : -1;
                }
            }
        }

        AttemptMove<Player>(xDir, yDir);
    }


    private bool IsPathBlocked(int xDir, int yDir)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        GetComponent<BoxCollider2D>().enabled = false;

        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);

        GetComponent<BoxCollider2D>().enabled = true;

        if (hit.transform == null) return false;

        if (hit.transform.CompareTag("Player")) return false;

        return true;
    }
    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
}
