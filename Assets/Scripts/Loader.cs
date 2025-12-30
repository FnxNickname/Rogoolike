using UnityEngine;
using System.Collections;
public class Loader : MonoBehaviour { public GameObject gameManagerPrefab; 
    void Awake() 
    { 
        if (gameManager.instance == null) 
        { 
            Instantiate(gameManagerPrefab); 
        } 
    } 
}