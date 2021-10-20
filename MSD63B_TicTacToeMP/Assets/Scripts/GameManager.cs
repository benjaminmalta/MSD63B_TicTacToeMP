using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public CanvasManager canvasManager;
    public Player currentActivePlayer;
    public BoardPiece[,] BoardMap = new BoardPiece[3, 3]; //a 2d Array

    // Start is called before the first frame update
    void Start()
    {
        //-start of temp code --later on we will replace this with data from photon
        players.Add(new Player() { id = Player.Id.Player1, nickname = "P1",
            assignedFruit = Fruit.FruitType.Apple });

        players.Add(new Player() { id = Player.Id.Player2, nickname = "P2",
            assignedFruit = Fruit.FruitType.Strawberry });
        //--end of temp code

        ChangeTopNames();
        ChangeActivePlayer();
   
    }

    //can be called to change active player. so if player1 ends his turn, we need to shift to player2
    //and vice versa
    public void ChangeActivePlayer()
    {
        if (currentActivePlayer == null)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player1); //by default assign player1
        else if (currentActivePlayer.id == Player.Id.Player1)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player2); //set player2
        else if (currentActivePlayer.id == Player.Id.Player2)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player1); //set player1

        //notify the canvasManager that player is changed
        canvasManager.ChangeBottomLabel("Player Turn:" + currentActivePlayer.nickname);

    }

    private void ChangeTopNames()
    {
        string player1Name = players.Find(x => x.id == Player.Id.Player1).nickname;
        string player2Name = players.Find(x => x.id == Player.Id.Player2).nickname;

        canvasManager.ChangeTopName(player1Name, player2Name);
    }

    //this method is going to be called from CanvasManager when the player clicks on a particular BoardPiece
    public void SelectBoardPiece(GameObject gameObjBoardPiece)
    {
        BoardPiece boardPiece = gameObjBoardPiece.GetComponent<BoardPiece>();

        if(boardPiece.GetFruit() == Fruit.FruitType.None) //if it is empty (not assigned a fruit)
        {   
            //set fruit according to player's turn
            boardPiece.SetFruit(currentActivePlayer.assignedFruit);

            //update the board map
            BoardMap[boardPiece.row, boardPiece.column] = boardPiece;

            //notify canvas manager to draw updated board
            canvasManager.BoardPaint(gameObjBoardPiece);

            //change active player
            ChangeActivePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
