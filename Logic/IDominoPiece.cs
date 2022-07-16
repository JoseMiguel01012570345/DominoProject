namespace Logic;
//caracteristicas de toda ficha de domino
public interface IDominoPiece<T>
{
    public T[] Values{get;}
    //todas tienen una ficha que las unio al juego
    public IDominoPiece<T> Father{get;}
    //todas tienen extremos por donde se pueden enlazar otras fichas
    public T[] Tops{get;}
    //todas pueden o no estar enlazadas a otras fichas
    public IDominoPiece<T>[] PiecesLinked{get;}
    //toda ficha es colocada por un jugador
    public IDominoPlayer<T> Player{get;}
    //necesitamos saber si entre los extremos de la ficha se halla un valor dado
    public bool Contains(T Value,Func<T,T,bool> Controler);
    //sobrecarga que permiteaplicar un criterio para decidir si dos fichas son enlazables
    public void LinkTo(IDominoPiece<T> other,T top,Func<T,T,bool> controler,bool father=false);
    //debe saber quien la enlazo al juego
    public void SetFather(IDominoPiece<T> father);
    //para guardar quien la coloco
    public void SetOwner(IDominoPlayer<T> player);
    //valor del extremo enlazado a una ficha dada
    public T Top(IDominoPiece<T> Piece);
    public static string? Description{get;}
}
