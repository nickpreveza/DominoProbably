using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DominoBlockHolder))]
[RequireComponent(typeof(DominoDisplay))]
public class GameManager : MonoBehaviour
{
    public int startingHand;
    public int numberOfPlayers;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject tieUI;

    public enum State { START, PLAYERTURN, AITURN, WIN, LOSE, TIE };
    public State currentState;

    DominoBlockHolder dominoBlockHolder;
    DominoDisplay dominoDisplay;

    public bool hasPlayerPlayedBlock;
    public bool hasAIPlayedBlock;

    int playerDominoesLeft;
    int aIDominoesLeft;

    bool canPlayerMove;
    bool canAIMove;


    void Start()
    {
        hasPlayerPlayedBlock = false;
        hasAIPlayedBlock = false;
        canPlayerMove = true;
        canAIMove = true;

        winUI.SetActive(false);
        loseUI.SetActive(false);
        tieUI.SetActive(false);

        currentState = State.START;

        startingHand = 7;
        numberOfPlayers = 2;

        playerDominoesLeft = startingHand;
        aIDominoesLeft = startingHand;

        dominoBlockHolder = GetComponent<DominoBlockHolder>();
        dominoDisplay = GetComponent<DominoDisplay>();

        StartCoroutine(StartUp());

    }

    IEnumerator StartUp()
    {
        dominoBlockHolder.StartingHand(startingHand);

        CalculateFirstPlayer();
        print("Starting");

        yield return new WaitForSeconds(1);
    }


    public void CheckGameRequirments()
    {
        if (dominoBlockHolder.playerHand.Count == 0)
        {
            print("Domino!");
            currentState = State.WIN;
            EndGame();
        }

        if (dominoBlockHolder.opponentHand.Count == 0)
        {
            print("You got dominoed");
            currentState = State.LOSE;
            EndGame();
        }

        if (!canPlayerMove && !canAIMove)
        {
            print("Really? Can't move?");
            currentState = State.TIE;
            EndGame();
        }

        if (hasPlayerPlayedBlock)
        {
            currentState = State.AITURN;
            OpponentTurn();
        }

        if (hasAIPlayedBlock)
        {
            currentState = State.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    public void OpponentTurn()
    {
        print("AI Turn");
        if (currentState != State.AITURN)
        {
            return;
        }

        hasPlayerPlayedBlock = false;
        canPlayerMove = true;

        List<int> possibleMoves = new List<int>();

        for (int i = 0; i < dominoBlockHolder.opponentHand.Count; i++)
        {
            if (dominoBlockHolder.Evaluate(dominoBlockHolder.opponentHand[i]))
            {
                possibleMoves.Add(i);
                canAIMove = true;
            }

            if (!dominoBlockHolder.Evaluate(dominoBlockHolder.opponentHand[i]))
            {
                canAIMove = false;
            }
        }

        int random = Random.Range(0, possibleMoves.Count);

        if (!hasAIPlayedBlock && canAIMove)
        {
            dominoBlockHolder.PlayBlock(dominoBlockHolder.opponentHand, random);
            aIDominoesLeft--;
            hasAIPlayedBlock = true;
        }

        CheckGameRequirments();
    }

    IEnumerator PlayerTurn()
    {
        print("Player Turn");
        if (currentState != State.PLAYERTURN)
        {
            StopCoroutine(PlayerTurn());
        }

        hasAIPlayedBlock = false;
        canAIMove = true;

        int interactableButtons = 0;

        for (int i = 0; i < dominoBlockHolder.playerHand.Count; i++)
        {
            if (dominoBlockHolder.Evaluate(dominoBlockHolder.playerHand[i]))
            {
                dominoDisplay.playerHolder.transform.GetChild(i).GetComponent<Button>().interactable = true;
                interactableButtons++;
            }
            else
            {
                dominoDisplay.playerHolder.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }

        if (interactableButtons > 0)
        {
            canPlayerMove = true;
        }

        if (interactableButtons <= 0)
        {
            canPlayerMove = false;
            hasPlayerPlayedBlock = true;
            //CheckGameRequirments();
        }

        CheckGameRequirments();

        while (!hasPlayerPlayedBlock)
        {
            yield return null;

        }

        CheckGameRequirments();

    }

    public void CalculateFirstPlayer()
    {
        int playerBiggestDouble = 0;
        int opponentBiggestDouble = 0;
        int playerBlockListPosition = 0;
        int AIBlockListPosition = 0;

        for (int i = 0; i < startingHand; i++)
        {

            if (dominoBlockHolder.playerHand[i].downValue == dominoBlockHolder.playerHand[i].upValue)
            {
                if (dominoBlockHolder.playerHand[i].totalValue > playerBiggestDouble)
                {
                    playerBlockListPosition = i;
                    playerBiggestDouble = dominoBlockHolder.playerHand[i].totalValue;
                }
            }

            if (dominoBlockHolder.opponentHand[i].downValue == dominoBlockHolder.opponentHand[i].upValue)
            {
                if (dominoBlockHolder.opponentHand[i].totalValue > opponentBiggestDouble)
                {
                    AIBlockListPosition = i;
                    opponentBiggestDouble = dominoBlockHolder.opponentHand[i].totalValue;
                }
            }
        }

        if (playerBiggestDouble >= opponentBiggestDouble)
        {
            dominoBlockHolder.PlayFirstBlock(dominoBlockHolder.playerHand, playerBlockListPosition);
            currentState = State.AITURN;
            OpponentTurn();
            print("AI turn");
        }
        else if (playerBiggestDouble < opponentBiggestDouble)
        {
            dominoBlockHolder.PlayFirstBlock(dominoBlockHolder.opponentHand, AIBlockListPosition);
            currentState = State.PLAYERTURN;
            StartCoroutine(PlayerTurn());
            print("Player turn");
        }
    }


    void EndGame()
    {
        if (currentState == State.WIN)
        {
            winUI.SetActive(true);
        }
        if (currentState == State.LOSE)
        {
            loseUI.SetActive(true);
        }
        if (currentState == State.TIE)
        {
            tieUI.SetActive(true);
        }
        
    }

    public void ButtonFunction(int index)
    {
        dominoBlockHolder.PlayBlock(dominoBlockHolder.playerHand, index);
        playerDominoesLeft--;
        hasPlayerPlayedBlock = true;
    }




}
