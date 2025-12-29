using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (gameManager.instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}
