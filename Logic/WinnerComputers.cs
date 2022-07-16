namespace Logic;
public static class WinnerComputers
{
    public static IDominoPlayer<int>[] WinnerComputerPieces(IDomino<int> domino)//gana el que menos fichas tiene
    {
        int smallest = int.MaxValue;
        IDominoPlayer<int>[] winner = new IDominoPlayer<int>[1];
        int j = 0;
        foreach (var i in ((ISorterPieces<int>)domino).PiecesByPlayer)
        {
            if (smallest > i.Value.Count && ((Domino<int>)domino).PlayersPlaying![j] == false)
            {
                smallest = i.Value.Count;
                winner[0] = i.Key;
            }
            j++;
        }
        return winner;
    }
    public static IDominoPlayer<int>[] WinnerComputer(IDomino<int> domino)//gana quien menos turnos se ha pasado
    {
        IDominoPlayer<int>[] winner = new IDominoPlayer<int>[1];
        winner[0] = domino.Players[Min((Domino<int>)domino)];

        return winner;
    }
    static int Min(Domino<int> domino)
    {
        int smallest = int.MaxValue;
        for (int i = 0; i < domino.Players.Length; i++)
        {
            if (smallest > domino.JumpsByPlayer[domino.Players[i]] && domino.PlayersPlaying![i] == false) smallest = domino.JumpsByPlayer[domino.Players[i]];
        }
        return smallest;
    }
    public static IDominoPlayer<int>[] ClassicWinnerComputer(IDomino<int> Game)
    {
        Dictionary<IDominoPlayer<int>,int> Points = new Dictionary<IDominoPlayer<int>, int>();
        foreach(var player in Game.Players)
        {
            if(((ClassicDomino)Game).PiecesByPlayer[player].Count == 0)
                return new[] { player };
            Points[player] = 0;
            foreach(var piece in ((ClassicDomino)Game).PiecesByPlayer[player])
            {
                foreach(var value in piece.Values)
                    Points[player] += value;
            }
        }
        IDominoPlayer<int> winner = Game.Players[0];
        foreach(var player in Game.Players)
        {
            if(Points[player] < Points[winner])
                winner = player;
        }
        List<IDominoPlayer<int>> winners = new List<IDominoPlayer<int>>();
        winners.Add(winner); 
        foreach(var player in Game.Players)
            if(!player.Equals(winner) && Points[player] == Points[winner])
                winners.Add(player);
        return winners.ToArray();
    }
}