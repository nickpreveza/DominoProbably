using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoBlock : MonoBehaviour
{
    public string blockName;
    public Sprite sprite;

    public int upValue;
    public int downValue;
    public int totalValue;

    public int indexPosition;

    GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();    
        
    }
    public void ButtonFunction()
    {
        gameManager.ButtonFunction(indexPosition);
    }
}
