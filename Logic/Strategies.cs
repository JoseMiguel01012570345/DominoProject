namespace Logic;
public class DominoTreeDevelopment<T> where T : ClassicDomino
{
    
}
public static class Strategies
{
    static Random random = new Random();
    public static DominoMovement<int> ClassicDominoRandom(IDomino<int> Game,IDominoPlayer<int> Player)
    {
        if(Game.State == null)
        {
            IDominoPiece<int>[] pieces = Game.ShowPieces(Player);
            IDominoPiece<int> piece =pieces[random.Next(pieces.Length)];

            return new DominoMovement<int>(new[] { piece }, new[] { 0 } , Player);
        }
        List<Tuple<IDominoPiece<int>,int>> ValidsPieces = new List<Tuple<IDominoPiece<int>,int>>();
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        foreach(var top in Game.State.Tops)
        {
            foreach(var piece in Pieces)
            {
                foreach(var piece_top in piece.Tops)
                {
                    if(Game.Controler(piece_top,top))
                    {
                        ValidsPieces.Add(new Tuple<IDominoPiece<int>, int>(piece,top));
                        break;
                    }
                }
            }
        }
        if(ValidsPieces.Count == 0)
            return new DominoMovement<int>(null,Game.State.Tops,Player);
        int piece_pos = random.Next(ValidsPieces.Count);
        return new DominoMovement<int>(new[]{ValidsPieces[piece_pos].Item1},new[]{ValidsPieces[piece_pos].Item2},Player);
    }
    public static DominoMovement<int> ClassicDominoThrowFat(IDomino<int> Game, IDominoPlayer<int> Player)
    {
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        if(Game.State == null)
            return new DominoMovement<int>(new[] { Pieces[0] },new[] { 0 } , Player);
        for(int i = 0; i < Pieces.Length; i++)
        {
            foreach(var top in Game.State.Tops)
            {
                if(Pieces[i].Contains(top,Game.Controler))
                {
                    IDominoPiece<int> result = Pieces[i];
                    return new DominoMovement<int>(new[] { result }, new[] { top },Player);
                }
            }
        }
        return new DominoMovement<int>(null, new[] { -1 } , Player);
    }
    public static DominoMovement<int> ClassicDoubleSixAliHeuristic(IDomino<int> Game, IDominoPlayer<int> Player)
    {
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        //si no se ha iniciado el juego
        if(Game.State == null)
        {
            //jugamos al azar
            IDominoPiece<int> piece = Pieces[random.Next(Pieces.Length)];
            return new DominoMovement<int>(new[] { piece }, new[] { 0 }, Player);
        }
        //obtenemos los extremos por donde podemos jugar
        int topLeft = Game.State.Tops[0];
        int topRight = Game.State.Tops[1];
        //obtenemos las fichas validas
        List<IDominoPiece<int>> ValidsPieces = new List<IDominoPiece<int>>();
        foreach(var piece in Pieces)
            if(piece.Contains(topLeft,Game.Controler) || piece.Contains(topRight,Game.Controler))
                ValidsPieces.Add(piece);
        //contamos cuantas fichas de cada numero de las que tenemos
        Dictionary<int,int> PiecesByTypes = new Dictionary<int, int>();
        foreach(var piece in Pieces)
        {
            if(!PiecesByTypes.Keys.Contains(((ClassicDominoPiece)piece).Left))
                PiecesByTypes[((ClassicDominoPiece)piece).Left] = 1;
            else
                PiecesByTypes[((ClassicDominoPiece)piece).Left]++;
            if(!Utils.IsDouble((ClassicDominoPiece)piece) && !PiecesByTypes.Keys.Contains(((ClassicDominoPiece)piece).Right))//comprobar que la ficha no es doble
                PiecesByTypes[((ClassicDominoPiece)piece).Right] = 1;
            else if(!Utils.IsDouble((ClassicDominoPiece)piece))
                PiecesByTypes[((ClassicDominoPiece)piece).Right]++;
        }
        //si tenemos mas de 4 fichas del mismo tipo 
        foreach(var top in PiecesByTypes.Keys)
            if(PiecesByTypes[top] > 4)
                foreach(var piece in ValidsPieces)
                    if(piece.Contains(top,(a,b) => { return a == b; }) && !Utils.IsDouble((ClassicDominoPiece)piece))
                        if(piece.Contains(topLeft,Game.Controler) && topLeft != top)
                            return new DominoMovement<int>(new[] { piece },new[] { topLeft }, Player);//ponemos el juego a las dos cabezas por el valor que nos favorece
                        else if(piece.Contains(topRight,Game.Controler) && topRight != top)
                            return new DominoMovement<int>(new[] { piece },new[] { topRight }, Player);
        //vemos que fichas se han jugado hasta el momento
        IDominoPiece<int>[] PiecesInGame = Game.State.PiecesPlayedTillNow;
        //vemos las fichas que quedan en juego con respecto a las que tenemos
        Dictionary<int,int> PiecesRemains = new Dictionary<int, int>();
        foreach(var top in PiecesByTypes.Keys)
        {
            //descontamos las fichas en mesa
            PiecesRemains[top] = Game.MaxNumberOfPieces;
            foreach(var piece in PiecesInGame)
                if(piece.Contains(top,Game.Controler))
                    PiecesRemains[top]--;
            //descontamos las que tenemos
            PiecesRemains[top] -= PiecesByTypes[top];
            //hay que descontar los dobles que esten en juego todavia
            if(!Utils.ContainsDouble(top,PiecesInGame) && !Utils.ContainsDouble(top,Pieces))
                PiecesRemains[top]--;
        }
        //jugamos las fichas para quedarnos con un lugar fijo por donde poder jugar
        foreach(var piece in ValidsPieces)
        {
            if(PiecesRemains.Keys.Contains(((ClassicDominoPiece)piece).Left))
            {
                //si no quedan mas de esas fichas
                if(PiecesRemains[((ClassicDominoPiece)piece).Left] == 0)
                {
                    if(Game.Controler(((ClassicDominoPiece)piece).Left,topLeft))
                        return new DominoMovement<int>(new[] { piece }, new[] { topLeft }, Player);
                    return new DominoMovement<int>(new[] { piece }, new[] { topRight }, Player);
                }
            }
            if(PiecesRemains[((ClassicDominoPiece)piece).Right] == 0)
            {
                if(Game.Controler(((ClassicDominoPiece)piece).Right,topLeft))
                    return new DominoMovement<int>(new[] { piece }, new[] { topLeft }, Player);
                return new DominoMovement<int>(new[] { piece }, new[] { topRight }, Player);
            }
        }
        //si podemos jugar para quedarnos fijos lo hacemos
        foreach(var piece in Game.State.PiecesTops)
        {
            //si la ficha no la puso este jugador
            if(!Equals(piece.Player, Player))
            {
                foreach(var valid_piece in ValidsPieces)
                {
                    if(PiecesRemains.Keys.Contains(((ClassicDominoPiece)valid_piece).Left))
                    {
                        //comparamos para ver si mantenemos la ventaja
                        if(PiecesRemains[((ClassicDominoPiece)valid_piece).Left] < PiecesByTypes[((ClassicDominoPiece)valid_piece).Left] - 2)
                        {
                            if(Game.Controler(((ClassicDominoPiece)valid_piece).Left,topLeft))
                                return new DominoMovement<int>(new[] { valid_piece }, new[] { topLeft }, Player);
                            return new DominoMovement<int>(new[] { valid_piece }, new[] { topRight }, Player);
                        }
                    }
                    if(PiecesRemains[((ClassicDominoPiece)valid_piece).Right] < PiecesByTypes[((ClassicDominoPiece)valid_piece).Right] - 2)
                    {
                        if(Game.Controler(((ClassicDominoPiece)valid_piece).Right,topLeft))
                            return new DominoMovement<int>(new[] { valid_piece }, new[] { topLeft }, Player);
                        return new DominoMovement<int>(new[] { valid_piece }, new[] { topRight }, Player);
                    }       
                }
            }
        }
        //escogemos las fichas que nos dejen con mas fichas del mismo tipo para poder jugar
        List<IDominoPiece<int>> GoodPieces = new List<IDominoPiece<int>>();
        foreach(var piece in ValidsPieces)
        {
            if(PiecesByTypes[((ClassicDominoPiece)piece).Left] == 1 || PiecesByTypes[((ClassicDominoPiece)piece).Left] == 1)
                continue;
            foreach(var top in PiecesByTypes.Keys)
            {
                if(PiecesRemains[top] == 1 && piece.Contains(top, (a,b) => { return a == b; }))
                {
                    if(piece.Contains(topLeft,Game.Controler))
                        return new DominoMovement<int>(new[] { piece }, new[] { topLeft }, Player);
                    return new DominoMovement<int>(new[] { piece }, new[] { topRight }, Player);
                }
                if(piece.Contains(top,(a,b) => { return a == b; }) && PiecesByTypes[top] > 1)
                {
                    //si tenemos el doble de esa ficha no la tira para evitar que que le cierren la salida de la ficha 
                    if(!Utils.IsDouble((ClassicDominoPiece)piece) && Utils.ContainsDouble(top, Pieces) && top != 0)
                        continue;
                    if(Utils.IsDouble((ClassicDominoPiece)piece))
                    {
                        if(Game.Controler(top,topLeft))
                            return new DominoMovement<int>(new[] { piece }, new[] { topLeft }, Player);
                        else
                            return new DominoMovement<int>(new[] { piece }, new[] { topRight }, Player);
                    }
                    GoodPieces.Add(piece);
                    break;
                }
            }
        }
        //si existen este tipo de fichas
        if(GoodPieces.Count > 0)
        {
            foreach(var piece in Game.State.PiecesTops)
            {
                if(!Equals(piece.Player, Player))
                {
                    foreach(var valid_piece in GoodPieces)
                    {
                        if(PiecesRemains.Keys.Contains(((ClassicDominoPiece)valid_piece).Left))
                        {
                            if(PiecesRemains[((ClassicDominoPiece)valid_piece).Left] == 0)
                            {
                                if(Game.Controler(((ClassicDominoPiece)valid_piece).Left,topLeft))
                                    return new DominoMovement<int>(new[] { valid_piece }, new[] { topLeft }, Player);
                                return new DominoMovement<int>(new[] { valid_piece }, new[] { topRight }, Player);
                            }
                        }
                        if(PiecesRemains[((ClassicDominoPiece)valid_piece).Right] == 0)
                        {
                            if(Game.Controler(((ClassicDominoPiece)valid_piece).Right,topLeft))
                                return new DominoMovement<int>(new[] { valid_piece }, new[] { topLeft }, Player);
                            return new DominoMovement<int>(new[] { valid_piece }, new[] { topRight }, Player);
                        }       
                    }
                }
            }   
        }
        foreach(var piece in ValidsPieces)
        {
            if(piece.Contains(topLeft,Game.Controler))
                return new DominoMovement<int>(new[] { piece }, new[] { topLeft }, Player);
            if(piece.Contains(topRight,Game.Controler))
                return new DominoMovement<int>(new[] { piece }, new[] { topRight }, Player);
        }
        return new DominoMovement<int>(null,new[] { -1 }, Player);
    }
    public static DominoMovement<int> CrouchedDownClassicDominoPlayer(IDomino<int> Game, IDominoPlayer<int> Player)
    {
        IDominoPiece<int>[] Pieces = Game.ShowPieces(Player);
        Dictionary<int,int> Frecuency = new Dictionary<int, int>();
        if(Game.State == null)
        {
            foreach(var piece in Pieces)
            {
                foreach(var top in piece.Tops)
                {
                    if(!Frecuency.Keys.Contains(top))
                        Frecuency[top] = 1;
                    else
                        Frecuency[top]++;
                }
            }
            int sum = 0;
            foreach(var top in Pieces[0].Tops)
                sum += Frecuency[top];
            for(int i = 0; i < Pieces.Length; i++)
            {
                int aux = 0;
                foreach(var top in Pieces[i].Tops)
                    aux += Frecuency[top];
                if(aux > sum)
                {
                    sum = aux;
                    IDominoPiece<int> temp = Pieces[i];
                    Pieces[i] = Pieces[0];
                    Pieces[0] = temp;
                }
            }
            return new DominoMovement<int>(new[] { Pieces[0] }, new[] { 0 }, Player);
        }
        List<IDominoPiece<int>> ValidsPieces = new List<IDominoPiece<int>>();
        foreach(var piece in Pieces)
        {
            foreach(var top in Game.State.Tops)
            {
                if(piece.Contains(top,Game.Controler))
                {
                    ValidsPieces.Add(piece);
                    break;
                }
            }
        }
        foreach(var piece in Pieces)
        {
            foreach(var top in piece.Tops)
            {
                if(!Frecuency.Keys.Contains(top))
                    Frecuency[top] = 1;
                else
                    Frecuency[top]++;
            }
        }
        if(ValidsPieces.Count == 0)
            return new DominoMovement<int>(null,new[] { -1 }, Player);
        int Sum = 0;
        foreach(var top in Pieces[0].Tops)
            Sum += Frecuency[top];
        for(int i = 0; i < ValidsPieces.Count; i++)
        {
            int aux = 0;
            foreach(var top in ValidsPieces[i].Tops)
                aux += Frecuency[top];
            if(aux > Sum)
            {
                Sum = aux;
                IDominoPiece<int> temp = ValidsPieces[i];
                ValidsPieces[i] = ValidsPieces[0];
                ValidsPieces[0] = temp;
            }
        }
        int topPlayed = 0;
        foreach(var top in Game.State.Tops)
            if(ValidsPieces[0].Contains(top,Game.Controler))
            {
                topPlayed = top;
                break;
            }
        return new DominoMovement<int>(new[] { ValidsPieces[0] }, new[] { topPlayed }, Player);
    }
}