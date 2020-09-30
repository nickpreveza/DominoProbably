using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DominoDisplay : MonoBehaviour
{
    public GameObject dominoBlockPrefab;
    public GameObject dominoBlockOpponentPrefab;

    public GameObject opponentHolder;
    public GameObject playerHolder;
    public GameObject tabletTopHolder;

    public GameObject[] playerDeck;
    public GameObject[] opponentDeck;

    public GameObject[] playedBlocks;
   

    public DominoBlockHolder dominoBlockHolder;
    public GameManager gameManager;

    int currentPlayerHand;
    int currentOpponentHand;
    int currentPlayedCards;

    void Awake()
    {
        
        dominoBlockHolder = GetComponent<DominoBlockHolder>();
        gameManager = GetComponent<GameManager>();

        DominoBlockHolder.OnDeckChanged += UpdateDisplayBasedOnData;

        for (int i = 0; i < 7; i++)
        {
            GameObject playerGameObject = Instantiate(dominoBlockPrefab);
            playerGameObject.transform.SetParent(playerHolder.transform, false);
            playerGameObject.SetActive(false);

            GameObject opponentGameObject = Instantiate(dominoBlockOpponentPrefab);
            opponentGameObject.transform.SetParent(opponentHolder.transform, false);
            opponentGameObject.SetActive(false);
        }

        for(int i = 0; i < 28; i++)
        {
            GameObject tableGameObject = Instantiate(dominoBlockPrefab);
            tableGameObject.transform.SetParent(tabletTopHolder.transform, false);
            tableGameObject.SetActive(false);
        }
    }


    public void UpdateDisplayBasedOnData()
    {
        DisplayDominoBlocks(playerHolder.transform, dominoBlockHolder.playerHand);
        DisplayDominoBlocks(opponentHolder.transform, dominoBlockHolder.opponentHand);
        DisplayDominoBlocks(tabletTopHolder.transform, dominoBlockHolder.playedBlocks);
    }

   
 

    public void DisplayDominoBlocks(Transform holder, List<DominoBlockData> originList)
    {
        int currentHand = originList.Count;

        for (int i = currentHand; i < holder.childCount; i++)
        {
            holder.GetChild(i).gameObject.SetActive(false);
        }

        if (currentHand > 0)
        {
            for (int i = 0; i < currentHand; i++)
            {
                DominoBlock block = holder.GetChild(i).gameObject.GetComponent<DominoBlock>();

                block.blockName = originList[i].name;
                block.upValue = originList[i].upValue;
                block.downValue = originList[i].downValue;
                block.totalValue = originList[i].totalValue;
                block.sprite = originList[i].sprite;
                block.indexPosition = i;

                Image image = block.gameObject.GetComponent<Image>();
                image.sprite = block.sprite;

                block.gameObject.SetActive(true);
            }
        }

        else if (currentPlayerHand == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                Transform currentBlockToUpdate = playerHolder.transform.GetChild(i);
                currentBlockToUpdate.gameObject.SetActive(false);
            }
        }
    }
}
