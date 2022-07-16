namespace Logic;
public interface IJudgeSelector<T>
{
    //permite elegir de que manera se computara el siguiente jugador
    public void SetJudger(Func<IDomino<T>,int> Judge);
}
public interface INextPlayerSelector
{
    //permite seleccionar quien sera el proximo jugador
    public void NextPlayer();
}