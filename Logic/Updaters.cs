namespace Logic;
public static class Updaters
{
    public static DominoMovement<int> ClassicUpdate(IDomino<int> Game)
    {
        if(!Game.Started)
            throw new InvalidOperationException("EL juego no se ha inicializado");
        if(Game.GameOver())
        {
            return null;
        }
        if(Game.State == null)
        {
            Game.FirstMovement();
            return new DominoMovement<int>(Game.State.Initials,new[] { 0 },Game.CurrentPlayers[0]);
        }
        DominoMovement<int> movement = Game.CurrentPlayers[0].Play(Game);
        if(movement.Pieces != null)
        {
            ClassicDominoPiece piecePlayed = (ClassicDominoPiece)movement.Pieces[0];
            try
            {
                Game.State.AddPiece(movement.Pieces[0],movement.Tops[0],Game.Controler);
                for(int i = 0; i < ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]].Count; i++)
                {
                    if(((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]][i].Equals(piecePlayed))
                    {
                        ((ClassicDomino)Game).PiecesByPlayer[Game.CurrentPlayers[0]].RemoveAt(i);
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                throw new InvalidOperationException("Ha ocurrido una violacion de las reglas");
            }
        }
        return movement;
    }
}