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
	
	public IEnumerable<IDominoCard> GetPlayerCard(IPlayer player){
		return _players[player].PlayerDeck;
	}
	
	public int GetPlayerPoint(IPlayer player) {
		return _players[player].Point;
	}
	
	public bool GetAlreadyPutCard(IPlayer player) {
		return _players[player].AlreadyPutCardThatTurn;
	}
	
	public IEnumerable<IPlayer> GetPlayers() {
		return _players.Keys
	}
	
	public IEnumerable<IDominoCard> GetMainDeck() {
		return _boards.MainDeck;
	}
	
	public IEnumerable<IDominoCard> GetBoardDeck() {
		return _boards.BoardDeck;
	}
	
	public IPlayer GetPlayerPlayed() {
		
	}

	

}
