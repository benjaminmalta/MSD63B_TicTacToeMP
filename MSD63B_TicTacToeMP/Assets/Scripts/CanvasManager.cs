using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Sprite fruitApple;
    public Sprite fruitStrawberry;
    public Sprite fruitNone;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        AssignBoardPieceClicks();

        //temp code -- will be deleted afterwards
        //GameObject boardPiece1 = GameObject.Find("Loc0-0");
        //GameObject boardPiece2 = GameObject.Find("Loc0-1");

        //boardPiece1.GetComponent<BoardPiece>().SetFruit(Fruit.FruitType.Apple);
        //boardPiece2.GetComponent<BoardPiece>().SetFruit(Fruit.FruitType.Strawberry);

        //BoardPaint(boardPiece1);
        //BoardPaint(boardPiece2);
        //ChangeBottomLabel("hello there");
        //ChangeTopName("android", "pc");

        //end of temp code

    }

    //draw the respective fruit for gameobject boardpiece
    public void BoardPaint(GameObject gameObjBoardPiece)
    {
        BoardPiece boardPiece = gameObjBoardPiece.GetComponent<BoardPiece>();
        if (boardPiece.GetFruit() == Fruit.FruitType.Apple)
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitApple;
        else if (boardPiece.GetFruit() == Fruit.FruitType.Strawberry)
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitStrawberry;
        else
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitNone;
    }

    public void ChangeBottomLabel(string message)
    {
        transform.Find("PanelBottom/LblMessage").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void ChangeTopName(string player1Name, string player2Name)
    {
        transform.Find("PanelTop/Player1Label").GetComponent<TextMeshProUGUI>().text = player1Name;
        transform.Find("PanelTop/Player2Label").GetComponent<TextMeshProUGUI>().text = player2Name;
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
        gameManager.SelectBoardPiece(pointerEventData.pointerClick.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
