using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameManager gameManager;
    public Transform parentToReturnTo = null;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(gameManager.currentState == GameManager.State.PLAYERTURN)
        {
            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent.parent);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = false;
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameManager.currentState == GameManager.State.PLAYERTURN)
        {
            this.transform.position = eventData.position;
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
