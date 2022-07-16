using Logic;

namespace Visual;
public static class WorkSpace
{
    public static ClassicDomino game = new ClassicDomino(5);
    public static void Start()
    {
        game.Start();        
    }
    public static void Update()
    {
        game.Update();
    }
    public static void Restart(int number_of_pieces)
    {
        game = new ClassicDomino(number_of_pieces);
    }
}