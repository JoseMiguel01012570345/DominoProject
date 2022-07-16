namespace Logic;
public static class GameOverers
{
    public static (string, int)[] parametros = { ("Cantidad de pases a perder", 2) };
    public static bool GameOvererByJump(IDomino<int> domino)
    {
        for (int i = 0; i < domino.Players.Length; i++)
        {
            if (domino.JumpsByPlayer[domino.Players[i]] == parametros[0].Item2) ((Domino<int>)domino).PlayersPlaying[i] = true;
        }
        int count = 0;
        for(int i = 0; i < domino.Players.Length; i++)
        {
            if(!((ClassicDomino)domino).PlayersPlaying[i])
                count++;
            if(count > 1)
                return false;
        }
        return true;
    }
    public static bool ClassicGameOver(IDomino<int> Game)
    {
        if(((ClassicDomino)Game).Jumps == Game.Players.Length)
            return true;
        foreach(var player in Game.Players)
            if(((ClassicDomino)Game).PiecesByPlayer[player].Count == 0)
                return true;
        return false;
    }
}