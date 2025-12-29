using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Player : MovingObject 
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsperBeer = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = gameManager.instance.playerFoodPoints;

        base.Start();

    }

    private void OnDisable()
    {
        gameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if(horizontal !=0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;
        base.AttemptMove <T> (xDir, yDir);
        CheckIfGameOver();
        gameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if(other.tag=="Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag=="Beer")
        {
            food += pointsperBeer;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerAttack1");
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food-=loss;
        CheckIfGameOver() ;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            gameManager.instance.GameOver();
        }
    }
}
