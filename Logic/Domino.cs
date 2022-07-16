namespace Logic;
//clase base de todos los dominos
public abstract class Domino<T>:IDomino<T>,IGameModesSelector<T>
{
    protected Func<IDomino<T>,IDominoState<T>>? GameStarter { get; set; }
    protected Func<IDomino<T>,bool>? GameOverer { get; set; }
    protected Func<IDomino<T>,int>? Judge { get; set; }
    protected Func<IDomino<T>,DominoMovement<T>>? Updater { get; set; }
    protected Func<IDomino<T>,int>? Starter { get; set; }
    protected Func<IDomino<T>,IDominoPlayer<T>[]>? WinnerComputer { get; set; }
    protected IDominoPlayer<T>[]? winners { get; set; }
    protected IDominoPlayer<T>? currentPlayer { get; set; }
    protected IDominoPlayer<T>[]? players { get; set; }
    protected IDominoState<T>? state { get; set; }
    protected DominoMovement<T>? lastMovement { get; set; }
    public List<IDominoPiece<T>> Pieces { get; protected set; }
    public Func<T,T,bool> Controler { get; protected set; }
    public Dictionary<IDominoPlayer<T>,int> JumpsByPlayer { get; protected set; }
    protected bool started { get; set; }
    public bool[]? PlayersPlaying { get; protected set; }
    public int MaxNumberOfPieces { get; set; }
    protected bool gameOver { get; set; }
    public int player { get; protected set; }
    public IDominoPlayer<T>[] Winners
    {
        get
        {
            if (gameOver)
            {
                IDominoPlayer<T>[]? winners = WinnerComputer(this);
                IDominoPlayer<T>[] result = new IDominoPlayer<T>[winners.Length];
                Array.Copy(winners, result, result.Length);
                return result;
            }
            else throw new Exception("El juego no se ha acabado");
        }
    }
    public IDominoPlayer<T>[] CurrentPlayers
    {
        get{ return GetCurrentPlayers(); }
    }
    public IDominoPlayer<T>[] Players
    {
        get{ return GetPlayers(); }
    }
    public IDominoState<T> State
    {
        get{ return state; }
    }
    public DominoMovement<T> LastMovement
    {
        get{ return new DominoMovement<T>(lastMovement.Pieces,lastMovement.Tops,lastMovement.Player); }
    }
    public bool Started
    {
        get{ return started; }
    }
    protected virtual IDominoPlayer<T>[] GetCurrentPlayers()
    {
        return new[] { currentPlayer };
    }
    protected virtual IDominoPlayer<T>[] GetPlayers()
    {
        IDominoPlayer<T>[] result = new IDominoPlayer<T>[players.Length];
        Array.Copy(players,result,result.Length);
        return result;
    }
    public virtual void SetPlayers(params IDominoPlayer<T>[] players)
    {
        this.players = players;
        PlayersPlaying = new bool[players.Length];
    }
    public virtual void SetGameStarter(Func<IDomino<T>,IDominoState<T>> GameStarter)
    {
        this.GameStarter = GameStarter;
    }
    public virtual void SetGameOver(Func<IDomino<T>,bool> GameOverer)
    {
        this.GameOverer = GameOverer;
    }
    public virtual void SetJudger(Func<IDomino<T>,int> Judge)
    {
        this.Judge = Judge;
    }
    public virtual void SetUpdater(Func<IDomino<T>,DominoMovement<T>> Updater)
    {
        this.Updater = Updater;
    }
    public virtual void SetStarter(Func<IDomino<T>, int> Starter)
    {
        this.Starter = Starter;
    }
    public virtual void SetWinnerComputer(Func<IDomino<T>,IDominoPlayer<T>[]> WinnerComputer)
    {
        this.WinnerComputer = WinnerComputer;
    }
    public virtual void SetControler(Func<T,T,bool> Controler)
    {
        this.Controler = Controler;
    }
    public abstract void FirstMovement();
    public abstract bool GameOver();
    public abstract void NextPlayer();
    public abstract void Update();
    public abstract void Start();
    public abstract IDominoPiece<T>[] ShowPieces(params IDominoPlayer<T>[] Players);  
}