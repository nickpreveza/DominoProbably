using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base data class for Domino pieces 
[System.Serializable]
public class DominoBlockData
{
    public string name;
    public Sprite sprite;

    public int upValue;
    public int downValue;
    public int totalValue;
}
