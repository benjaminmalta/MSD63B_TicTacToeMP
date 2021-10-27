using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public List<Player> players = new List<Player>();
    public CanvasManager canvasManager;
    public Player currentActivePlayer;
    public BoardPiece[,] BoardMap = new BoardPiece[3, 3]; //a 2d Array

    // Start is called before the first frame update
    void Start()
    {
        //-start of temp code --later on we will replace this with data from photon
        //players.Add(new Player() { id = Player.Id.Player1, nickname = "P1",
        //    assignedFruit = Fruit.FruitType.Apple });

        //players.Add(new Player() { id = Player.Id.Player2, nickname = "P2",
        //    assignedFruit = Fruit.FruitType.Strawberry });
        //--end of temp code

        Photon.Realtime.Player[] allPlayers = PhotonNetwork.PlayerList;
        foreach(Photon.Realtime.Player player in allPlayers)
        {
            if (player.ActorNumber == 1)
                players.Add(new Player()
                {
                    id = Player.Id.Player1,
                    nickname = player.NickName,
                    assignedFruit = Fruit.FruitType.Apple
                });
            else if(player.ActorNumber == 2)
            {
                players.Add(new Player() { id = Player.Id.Player2, nickname = player.NickName,
                    assignedFruit = Fruit.FruitType.Strawberry });
            }

        }


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

            //check for a winner
            bool win = CheckWinner(boardPiece);
            if (win)
            {
                print("Detected winner. Winner is:" + currentActivePlayer.nickname);
                canvasManager.ChangeBottomLabel("Winner:" + currentActivePlayer.nickname);
            }
            else
            {
                //check if game is over - check if draw
                if (IsGameDraw())
                {
                    print("Game is draw");
                    canvasManager.ChangeBottomLabel("Game Draw");
                }
                else
                {
                    print("Game is not draw. Continue playing..");
                    ChangeActivePlayer();
                }
            }



            //change active player
            //ChangeActivePlayer();
        }
    }

    private bool CheckWinner(BoardPiece boardPiece)
    {
        //check rows
        int rowCounter = 0;
        for (int i=0; i < 3; i++)
        {
            BoardPiece tmpBoardPiece = BoardMap[boardPiece.row, i];
            if (tmpBoardPiece != null)
            {
                if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
                    rowCounter += 1;
            }
        }

        if (rowCounter == 3)
        {
            print("Similar in row");
            return true;
        }

        //check the column
        int colCounter = 0;
        for(int i = 0; i < 3; i++)
        {
            BoardPiece tmpBoardPiece = BoardMap[i, boardPiece.column];
            if(tmpBoardPiece != null)
            {
                if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
                    colCounter += 1;
            }
        }
        if (colCounter == 3)
        {
            print("Similar in column");
            return true;
        }

        //check diagonal 1
        int diagOneCounter = 0;
        int diagCol1 = -1;

        for(int i = 0; i < 3; i++)
        {
            diagCol1 += 1;
            BoardPiece tmpBoardPiece = BoardMap[i, diagCol1];
            //i->0; diagCol1->0
            //i->1; diagCol1->1
            //i->2; diagCol1 ->2
            if(tmpBoardPiece != null)
            {
                if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
                    diagOneCounter += 1;
            }
        }

        if(diagOneCounter == 3)
        {
            print("Similar in diag 1");
            return true;
        }

        //check diagonal 2
        int diagTwoCounter = 0;
        int diagCol2 = 3;

        for(int i=0; i < 3; i++)
        {
            diagCol2 -= 1;
            BoardPiece tmpBoardPiece = BoardMap[i, diagCol2];
            //i->0; diagCol2->2
            //i->1; diagCol2->1
            //i->2; diagCol2->0
            if(tmpBoardPiece != null)
            {
                if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
                    diagTwoCounter += 1;
            }
        }

        if(diagTwoCounter==3)
        {
            print("Similar in diag2");
            return true;
        }

        return false;


    }

    private bool IsGameDraw()
    {
        foreach(BoardPiece boardPiece in BoardMap)
        {
            if (boardPiece == null)
                return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
