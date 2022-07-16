namespace Logic;
public static class PiecesSorters
{
    static Random random = new Random();
    public static int TotalPieces = 3;
    public static void ClassicStaticSortPieces(IDomino<int> Game)
    {
        foreach(var player in Game.Players)
        {
            ((ClassicDomino)Game).PiecesByPlayer[player] = new List<IDominoPiece<int>>();
            for(int i = 0; i < TotalPieces; i++)
            {
                int pos = random.Next(((ClassicDomino)Game).Pieces.Count);
                ((ClassicDomino)Game).Pieces[pos].SetOwner(player);
                ((ClassicDomino)Game).PiecesByPlayer[player].Add(((ClassicDomino)Game).Pieces[pos]);
                ((ClassicDomino)Game).Pieces.RemoveAt(pos);
            }
        }
    }
}