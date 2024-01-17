namespace DominoGame;

public interface IDominoBoard
{
    public List<IDominoCard> MainDeck {get; set;}
    public LinkedList<IDominoCard> BoardDeck {get; set;} 
}
