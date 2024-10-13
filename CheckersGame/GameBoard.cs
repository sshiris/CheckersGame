class GameBoard
{
    public char[,] board;
    public int SIZE = 8;

    //constructor, when the GameBoard is initialized, the 8*8 board will be set up filled with '-'
    public GameBoard()
    {
        board = new char[SIZE, SIZE];

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                board[i, j] = '-';
            }
        }

    }

    //print Gameboard
    public void PrintGameBoard()
    {
        Console.WriteLine("  0 1 2 3 4 5 6 7");
        for (int i = 0; i < board.GetLength(0); i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public void InitializeBoardForPlayers(Player player1, Player player2)
    {
        //set player1's pieces, top 3 rows
        for (int i = 0; i < 3; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 1; j < SIZE; j += 2)
                {
                    board[i, j] = player1.pieceSymbol;
                    player1.playerPieces.Add(new Piece(GetTheArrayPosition(i, j)));

                }
            }
            else
            {
                for (int j = 0; j < SIZE; j += 2)
                {
                    board[i, j] = player1.pieceSymbol;
                    player1.playerPieces.Add(new Piece(GetTheArrayPosition(i, j)));
                }
            }
        }

        //set player2's pieces, botom 3 rows
        for (int i = 5; i < 8; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 1; j < SIZE; j += 2)
                {
                    board[i, j] = player2.pieceSymbol;
                    player2.playerPieces.Add(new Piece(GetTheArrayPosition(i, j)));

                }
            }
            else
            {
                for (int j = 0; j < SIZE; j += 2)
                {
                    board[i, j] = player2.pieceSymbol;
                    player2.playerPieces.Add(new Piece(GetTheArrayPosition(i, j)));
                }
            }
        }
    }

    public static string GetTheArrayPosition(int x, int y)
    {
        return x.ToString() + y.ToString();
    }

    public void MoveMethods(Player player, Player opponentPlayer, string startPosition, string endPosition)
    {

        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        //move one piece 
        if (Math.Abs(startPositionX - endPositionX) == 1 && Math.Abs(startPositionY - endPositionY) == 1)
        {
            board[startPositionX, startPositionY] = '-';

            board[endPositionX, endPositionY] = player.pieceSymbol;
        }
        //make one jump move
        int midX = (startPositionX + endPositionX) / 2;
        int midY = (startPositionY + endPositionY) / 2;

        if (Math.Abs(startPositionX - endPositionX) == 2 && Math.Abs(startPositionY - endPositionY) == 2)
        {
            board[startPositionX, startPositionY] = '-';
            board[endPositionX, endPositionY] = player.pieceSymbol;
            board[midX, midY] = player.pieceSymbol;
            opponentPlayer.RemoveCapturedPiece(new Piece(GetTheArrayPosition(midX, midY)));
            //check if a piece has made to be a king
        }
    }

    public bool IsValidMove(Player player, string startPosition, string endPosition)
    {
        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        //check if the start and end position is within the board
        if (startPositionX < 0 || startPositionX >= SIZE || startPositionY < 0 || startPositionY >= SIZE
        || endPositionX < 0 || endPositionX >= SIZE || endPositionY < 0 || endPositionY >= SIZE)
        {
            return false;
        }

        //check if the start position has the player's piece
        if (board[startPositionX, startPositionY] != player.pieceSymbol)
        {
            return false;
        }

        //check the end position, if it is empty
        if (board[endPositionX, endPositionY] != '-')
        {
            return false;
        }

        //check when player makes jump, if the piece next to it is opponent's piece
        int midX = (startPositionX + endPositionX) / 2;
        int midY = (startPositionY + endPositionY) / 2;

        if (Math.Abs(startPositionX - endPositionX) == 2 && Math.Abs(startPositionY - endPositionY) == 2)
        {
            if (board[midX, midY] == '-' || board[midX, midY] == player.pieceSymbol)
            {
                return false;
            }
        }
        return true;
    }

    public void MovePiece(Player player, Player opponentPlayer, string startPosition, string endPosition)
    {
        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        bool isValidMove = IsValidMove(player, startPosition, endPosition);

        if (isValidMove)
        {
            //check if the piece you are moving is King first
            foreach (Piece piece in player.playerPieces)
            {
                if (piece.isKing)
                {
                    MoveMethods(player, opponentPlayer, startPosition, endPosition);

                }
                else
                {
                    if (player.pieceSymbol == 'X')
                    {
                        if (endPositionX > startPositionX)
                        {
                            MoveMethods(player, opponentPlayer, startPosition, endPosition);
                        }
                    }

                    if (player.pieceSymbol == 'O')
                    {
                        if (endPositionX < startPositionX)
                        {
                            MoveMethods(player, opponentPlayer, startPosition, endPosition);
                        }
                    }
                }
            }

        }
    }


}