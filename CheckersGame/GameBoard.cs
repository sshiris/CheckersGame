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

    public void MoveMethods(Player player, string startPosition, string endPosition)
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
            board[midX, midY] = '-';

        }
    }

    public bool IsValidMove(Player player, string startPosition, string endPosition)
    {
        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        Piece startPiece = player.playerPieces.FirstOrDefault(p => p.position == startPosition);

        //check if the start and end position is within the board
        if (startPositionX < 0 || startPositionX >= SIZE || startPositionY < 0 || startPositionY >= SIZE
        || endPositionX < 0 || endPositionX >= SIZE || endPositionY < 0 || endPositionY >= SIZE)
        {
            return false;
        }

        //check if the start position has the player's piece
        if (startPiece == null)
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

        // Regular piece movement rules (forward only)

        if (startPiece != null && !startPiece.isKing)
        {
            if (player.pieceSymbol == 'X' && endPositionX <= startPositionX) // X can only move down
            {
                return false;
            }
            if (player.pieceSymbol == 'O' && endPositionX >= startPositionX) // O can only move up
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
            MoveMethods(player, startPosition, endPosition);
            UpdatePiece(player, opponentPlayer, startPosition, endPosition);
            KingMove(player, startPosition, endPosition);
            IsGameOver(player);
        }
    }

    public void UpdatePiece(Player player, Player opponentPlayer, string startPosition, string endPosition)
    {
        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        Piece startPiece = player.playerPieces.FirstOrDefault(p => p.position == startPosition);
        startPiece.position = endPosition;

        //if it is a jump, remove the captured piece from the opponent's pieces
        if (Math.Abs(startPositionX - endPositionX) == 2 && Math.Abs(startPositionY - endPositionY) == 2)
        {
            int capturedX = (startPositionX + endPositionX) / 2;
            int capturedY = (startPositionY + endPositionY) / 2;
            string capturedPosition = GetTheArrayPosition(capturedX, capturedY);

            Piece capturedPiece = opponentPlayer.playerPieces.FirstOrDefault(p => p.position == capturedPosition);
            if (capturedPiece != null)
            {
                opponentPlayer.RemoveCapturedPiece(capturedPiece);
            }

        }

        Piece endPiece = player.playerPieces.FirstOrDefault(p => p.position == endPosition);
        //check if the piece has become a king
        if (endPiece.isKing == false && player.pieceSymbol == 'X' && endPositionX == 7)
        {
            endPiece.IsKing();
            board[endPositionX, endPositionY] = 'V';

        }
        if (endPiece.isKing == false && player.pieceSymbol == 'O' && endPositionX == 0)
        {
            endPiece.IsKing();
            board[endPositionX, endPositionY] = 'U';
        }
    }

    public void KingMove(Player player, string startPosition, string endPosition)
    {
        int startPositionX = int.Parse(startPosition[0].ToString());
        int startPositionY = int.Parse(startPosition[1].ToString());

        int endPositionX = int.Parse(endPosition[0].ToString());
        int endPositionY = int.Parse(endPosition[1].ToString());

        Piece endPiece = player.playerPieces.FirstOrDefault(p => p.position == endPosition);

        if (endPiece != null)
        {
            if (endPiece.isKing)
            {
                if (player.pieceSymbol == 'X')
                {
                    board[endPositionX, endPositionY] = 'V';
                }
                if (player.pieceSymbol == 'O')
                {
                    board[endPositionX, endPositionY] = 'U';
                }

            }
        }

    }

    public bool HasValidMoves(Player player)
    {
        foreach (Piece piece in player.playerPieces)
        {
            string position = piece.position;
            int startX = int.Parse(position[0].ToString());
            int startY = int.Parse(position[1].ToString());

            //check possible moves
            int[] dx = { 1, -1, 1, -1, 2, -2, 2, -2 };
            int[] dy = { 1, 1, -1, -1, 2, 2, -2, -2 };

            for (int i = 0; i < dx.Length; i++)
            {
                int newX = startX + dx[i];
                int newY = startY + dy[i];
                if (newX >= 0 && newX < SIZE && newY >= 0 && newY < SIZE)
                {
                    string newPosition = GetTheArrayPosition(newX, newY);
                    if (IsValidMove(player, position, newPosition))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsGameOver(Player player)
    {
        if (player.playerPieces.Count == 0)
        {
            return true;
        }
        if (!HasValidMoves(player))
        {
            return true;
        }
        return false;
    }

}