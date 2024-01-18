using System.Linq;
namespace DominoGame;

public class Game
{
	private Dictionary<IPlayer, PlayerData> _players;
	private IDominoBoard _boards;
	private GameStatus _gameStatus;
	private List<bool> _chainStatus;
	private int indexTurnPlayer;
	private int maxPoint;
	private int numberDistributeCards;
	public Action<IDominoCard, CardStatus> CardStatus;

	public Game(IDominoBoard board, List<IPlayer> players,  int maxPoint, int numberOfDistributeCard){
		this._boards = board;
		this.maxPoint = maxPoint;
		this.numberDistributeCards = numberOfDistributeCard;
		foreach(IPlayer player in players)
		{
			CreatePlayer(player);
		}
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
		return _players[indexTurnPlayer];
	}

	public IEnumerable<bool> GetChainStatus() {
		return _chainStatus;

	}

	public bool CreatePlayer(IPlayer player) {
		_players.Add(player, new PlayerData());
	}

	public bool IsReadyToPlay() {
		return _players.Count() >= 2;
	}

	public bool CanDrawCard(IPlayer player) {
		//cek main deck
		//cek playerdata.alreadyputcard
		//misal gaisa naruh kartu, bisa minum nek blm naruh kartu
	}

	public IDominoCard DrawCard(IPlayer player) {
		// Bisa ngambil random kartu dari main deck
		_players[player].PlayerDeck.Add();
		return 
	}

	public bool DistributeCards() {
		if (IsReadyToPlay()){
			foreach (IPlayer player in _players.Keys){
				for (int i = 0; i <= numberDistributeCards; i++){
					DrawCard(player); 
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
	
	public IDominoCard FirstCard(IPlayer player) {
		//cari greatest double dari player
	}
	
	public bool StillHaveCard(IPlayer player) {
		return _players[player].PlayerDeck != null;
	}
	
	public bool StillCanPlay(IPlayer player) {
		//ngecek playerdeck kalo ada kartu yang bisa dimainin
		//kalo gaada yang bisa dimainin, cek bisa draw apa engga
		//kalo masih gabisa, return false.
	}
	
	public bool CanChainCard(IDominoCard card) {
		//cek ujung board deck
		//intine cek kalo kartune bisa dichain
	}
	
	public bool PlayerPutCard(IPlayer player, IDominoCard card) {
		// Naruh kartu ke board deck 
		_players[player].PlayerDeck.Remove(card);
		_players[player].AlreadyPutCardThatTurn = true;
		card.status = CardStatus.BoardDeck;
	}
	
	public bool AllPlayerCannotChain() {
		return _chainStatus.All(x => x == _chainStatus[0]);
	}
	
	public void AddChainStatus(bool status) {
		if (status) {
			_chainStatus.Clear();
		}
		else {
			_chainStatus.Add(status);
		}
	}
	
	public int CalculatePoint(IPlayer player) {
		return _players[player].AccumulateCard();
	}
	
	public void NextTurn() {
		indexTurnPlayer = (indexTurnPlayer + 1) % _players.Keys.Count(); 
	}
	
	public GameStatus GetGameStatus() {
		return _gameStatus;
	}
	
	public IPlayer RoundWinner() {
		foreach (IPlayer player in _players) {
			if (_players[player].PlayerDeck == null) {
				return player;
			}
		}
		
	}
	
	public bool PlayerReachMaxPoint() {
		foreach(IPlayer player in _players) {
			if _players[player].Point >= maxPoint {
				return true;
			}
		}
		return false;
	}
	
	public IPlayer GameWinner() {
		if (PlayerReachMaxPoint())
		{
			foreach (IPlayer player in _players) {
				if _players[player].Point >= maxPoint {
					return player;
				}
			}
			return null;
		}
	}
	
	public bool ResetPoint() {
		//reset point setiap player
	}
	
	public bool ResetPlayerCard() {
		//ngosongin player deck, inisialisasi ulang maindeck
		//dipanggil waktu distribute card gaada double
		//dipanggil waktu menang round
		
	}
	
	public bool ResetBoard(IDominoBoard board) {
		_boards = board;
	}
	
	
	
	
	
	
	



	



	

}
