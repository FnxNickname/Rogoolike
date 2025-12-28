using UnityEngine;

public class gameManager : MonoBehaviour
{
    public BoardManager boardScript;

    private int level = 3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
