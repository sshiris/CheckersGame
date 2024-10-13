class Player
{
    public string name;
    public char pieceSymbol;
    public List<Piece> playerPieces;


    public Player(string name, char pieceSymbol)
    {
        this.name = name;
        this.pieceSymbol = pieceSymbol;
        this.playerPieces = new List<Piece>();
    }

    public void RemoveCapturedPiece(Piece capturedPiece)
    {
        playerPieces.Remove(capturedPiece);
    }
}