using System.Data;

class Game
{
    GameBoard board;
    Player currentPlayer;
    Player player1;
    Player player2;
    string startPosition;
    string endPosition;

    public Game()
    {
        //initialize board
        board = new GameBoard();

        //set players and board to play
        player1 = new Player("player1", 'X');
        player2 = new Player("player2", 'O');
        currentPlayer = player1;
    }

    public void Start()
    {
        board.InitializeBoardForPlayers(player1, player2);
        Console.WriteLine("Welcome to the game, have fun!");
        board.PrintGameBoard();

        while (true)
        {
            HandleMove();
            board.PrintGameBoard();
            SwitchPlayer();
        }


    }

    void SwitchPlayer()
    {
        currentPlayer = currentPlayer == player1 ? player2 : player1;
    }

    private Player GetOpponent()
    {
        return (currentPlayer == player1 ? player2 : player1);
    }

    public void HandleMove()
    {
        Console.WriteLine($"{currentPlayer.name}'s turn({currentPlayer.pieceSymbol}), enter position(row coloum) eg: 00, 01");
        Console.Write("start position: ");
        startPosition = Console.ReadLine();
        Console.Write("end position: ");
        endPosition = Console.ReadLine();


        bool isValidInput = IsValidInput(startPosition, endPosition);
        while (!isValidInput)
        {
            Console.WriteLine("Invalid move, try again.");

            Console.WriteLine($"{currentPlayer.name}'s turn({currentPlayer.pieceSymbol}), enter position(row coloum) eg: 00, 01");
            Console.Write("start position: ");
            startPosition = Console.ReadLine();
            Console.Write("end position: ");
            endPosition = Console.ReadLine();

            isValidInput = IsValidInput(startPosition, endPosition);
        }

        board.MovePiece(currentPlayer, GetOpponent(), startPosition, endPosition);
    }
    public bool IsValidInput(string startPosition, string endPosition)
    {

        if (board.IsValidMove(currentPlayer, startPosition, endPosition))
        {
            return true;

        }

        return false;
    }

}