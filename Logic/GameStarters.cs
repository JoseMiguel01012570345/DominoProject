namespace Logic;
public static class GameStarters
{
    public static ClassicDominoState ClassicDoubleSixGameStarter(IDomino<int> Game)
    {
        if(Game.State != null)
            return (ClassicDominoState)Game.State;
        int pos = 0;
        foreach(ClassicDominoPiece piece in ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]])
        {
            if(piece.Left == piece.Right && piece.Right == Game.MaxNumberOfPieces - 1)
            {
                ClassicDominoState state = new ClassicDominoState(piece);
                ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]].RemoveAt(pos);
                return state;
            }
            pos++;
        }
        return null;
    }
    public static ClassicDominoState ClassicDoubleNineStarter(IDomino<int> Game)
    {
        if(Game.State != null)
            return (ClassicDominoState)Game.State;

        DominoMovement<int> movement = Game.Players[((ClassicDomino)Game).player].Play(Game);
        for(int i = 0; i < ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]].Count; i++)
        {
            if(((ClassicDominoPiece)movement.Pieces[0]).Equals(((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]][i]))
            {
                ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]].RemoveAt(i);
                break;
            }   
        }
        return new ClassicDominoState((ClassicDominoPiece)movement.Pieces[0]);
    }
}