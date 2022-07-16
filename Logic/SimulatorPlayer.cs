using System.Reflection.Metadata;
namespace Logic;
public class Node<T>
{
    public T TopLeft { get; private set; }
    public T TopRight { get; private set; }
    public Dictionary<IDominoPlayer<T>,List<IDominoPiece<T>>> PiecesByPlayer { get; private set; }
    public IDominoPlayer<T> Player { get; private set; }
    public Func<T,T,bool> Controler { get; private set; }
    public IDominoPiece<T> Piece { get; private set; }
    public Node(T TopLeft, T TopRight, Dictionary<IDominoPlayer<T>,List<IDominoPiece<T>>> PiecesByPlayer,Func<T,T,bool> Controler, IDominoPlayer<T> Player, IDominoPiece<T> Piece)
    {
        this.TopLeft = TopLeft;
        this.TopRight = TopRight;
        this.PiecesByPlayer = PiecesByPlayer;
        this.Controler = Controler;
        this.Player = Player;
        this.Piece = Piece;
    }
}
public class ClassicDominoTree<T>
{
    public Node<T> Value { get; set; }
    public ClassicDominoTree<T>[] Childs { get; set; }
    IDominoPlayer<T> Player { get; set; }
    bool Growed { get; set; }
    public int Wins
    {
        get
        {
            bool IsLeaf = true;
            foreach(var child in Childs)
                if(child != null)
                {
                    IsLeaf = false;
                    break;
                }
            if(IsLeaf)
            {
                foreach(var piece in Value.PiecesByPlayer[Player])
                {
                    if(piece.Contains(Value.TopLeft,Value.Controler) || piece.Contains(Value.TopRight,Value.Controler))
                        return 1;
                }
                return 0;
            }
            else
            {
                int wins = 0;
                foreach(var child in Childs)
                    if(child != null)
                        wins += child.Wins;
                return wins;
            }
        }
    }
    public ClassicDominoTree(Node<T> Value,IDominoPlayer<T> Player)
    {
        this.Value = Value;
        int count = 0;
        this.Player = Player;
        foreach(var piece in Value.PiecesByPlayer[Value.Player])
            if(piece.Contains(Value.TopLeft,Value.Controler) || piece.Contains(Value.TopRight,Value.Controler))
                count++;
        Childs = new ClassicDominoTree<T>[count];
        Growed = false;
    }
    public void Grow(IDominoPlayer<T> player)
    {
        if(!Growed)
        {
            Growed = true;
            List<IDominoPiece<T>> ValidsPieces = new List<IDominoPiece<T>>();
            foreach(var piece in Value.PiecesByPlayer[Value.Player])
            {
                if(piece.Contains(Value.TopLeft,Value.Controler) || piece.Contains(Value.TopRight,Value.Controler))
                    ValidsPieces.Add(piece);
            }
            for(int i = 0; i < Childs.Length; i++)
            {
                if(ValidsPieces.Count == 0)
                    return;
                IDominoPiece<T> pieceToPlay = ValidsPieces[i];
                Dictionary<IDominoPlayer<T>,List<IDominoPiece<T>>> piecesByPlayer = new Dictionary<IDominoPlayer<T>, List<IDominoPiece<T>>>();
                foreach(var p in Value.PiecesByPlayer.Keys)
                {
                    piecesByPlayer[p] = new List<IDominoPiece<T>>();
                    foreach(var piece in Value.PiecesByPlayer[p])
                    {
                        if(!pieceToPlay.Equals(piece))
                            piecesByPlayer[p].Add(piece);
                    }
                }
                T newtop = default(T);
                if(pieceToPlay.Contains(Value.TopLeft,Value.Controler))
                {
                    foreach(var top in pieceToPlay.Tops)
                    {
                        if(!Value.Controler(top,Value.TopLeft))
                        {
                            newtop = top;
                            Node<T> node = new Node<T>(newtop,Value.TopRight,piecesByPlayer,Value.Controler,player,pieceToPlay);
                            Childs[i] = new ClassicDominoTree<T>(node,Player);
                            break;
                        }
                    }
                }
                else
                {
                    foreach(var top in pieceToPlay.Tops)
                    {
                        if(!Value.Controler(top,Value.TopRight))
                        {
                            newtop = top;
                            Node<T> node = new Node<T>(Value.TopLeft,newtop,piecesByPlayer,Value.Controler,player,pieceToPlay);
                            Childs[i] = new ClassicDominoTree<T>(node,Player);
                        }
                    }
                }
            }
        }
        else
        {
            foreach(var child in Childs)
            {
                child.Grow(player);
            }
        }
    }
}
public static class PlayerSimulator
{
    static ClassicDominoTree<int> PieceToPlay(ClassicDominoTree<int> Tree,IDomino<int> Game,IDominoPlayer<int> Player)
    {
        Queue<ClassicDominoTree<int>> Leafs = new Queue<ClassicDominoTree<int>>();
        int player = 0;
        for(int j = 0; j < Game.Players.Length; j++)
            if(Game.Players[j].Equals(Player))
                {
                    player = j;
                    break;
                }
        Leafs.Enqueue(Tree);
        for(int i = 0; i < Game.Players.Length; i++)
        {
            player++;
            player %= Game.Players.Length;
            List<ClassicDominoTree<int>> states = new List<ClassicDominoTree<int>>();
            while(Leafs.Count > 0)
            {
                if(Leafs.Peek() == null)
                    Leafs.Dequeue();
                else
                {
                    Leafs.Peek().Grow(Game.Players[player]);
                    states.Add(Leafs.Dequeue());
                }
            }
            foreach(var tree in states)
            {
                ClassicDominoTree<int> state = tree;
                foreach(var child in state.Childs)
                {
                    Leafs.Enqueue(child);
                }
            }
        }
        ClassicDominoTree<int> pieceToPlay = null;
        if(Tree.Childs.Length > 0)
            pieceToPlay = Tree.Childs[0];
        foreach(var node in Tree.Childs)
        {
            if(pieceToPlay == null && node != null)
                pieceToPlay = node;
            else if(node != null)
                if(node.Wins > pieceToPlay.Wins)
                    pieceToPlay = node;
        }
        return pieceToPlay;
    }
    public static DominoMovement<int> Simulator(IDomino<int> Game,IDominoPlayer<int> Player)
    {
        if(Game.State == null)
            return Strategies.ClassicDominoThrowFat(Game,Player);
        Queue<ClassicDominoTree<int>> Leafs = new Queue<ClassicDominoTree<int>>();
        Node<int> LeftRoot = new Node<int>(Game.State.Tops[0],Game.State.Tops[1],((ClassicDomino)Game).PiecesByPlayer,Game.Controler,Player,Game.State.PiecesTops[0]);
        Node<int> RightRoot = new Node<int>(Game.State.Tops[0],Game.State.Tops[1],((ClassicDomino)Game).PiecesByPlayer,Game.Controler,Player,Game.State.PiecesTops[1]);
        ClassicDominoTree<int> LeftTree = new ClassicDominoTree<int>(LeftRoot,Player);
        ClassicDominoTree<int> RightTree = new ClassicDominoTree<int>(RightRoot,Player);
        ClassicDominoTree<int> pieceToPlay = PieceToPlay(LeftTree,Game,Player);
        if(pieceToPlay == null)
        {
            ClassicDominoTree<int> a = PieceToPlay(RightTree,Game,Player);
            if(a == null)
                return Strategies.ClassicDominoThrowFat(Game,Player);
            pieceToPlay = a;
        }
        ClassicDominoTree<int> pieceToPlay_other = PieceToPlay(RightTree,Game,Player);
        if(pieceToPlay_other != null && pieceToPlay_other.Wins > pieceToPlay.Wins)
            pieceToPlay = pieceToPlay_other;
        int topToPlay = 0;
        foreach(var top in Game.State.Tops)
            if(pieceToPlay.Value.Piece.Contains(top,Game.Controler))
            {
                topToPlay = top;
                break;
            }
        return new DominoMovement<int>(new[] { pieceToPlay.Value.Piece }, new[] { topToPlay }, Player);
    }
}