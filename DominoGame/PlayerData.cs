namespace DominoGame;

public class PlayerData
{
    public List<IDominoCard> PlayerDeck {get; internal set;}
    public int Win {get; internal set;}
    public int Point {get; internal set;}
    public bool AlreadyPutCardThatTurn {get; internal set;}

    public int AccumulateCard(){
        int value;
        foreach (IDominoCard card in PlayerDeck){
            value += card.CardValue();
        }
        return value;
    }
}
