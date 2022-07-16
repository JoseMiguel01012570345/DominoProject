using System.IO.Pipes;
namespace Logic;
public static class Starters
{
    public static int ClassicDoubleSixStarter(IDomino<int> Game)
    {
        try
        {
            int result = 0;
            ((ClassicDomino)Game).SortPieces();
            for(int i = 0; i < Game.Players.Length; i++)
            {
                bool found = false;
                foreach(ClassicDominoPiece piece in ((ClassicDomino)Game).PiecesByPlayer[Game.Players[i]])
                {
                    if(piece.Left == piece.Right && piece.Right == Game.MaxNumberOfPieces - 1)
                    {
                        found = true;
                        result = i;
                        break;
                    }
                }
                if(found)
                    break;
            }
            return result;
        }
        catch(Exception e)
        {
            return -1;
        }
    }
    public static int ClassicDoubleNineStarter(IDomino<int> Game)
    {
        ((ClassicDomino)Game).SortPieces();
        int[] choices = new int[Game.Players.Length];
        Random random = new Random();
        for(int i = 0; i < choices.Length; i++)
            choices[i] = random.Next((Game.MaxNumberOfPieces - 1) * 2);
        int player = 0;
        for(int i = 0; i < choices.Length; i++)
            if(choices[i] > choices[player])
                player = i;
        return player;
    }
}