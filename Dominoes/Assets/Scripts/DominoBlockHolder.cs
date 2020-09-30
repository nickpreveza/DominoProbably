using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoBlockHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int totalBlocks;

    public Sprite[] dominoSprites;

    public List<DominoBlockData> unusedBlocks;
    public List<DominoBlockData> playedBlocks;
    public List<DominoBlockData> playerHand;
    public List<DominoBlockData> opponentHand;

    public static event System.Action OnDeckChanged;

    public int openLeftPosition;
    public int openRightPosition;

    void Awake()
    {
        totalBlocks = 28;

        unusedBlocks = new List<DominoBlockData>();
        playedBlocks = new List<DominoBlockData>();
        playerHand = new List<DominoBlockData>();
        opponentHand = new List<DominoBlockData>();



        for (int i = 0; i < totalBlocks; i++)
        {
            DominoBlockData dominoBlockData = new DominoBlockData();

            if (dominoSprites[i] != null)
            {
                string[] downup = dominoSprites[i].name.Split('-');

                dominoBlockData.downValue = int.Parse(downup[0]);
                dominoBlockData.upValue = int.Parse(downup[1]);
                dominoBlockData.sprite = dominoSprites[i];

                dominoBlockData.name = dominoSprites[i].name;

                dominoBlockData.totalValue = dominoBlockData.downValue + dominoBlockData.upValue;
                unusedBlocks.Add(dominoBlockData);
            }
            else
            {
                Debug.LogError("Sprites Missing");
            }

        }

        //StartingHand();
    }


    public void MoveBlock(List<DominoBlockData> originList, DominoBlockData block, List<DominoBlockData> targetList)
    {
        targetList.Add(block);
        originList.Remove(block);
        if (OnDeckChanged != null)
        {
            OnDeckChanged();
        }
    }

    public void StartingHand(int numberOfBlocks)
    {
        for (int i = 0; i < numberOfBlocks; i++)
        {
            MoveBlock(unusedBlocks, unusedBlocks[ReturnRandomIndexOfList(unusedBlocks)], playerHand);
            MoveBlock(unusedBlocks, unusedBlocks[ReturnRandomIndexOfList(unusedBlocks)], opponentHand);
        }
    }

    public void PlayFirstBlock(List<DominoBlockData> originList, int index)
    {
        DominoBlockData block = originList[index];
        MoveBlock(originList, block, playedBlocks);
        openLeftPosition = playedBlocks[0].downValue;
        openRightPosition = playedBlocks[0].upValue;

        if (OnDeckChanged != null)
        {
            OnDeckChanged();
        }
    }

    public void PlayBlock(List<DominoBlockData> originList, int index)
    {
        DominoBlockData block = originList[index];
        MoveBlock(originList, block, playedBlocks);
        if (block.downValue == openLeftPosition)
        {
            block.upValue = openLeftPosition;
        }
        if (block.downValue == openRightPosition)
        {
            block.upValue = openRightPosition;
        }
        if (block.upValue == openLeftPosition)
        {
            block.downValue = openLeftPosition;
        }
        if (block.upValue == openRightPosition)
        {
            block.downValue = openRightPosition;
        }

        if (OnDeckChanged != null)
        {
            OnDeckChanged();
        }
    }

    public int ReturnRandomIndexOfList(List<DominoBlockData> targetList)
    {
        int index = Random.Range(0, targetList.Count - 1);
        return index;
    }

    public bool Evaluate(DominoBlockData block)
    {
        if (block.downValue == openLeftPosition || block.upValue == openLeftPosition || block.downValue == openRightPosition || block.downValue == openRightPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    // Legacy - misunderstood the rules 

    /*
    public IEnumerator FirstTurn()
    {
        for (int i = 0; i < 1; i++)
        {
            MoveBlock(unusedBlocks, unusedBlocks[ReturnRandomIndexOfList(unusedBlocks)], playerHand);
            playerHigherTotal = playerHand[0].totalValue;
            MoveBlock(unusedBlocks, unusedBlocks[ReturnRandomIndexOfList(unusedBlocks)], opponentHand);
            opponentHigherTotal = opponentHand[0].totalValue;
        }

        yield return new WaitForSeconds(3);

        for (int i = 0; i < 1; i++)
        {
            MoveBlock(playerHand, playerHand[ReturnRandomIndexOfList(playerHand)], unusedBlocks);
            MoveBlock(opponentHand, opponentHand[ReturnRandomIndexOfList(opponentHand)], unusedBlocks);
        }

        yield return new WaitForSeconds(1);

        StartingHand(7);
    }
    */





}
