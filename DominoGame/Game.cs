using System.Linq;
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
	public Action<IDominoCard, CardStatus> CardStatus;

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
		return _players.Keys;
	}
	
	public IEnumerable<IDominoCard> GetMainDeck() {
		return _boards.MainDeck;
	}
	
	public IEnumerable<IDominoCard> GetBoardDeck() {
		return _boards.BoardDeck;
	}
	
	public IPlayer GetPlayerPlayed() {
		
	}

	public IEnumerable<bool> GetChainStatus() {

	}

	public bool CreatePlayer(IPlayer player) {

	}

	public bool IsReadyToPlay() {

	}

	public bool CanDrawCard(IPlayer player) {

	}

	public IDominoCard DrawCard(IPlayer player) {

	}

	public bool DistributeCards() {
		if (IsReadyToPlay()){
			foreach (IPlayer player in _players.Keys){
				for (int i = 0; i <= numberDistributeCards; i++){
					_players[player].PlayerDeck.Add(); 
				}
			}
			return true;
		}
		else {
			return false;
		}
	}

	public bool PlayerHasDouble() {
		foreach (IPlayer player in _players.Keys){
			foreach(IDominoCard card in _players[player].PlayerDeck){
				if (card.IsDouble()){
					return true;
				}
			}
		}
		return false;
	}

	public IPlayer? WhoHasGreatestDouble() {
		if (PlayerHasDouble()){
			Dictionary<IPlayer, IDominoCard> playerDoubles = new Dictionary<IPlayer, IDominoCard>();
			foreach (IPlayer player in _players.Keys){
				playerDoubles.Add(player, _players[player].FindGreatestDouble());
			}
			IPlayer greatestDouble = playerDoubles.Aggregate((x,y) => x.Value.CardValue() > y.Value.CardValue() ? x : y).Key;
			return greatestDouble;
		}
		return null;
	}



	



	

}
