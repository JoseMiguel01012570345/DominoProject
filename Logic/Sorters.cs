namespace Logic;
//ordenadores de fichas
public static class Sorters
{
    public static IDominoPiece<int>[] ClassicSort(IDomino<int> Game, IDominoPiece<int>[] Pieces)
    {
        for(int i = 0; i < Pieces.Length; i++)
        {
            for(int j = i; j < Pieces.Length; j++)
            {
                if(((IComparer<IDominoPiece<int>>)Game).Compare(Pieces[i],Pieces[j]) < 0)
                {
                    IDominoPiece<int> temp = Pieces[i];
                    Pieces[i] = Pieces[j];
                    Pieces[j] = temp;
                }
            }
        }
        return Pieces;
    }
}