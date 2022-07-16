namespace Logic;
public interface IUpdaterSelector<T>
{
    //permite elegir de que forma se va a desarrollar el juego 
    public void SetUpdater(Func<IDomino<T>,DominoMovement<T>> Updater);    
}
public interface IUpdater
{
    //permite desarrollar el juego
    public void Update();
}