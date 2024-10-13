class Piece
{
    public string position;
    public bool isKing;

    public Piece(string position, bool isKing=false)
    {
        this.position = position;
        this.isKing = isKing;
    }

    public bool IsKing()
    {
        isKing = true;
        return isKing;
    }

    
}



