using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public Sprite fruitApple;
    public Sprite fruitStrawberry;
    public Sprite fruitNone;

    // Start is called before the first frame update
    void Start()
    {
        AssignBoardPieceClicks();
    }

    private void AssignBoardPieceClicks()
    {
        //going to loop through all board pieces by using object's name

        for (int i = 0; i < 3; i++) //iterate rows
        {
            for(int j = 0; j < 3; j++) //iterate columns
            {
                GameObject boardPiece = this.transform.Find("PanelBoardPieces/Loc" + i + "-" + j).gameObject;
                //now change the row and column values inside the class BoardPiece
                boardPiece.GetComponent<BoardPiece>().row = i;
                boardPiece.GetComponent<BoardPiece>().column = j;

                //load the EventTrigger component from the boardpiece
                EventTrigger eventTrigger = boardPiece.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();

                entry.eventID = EventTriggerType.PointerClick;
                entry.callback = new EventTrigger.TriggerEvent();

                UnityEngine.Events.UnityAction<BaseEventData> callback =
                    new UnityEngine.Events.UnityAction<BaseEventData>(GameBoardPieceEventMethod);

                entry.callback.AddListener(callback);
                eventTrigger.triggers.Add(entry);
            }
        }
    }

    /// <summary>
    /// This method is going to be called automatically when the player clicks on the board piece
    /// </summary>
    /// <param name="baseEvent">contains data about the gameobject clicked</param>
    public void GameBoardPieceEventMethod(UnityEngine.EventSystems.BaseEventData baseEvent)
    {
        //get information about gameobject (button/image) that was clicked
        PointerEventData pointerEventData = (PointerEventData)baseEvent;
        print("GameObjectClicked:" + pointerEventData.pointerClick.gameObject.name);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
