namespace Logic;
public static class Selecters
{
    static Random random = new Random();
    public static IDominoPiece<int> RandomSelecter(IDomino<int> Game,IDominoPlayer<int> Player)
    {
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        return Pieces[random.Next(Pieces.Length)];
    }
    public static IDominoPiece<int> ThrowFatSelecter(IDomino<int> Game, IDominoPlayer<int> Player)
    {
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        return Pieces[Pieces.Length - 1];
    }
    
}