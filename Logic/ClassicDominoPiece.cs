namespace Logic;
//ficha clasica de domino de dos lados con puntos en ellos
public class ClassicDominoPiece : IDominoPiece<int>
{
    //ficha que enlazo esta al juego
    ClassicDominoPiece? father { get; set; }
    //fichas con las que se encuentra enlazada
    ClassicDominoPiece[] piecesLinked { get; set; }
    //jugador que coloco esta ficha
    IDominoPlayer<int>? player { get; set; }
    //valores de los extremos de la ficha
    int[] values { get; set; }
    static string description = "Fichas clasicas del domino de dos lados con números en los extremos formados por puntos.";
    public ClassicDominoPiece(int Left, int Right)
    {
        values = new[] { Left, Right };
        piecesLinked = new ClassicDominoPiece[2];
    }
    public static string Description
    {
        get{ return description; }
    }
    //devuelve la ficha que enlazo esta al juego
    public IDominoPiece<int> Father
    {
        get 
        {
            if(father == null)
                return null;
            ClassicDominoPiece result = new ClassicDominoPiece(father.Left,father.Right);
            result.SetFather(father.Father);
            return result;
        }
    }
    //devuelve los valores de los extremos de la ficha
    public int[] Values
    {
        get{ return new[] { Left, Right}; }
    }
    //devuelve los valores de los extremos libres de la ficha por donde se puede enlazar otra
    public int[] Tops
    {
        get
        {
            if(piecesLinked[0] == null && piecesLinked[1] == null)
                return new[] { Values[0], Values[1] };
            if(piecesLinked[0] == null)
                return new[] { Values[0] };
            if(piecesLinked[1] == null)
                return new[] { Values[1] };
            return new int[0];
        }
    }
    //devuelve las fichas a las que se encuentra enlazada a excepcion del padre de esta ficha
    public IDominoPiece<int>[] PiecesLinked
    {
        get
        {
            List<ClassicDominoPiece> result = new List<ClassicDominoPiece>();
            foreach(var piece in piecesLinked)
            {
                if(piece != null && piece.Equals(father))
                    continue;
                if(piece != null)
                {
                    result.Add(piece);
                }
            }
            return result.ToArray();
        }
    }
    //devuelve el jugador que coloco esta ficha
    public IDominoPlayer<int> Player
    {
        get { return player; }
    }   
    //valor izquierdo de la ficha
    public int Left
    {
        get { return values[0]; }
    }
    // valor derecho de la ficha
    public int Right
    {
        get { return values[1]; }
    }
    //ficha enlazada por la izquierda a esta ficha
    public ClassicDominoPiece LeftPiece
    {
        get
        {
            if(piecesLinked[0] == null)
                return null;
            ClassicDominoPiece result = new ClassicDominoPiece(piecesLinked[0].Left,piecesLinked[0].Right);
            result.SetFather(piecesLinked[0].Father);
            return result;
        }
    }
    //ficha enlazad por la derecha a esta ficha
    public ClassicDominoPiece RightPiece
    {
        get
        {
            if(piecesLinked[1] == null)
                return null;
            ClassicDominoPiece result = new ClassicDominoPiece(piecesLinked[1].Left,piecesLinked[1].Right);
            result.SetFather(piecesLinked[1].Father);
            return result;
        }
    }
    //metodo que especifica que ficha enlazo esta al juego
    public void SetFather(IDominoPiece<int> father)
    {
        try
        {
            this.father = (ClassicDominoPiece)father;
        }
        catch(Exception e)
        {
            throw new ArgumentOutOfRangeException("Las fichas no son del mismo tipo");
        }
    }
    //metodo que nos dice si un valor dado es compatible con alguno de los valores extremos de la ficha
    //segun un criterio dado
    public bool Contains(int Value, Func<int,int,bool> Controler)
    {
        return Controler(Left,Value) || Controler(Right, Value);
    }
    //metodo que enlaza esta ficha a otra aplicando el criterio dado
    //si el parametro "father" es true entonces esta ficha enlazo la ficha dada al juego
    public void LinkTo(IDominoPiece<int> other, int top, Func<int,int,bool> Controler, bool father = false)
    {
        if(father)
            other.SetFather(this);
        //si es compatible el valor en la posicion con el valor pasado y no hay fichas enlazadas por ahi
        if(Controler(Values[0],top) && piecesLinked[0] == null)
            piecesLinked[0] = (ClassicDominoPiece)other;
        else if(Controler(Values[1],top) && piecesLinked[1] == null)
            piecesLinked[1] = (ClassicDominoPiece)other;
        else
            throw new InvalidOperationException("Las fichas no son enlazables");
    }
    //metodo que especifica que jugador coloco esta ficha
    public void SetOwner(IDominoPlayer<int> player)
    {
        this.player = player;
    }
    //metodo que nos dice el valor del extremo por el que se encuentra enlazada la ficha dada
    public int Top(IDominoPiece<int> piece)
    {
        if(piecesLinked[0] != null && piecesLinked[0].Equals(piece))
            return Values[0];
        if(piecesLinked[1] != null && piecesLinked[1].Equals(piece))
            return Values[1];
        throw new ArgumentOutOfRangeException("La ficha no esta enlazada a esta ficha");
    }
    
    public override string ToString()
    {
        return "[" + Left + "|" + Right + "]";
    }
    
    public override bool Equals(object? other)
    {
        try
        {
            //dos fichas son iguales si y solo si contienen los mismos valores y fueron enlazadas al juego por la misma ficha
            if(((ClassicDominoPiece)other).Contains(Left, (a,b) => { return a == b;}) && ((ClassicDominoPiece)other).Contains(Right, (a,b) => { return a == b;}))
            {
                if(((ClassicDominoPiece)other).Father != null)
                    return ((ClassicDominoPiece)other).Father.Equals(father);
                return father == null;
            }
            return false;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}