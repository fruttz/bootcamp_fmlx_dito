namespace DominoGame;

public class DominoBoard : IDominoBoard
{
    public List<IDominoCard> MainDeck {get; set;}
    public LinkedList<IDominoCard> BoardDeck {get; set;} 

    public DominoBoard(List<IDominoCard> main, LinkedList<IDominoCard> board){
        this.MainDeck = main;
        this.BoardDeck = board;
    }
}

