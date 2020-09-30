using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableTopGrid : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }


    public void OnDrop(PointerEventData eventData)
    {
        Dragable block = eventData.pointerDrag.GetComponent<Dragable>();
        if(block != null)
        {
            block.parentToReturnTo = this.transform;
            block.transform.Rotate(0, 0, 90);
            // or -90
        }
    }
}
