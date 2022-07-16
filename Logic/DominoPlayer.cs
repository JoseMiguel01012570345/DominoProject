namespace Logic;
public class DominoPlayer<T>:IDominoPlayer<T>
{
    Func<IDomino<T>,IDominoPlayer<T>,DominoMovement<T>> Player { get; set; }
    Func<IDomino<T>,IDominoPlayer<T>,IDominoPiece<T>> Selecter{ get; set; }
    public Random random { get; protected set; }
    public bool PlayMode { get; protected set; }
    public bool SelectMode { get; protected set; }
    public string Name{ get; protected set; }
    public DominoPlayer(string Name)
    {
        random = new Random();
        this.Name = Name;
        PlayMode = false;
        SelectMode = false;
    }
    public virtual void SetPlayMode(Func<IDomino<T>,IDominoPlayer<T>,DominoMovement<T>> Player)
    {
        this.Player = Player;
        if(Player != null)
            PlayMode = true;
        else
            PlayMode = false;
    }
    public virtual void SetSelecter(Func<IDomino<T>,IDominoPlayer<T>,IDominoPiece<T>> Selecter)
    {
        this.Selecter = Selecter;
        if(Selecter != null)
            SelectMode = true;
        else
            SelectMode = false;
    }
    public virtual DominoMovement<T> Play(IDomino<T> Game)
    {
        if(!PlayMode)
        {
            IDominoPiece<T>[] pieces = Game.ShowPieces(this);
            foreach(var piece in pieces)
                foreach(var top in Game.State.Tops)
                    if(piece.Contains(top,Game.Controler))
                        return new DominoMovement<T>(new[] { piece }, new[] { top }, this);
            return new DominoMovement<T>(null,null,this);
        }
        return Player(Game,this);
    }
    public virtual IDominoPiece<T> Select(IDomino<T> Game)
    {
        if(!SelectMode)
        {
            IDominoPiece<T>[] pieces = Game.ShowPieces(this);
            return pieces[random.Next(pieces.Length)];
        }
        return Selecter(Game,this);
    }
}