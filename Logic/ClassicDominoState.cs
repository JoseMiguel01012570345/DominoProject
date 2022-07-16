namespace Logic;
public class ClassicDominoState:IDominoState<int>
{
    ClassicDominoPiece initial { get; set; }
    List<ClassicDominoPiece> piecesPlayeds { get; set; }
    ClassicDominoPiece topLeftPiece {get; set; }
    ClassicDominoPiece topRightPiece { get; set; }
    public int TopLeft {get; private set; }
    public int TopRight { get; private set; }
    int count { get; set; }
    public ClassicDominoState(ClassicDominoPiece Initial)
    {
        initial = Initial;
        count = 1;
        piecesPlayeds = new List<ClassicDominoPiece>();
        piecesPlayeds.Add(initial);
        TopLeft = initial.Left;
        TopRight = initial.Right;
        topRightPiece = initial;
        topLeftPiece = initial;
    }
    public int Count
    {
        get{ return count; }
    }
    public IDominoPiece<int>[] Initials
    {
        get
        {
            ClassicDominoPiece result = new ClassicDominoPiece(initial.Left,initial.Right);
            return new[] { result };
        }
    }
    public IDominoPiece<int>[] PiecesPlayed
    {
        get
        {
            ClassicDominoPiece[] result = new ClassicDominoPiece[piecesPlayeds.Count];
            Array.Copy(piecesPlayeds.ToArray(),result,result.Length);
            return result;
        }
    }
    public IDominoPiece<int>[] PiecesPlayedTillNow
    {
        get
        {
            ClassicDominoPiece[] result = new ClassicDominoPiece[count];
            int pos = 0;
            result[pos] = initial;
            pos++;
            ClassicDominoPiece leftPiece = initial.LeftPiece;
            ClassicDominoPiece rightPiece = initial.RightPiece;
            while(leftPiece != null)
            {
                result[pos] = leftPiece;
                pos++;
                try
                {
                    leftPiece = (ClassicDominoPiece)leftPiece.PiecesLinked[0];
                }
                catch(Exception e)
                {
                    leftPiece = null;
                }
            }
            while(rightPiece != null)
            {
                result[pos] = rightPiece;
                pos++;
                try
                {
                    rightPiece = (ClassicDominoPiece)rightPiece.PiecesLinked[0];
                }
                catch(Exception e)
                {
                    rightPiece = null;
                }
            }
            List<ClassicDominoPiece> Result = new List<ClassicDominoPiece>();
            foreach(var piece in result)
                if(piece != null)
                    Result.Add(piece);
            return Result.ToArray();
        }
    }
    public IDominoPiece<int>[] PiecesTops
    {
        get
        {
            ClassicDominoPiece left = new ClassicDominoPiece(topLeftPiece.Left,topLeftPiece.Right);
            left.SetFather(topLeftPiece.Father);
            ClassicDominoPiece right = new ClassicDominoPiece(topRightPiece.Left,topRightPiece.Right);
            right.SetFather(topRightPiece.Father);
            return new[] { left, right };
        }
    }
    public int[] Tops
    {
        get { return new[] { TopLeft, TopRight}; }
    }
    public void AddPiece(IDominoPiece<int> Piece, int Top, Func<int,int,bool> Controler)
    {
        //se calcula el extremo de la ficha que se quiere enlazar
        foreach(var top in Piece.Values)
            if(Controler(top,Top))
            {
                Top = top;
                break;
            }
        //se comprueba que las fichas sean enlazables
        if(!Controler(Top,TopLeft) && !Controler(Top,TopRight))
            throw new InvalidOperationException("Jugada invalida");
        if(Controler(Top,TopLeft))
        {
            //se enlaza la ficha de la izquierda con la nueva ficha con el extremo dado
            topLeftPiece.LinkTo(Piece,Top,Controler,true);
            //se enlaza la nueva ficha con el valor del extremo correspondiente
            Piece.LinkTo(topLeftPiece,TopLeft,Controler);
            piecesPlayeds.Add((ClassicDominoPiece)Piece);
            topLeftPiece = (ClassicDominoPiece)Piece;
            TopLeft = Piece.Tops[0];
            count++;
        }
        else if(Controler(Top,TopRight))
        {
            topRightPiece.LinkTo(Piece,Top,Controler,true);
            Piece.LinkTo(topRightPiece,TopRight,Controler);
            piecesPlayeds.Add((ClassicDominoPiece)Piece);
            topRightPiece = (ClassicDominoPiece)Piece;
            TopRight = Piece.Tops[0];
            count++;
        }
    }
    public void ChangePlayer()
    {
        piecesPlayeds.Clear();
    }
}