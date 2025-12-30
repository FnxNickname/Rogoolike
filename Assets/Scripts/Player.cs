using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerBeer = 20;
    public float restartLevelDelay = 1f;
    public TextMeshProUGUI foodText;




    
    private int food;
    protected override void Start()
    {
        
        food = gameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;

        base.Start();

    }

    private void OnDisable()
    {
        gameManager.instance.playerFoodPoints = food;
    }


    void Update()
    {
        if (!gameManager.instance.playersTurn || isMoving)
            return;

        int horizontal = 0;
        int vertical = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) horizontal = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) horizontal = 1;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) vertical = 1;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) vertical = -1;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }



    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();
        gameManager.instance.playersTurn = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            gameManager.instance.AdvanceLevel();

            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + "Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Drink")
        {
            food += pointsPerBeer;
            foodText.text = "+" + pointsPerBeer + "Food: " + food;

            other.gameObject.SetActive(false);
        }
    }

    protected override void OnMoveComplete()
    {
        gameManager.instance.playersTurn = false;
    }



    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;

        if (hitWall != null)
        {
            hitWall.DamageWall(wallDamage);
            animator.SetTrigger("playerAttack1");
        }

        else
        {
            animator.SetTrigger("playerAttack1");
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHurt");
        food -= loss;
        foodText.text = "-" + loss + "Food: " + food;
        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            animator.SetTrigger("playerDie");

            gameManager.instance.StartGameOverSequence();

            enabled = false;
        }
    }




}