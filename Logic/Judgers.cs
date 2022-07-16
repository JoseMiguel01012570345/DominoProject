namespace Logic;
public static class Judgers
{
    static int TurnsToPlay = -1;
    static int PlayerOrder = 0;
    static int iterator = 0;
    static int CountDown = 0;
    public static int JudgerRuner(IDomino<int> domino)//juega mientras pueda
    {
        if (domino.LastMovement.Pieces != null)
        {
            return PlayerOrder;
        }
        else
        {
            if (PlayerOrder == domino.Players.Length - 1) return PlayerOrder = 0;

            return PlayerOrder++;
        }
    }
    public static int JudgerJose(IDomino<int> domino)//el int devuelto es el jugador que le toca jugar
    {
        if (TurnsToPlay == -1)
        {
            if (iterator == 0) iterator = ((Domino<int>)domino).Players.Length;
            iterator--;
            TurnsToPlay = Max((Domino<int>)domino) - domino.JumpsByPlayer[((Domino<int>)domino).Players[iterator]];
        }
        if (TurnsToPlay > -1)
            TurnsToPlay--;

        return domino.Players.ToList().IndexOf(((Domino<int>)domino).Players[iterator]);//juega seguido la diferencia entre los turnos pasados del jugador y la mayor cantidad de turnos pasados
    }
    static int Max(Domino<int> domino)
    {
        int best = 0;
        for (int i = 0; i < domino.Players.Length; i++)
        {
            if (best < domino.JumpsByPlayer[domino.Players[i]]) best = domino.JumpsByPlayer[domino.Players[i]];
        }
        return best;
    }
    public static int ClassicNextPlayer(IDomino<int> Game)
    {
        int player = ((ClassicDomino)Game).player;
        player++;
        player %= Game.Players.Length;
        return player;
    }
}