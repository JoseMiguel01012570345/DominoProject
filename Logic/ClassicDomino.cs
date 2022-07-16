namespace Logic;
//Domino con fichas clasicas de dos lados con valores numericos
//
//En esta modalidad se reparte cierta cantidad de fichas por jugador y mientras mas puntos tenga la ficha
//mayor es su valor en el juego a la hora de contar
public class ClassicDomino:Domino<int>,IDominoPiecesSorter<int>,IDominoSorterPieces<int>
{
    protected Func<IDomino<int>,IDominoPiece<int>[],IDominoPiece<int>[]> Sorter { get; set; }
    public Func<IDominoPiece<int>,IDominoPiece<int>,int> Comparer { get; protected set; }
    protected Action<IDomino<int>> SorterPieces { get; set; }
    protected Dictionary<IDominoPlayer<int>,List<IDominoPiece<int>>> piecesByPlayer { get; set; }
    public bool Sorted { get; protected set; }
    //cantidad de pases consecutivos que se han dado
    public int Jumps { get; protected set; }
    public ClassicDomino(int MaxNumberOfPieces,params IDominoPlayer<int>[] players)
    {
        this.MaxNumberOfPieces = MaxNumberOfPieces;
        this.players = players;
        started = false;
        gameOver = false;
        player = 0;
        Jumps = 0;
        PlayersPlaying = new bool[players.Length];
        JumpsByPlayer = new Dictionary<IDominoPlayer<int>, int>();
        piecesByPlayer = new Dictionary<IDominoPlayer<int>, List<IDominoPiece<int>>>();
        Pieces = new List<IDominoPiece<int>>();
        for(int i = 0; i < MaxNumberOfPieces; i++)
        {
            for(int j = i; j < MaxNumberOfPieces; j++)
            {
                Pieces.Add(new ClassicDominoPiece(i, j));
            }
        }
    }
    public Dictionary<IDominoPlayer<int>,List<IDominoPiece<int>>> PiecesByPlayer
    {
        get{ return piecesByPlayer; }
    }
    public override void SetPlayers(params IDominoPlayer<int>[] players)
    {
        base.SetPlayers(players);
        foreach(var player in players)
            JumpsByPlayer[player] = 0;
    }
    public virtual void SetSorterPieces(Action<IDomino<int>> SorterPieces)
    {
        this.SorterPieces = SorterPieces;
    }
    public virtual void SortPieces()
    {
        SorterPieces(this);
        Sorted = true;
    }
    public virtual void SetSorter(Func<IDomino<int>,IDominoPiece<int>[],IDominoPiece<int>[]> Sorter)
    {
        this.Sorter = Sorter;
    }
    public virtual void SetComparer(Func<IDominoPiece<int>,IDominoPiece<int>,int> Comparer)
    {
        this.Comparer = Comparer;
    }
    public IDominoPiece<int>[] Sort(IDominoPiece<int>[] Pieces)
    {
        return Sorter(this,Pieces);
    }
    public int Compare(IDominoPiece<int> piece1, IDominoPiece<int> piece2)
    {
        return Comparer(piece1, piece2);
    }
    public override void Update()
    {
        if(gameOver)
            return;
        if(!started)
            throw new InvalidOperationException("No se ha inicializado el juego");
        lastMovement = Updater(this);
        if(lastMovement.Pieces != null)
            Jumps = 0;
        else
        {
            Jumps++;
            JumpsByPlayer[currentPlayer]++;
        }
        gameOver = GameOverer(this);
    }
    public override bool GameOver()
    {
        return gameOver;
    }
    public override void NextPlayer()
    {
        player = Judge(this);
        currentPlayer = players[player];
        if(state != null)
            state.ChangePlayer();
    }
    public override void FirstMovement()
    {
        state = GameStarter(this);
        lastMovement = new DominoMovement<int>(state.Initials,new[] { 0 },state.Initials[0].Player);
    }
    public override IDominoPiece<int>[] ShowPieces(params IDominoPlayer<int>[] players)
    {
        List<IDominoPiece<int>> aux = new List<IDominoPiece<int>>();
        foreach(var player in players)
            foreach(var piece in piecesByPlayer[player])
                aux.Add(piece);
        IDominoPiece<int>[] result = new IDominoPiece<int>[aux.Count];
        Array.Copy(aux.ToArray(),result,result.Length);
        return Sort(result);
    }
    public override void Start()
    {
        if(GameOverer == null)
            throw new InvalidOperationException("No se ha determinado una forma de terminar el juego");
        if(GameStarter == null)
            throw new InvalidOperationException("No se ha determinado una forma de comenzar el juego");
        if(Judge == null)
            throw new InvalidOperationException("No se ha determinado una forma de computar los turnos de los jugadores");
        if(Updater == null)
            throw new InvalidOperationException("No se ha determinado una forma de desarrollar e juego");
        if(Starter == null)
            throw new InvalidOperationException("No se ha determinado una forma de inicializar el juego");
        if(Sorter == null)
            throw new InvalidOperationException("No se ha determinado una forma de ordenar las fichas");
        if(Controler == null)
            throw new InvalidOperationException("No se ha determinado una forma de determinar la compatibilidad de  las fichas");
        if(Comparer == null)
            throw new InvalidOperationException("No se ha determinado una forma de comparar las fichas");
        if(WinnerComputer == null)
            throw new InvalidOperationException("No se ha determinado una forma de computar los ganadores del juego");
        if(SorterPieces == null)
            throw new InvalidOperationException("No se ha determinado una forma de repartir las fichas");
        started = true;
        player = Starter(this);
        currentPlayer = players[player];
    }
}