namespace DominoGame;

public class Game
{
    private Dictionary<IPlayer, PlayerData> _players;
    private IDominoBoard _boards;
    private GameStatus gameStatus;
    private List<bool> _chainStatus;
    private int indexTurnPlayer;
    private int maxPoint;
    private int numberDistributeCards;
    public Action<IDominoCard card, CardStatus card.Status> CardStatus;

    public Game(IDominoBoard board, int maxPoint, int numberOfDistributeCard){
        this._boards = board;
        this.maxPoint = maxPoint;
        this.numberDistributeCards = numberOfDistributeCard;
    }

    

}
