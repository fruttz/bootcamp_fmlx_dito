using System.Linq;
namespace DominoGame;

public class Game
{
	private Dictionary<IPlayer, PlayerData> _players;
	private IDominoBoard _boards;
	private GameStatus _gameStatus = GameStatus.NotReady;
	private List<bool> _chainStatus;
	private int indexTurnPlayer;
	private int maxPoint;
	private int numberDistributeCards;
	public Action<IDominoCard, CardStatus> ChangeCardStatus;

	public Game(IDominoBoard board, List<IPlayer> players, int maxPoint, int numberOfDistributeCard){
		this._boards = board;
		this.maxPoint = maxPoint;
		this.numberDistributeCards = numberOfDistributeCard;
		this._players = new Dictionary<IPlayer, PlayerData>();
		foreach(IPlayer player in players) {
			CreatePlayer(player);
		}
		this._gameStatus = GameStatus.Playing;

	}
	
	public IEnumerable<IDominoCard>? GetPlayerCard(IPlayer player){
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

	public void SetMainDeck(List<IDominoCard> deck){
		_boards.MainDeck = deck;
	}
	
	public IEnumerable<IDominoCard> GetBoardDeck() {
		return _boards.BoardDeck;
	}

	public void SetIndexTurn(int index) {
		indexTurnPlayer = index;
	}

	public bool IsEmptyBoard(){
		return _boards.BoardDeck.Count == 0;
	}

	public bool IsEmptyMain(){
		return _boards.MainDeck.Count == 0;
	}

	public bool IsPlayerHandEmpty(IPlayer player){
		return _players[player].PlayerDeck.Count == 0;
	}

	public IPlayer? GetPlayer(int id){
		foreach(IPlayer player in _players.Keys){
			if (player.Id == id){
				return player;
			}
		}
		return null;
	}
	
	public IPlayer GetPlayerPlayed() {
		return GetPlayer(indexTurnPlayer);
	}

	public IEnumerable<bool> GetChainStatus() {
		return _chainStatus;

	}

	public bool CreatePlayer(IPlayer player) {
		_players.Add(player, new PlayerData());
		return true;
	}

	public bool IsReadyToPlay() {
		return _players.Count() >= 2;
	}

	public bool CanDrawCard(IPlayer player) {
		//cek main deck
		//cek playerdata.alreadyputcard
		//misal gaisa naruh kartu, bisa minum nek blm naruh kartu
		
		if (!IsEmptyMain()) {
			return true;
		}
		else {
			return false;
		}
	}
	
	public int RandomizeFromListRange(List<IDominoCard> list) {
		int number;
		Random rng = new Random();
		number = rng.Next(0, list.Count());
		return number;
		
	}
	
	public void SetCardStatus(IDominoCard card, CardStatus cardStatus) {
		card.status = cardStatus.ToString();
	}
	
	public IDominoCard? GetCardFromMainDeck(int id) {
		if (_boards.MainDeck != null && _boards.MainDeck.ElementAtOrDefault(id) != null) {
			IDominoCard card = _boards.MainDeck[id];
			_boards.MainDeck.RemoveAt(id);
			return card;	
		}
		return null;
		
		
	}

	public IDominoCard? DrawCard(IPlayer player) {
		// Bisa ngambil random kartu dari main deck
		if (CanDrawCard(player)) {
			IDominoCard card = GetCardFromMainDeck(RandomizeFromListRange(_boards.MainDeck));
			_players[player].PlayerDeck.Add(card);
			ChangeCardStatus = SetCardStatus;
			ChangeCardStatus(card, CardStatus.PlayerHand);
			return card;	
		}
		return null;
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
		foreach (IPlayer player in _players.Keys) {
			foreach (IDominoCard card in _players[player].PlayerDeck) {
				if (card.IsDouble()) {
					return true;
				}
			}
		}
		return false;
	}

	public IPlayer? WhoHasGreatestDouble() {
		if (_players != null && _players.Any() && PlayerHasDouble()) {
			Dictionary<IPlayer, IDominoCard> playerDoubles = new Dictionary<IPlayer, IDominoCard>();
			foreach (IPlayer player in _players.Keys) {
				IDominoCard greatestDouble = _players[player].FindGreatestDouble();
				if (greatestDouble != null) {
					playerDoubles.Add(player, greatestDouble);
				}
			}

			if (playerDoubles.Count > 0) {
				IPlayer greatestDoublePlayer = playerDoubles.Aggregate((x, y) => x.Value.CardValue() > y.Value.CardValue() ? x : y).Key;

				return greatestDoublePlayer;
			}
		}

		return null;
	}
	
	public IDominoCard FirstCard(IPlayer player) {
		return _players[player].FindGreatestDouble();
	}
	
	public bool StillHaveCard(IPlayer player) {
		return _players[player].PlayerDeck.Count != 0;
	}
	
	
	public bool StillCanPlay(IPlayer player) {
		//ngecek playerdeck kalo ada kartu yang bisa dimainin
		//kalo gaada yang bisa dimainin, cek bisa draw apa engga
		//kalo masih gabisa, return false.
		if(!IsEmptyBoard()){
			foreach(IDominoCard card in _players[player].PlayerDeck) {
				if (CanChainCard(card)) {
					return true;
				}
			}
			return CanDrawCard(player);
		}
		return true;
	}
	
	public bool CanChainCard(IDominoCard card) {
		//cek ujung board deck
		//intine cek kalo kartune bisa dichain
		IDominoCard first = _boards.BoardDeck.First();
		IDominoCard last = _boards.BoardDeck.Last();
		if (card.End1 == first.End1 || card.End2 == first.End1 || card.End1 == last.End2 || card.End2 == last.End2) {
			return true;
		}
		else {
			return false;
		}
	}

	public IEnumerable<IDominoCard> PlayableCards(IPlayer player){
		if(GetPlayerCard(player) != null && !IsPlayerHandEmpty(player)) {
			List<IDominoCard> cards = new List<IDominoCard>();
			if(!IsEmptyBoard()){
				foreach(IDominoCard card in GetPlayerCard(player)){
					if(CanChainCard(card)){
						cards.Add(card);
					}
				}
				return cards;
			} else {
				cards.Add(FirstCard(player));
				return cards;
			}
		}
		return Enumerable.Empty<IDominoCard>();

	}

	public IDominoCard ChoosePlayableCard(IPlayer player, int i){
		return PlayableCards(player).ElementAt(i-1);
	}
	
	public bool PlayerPutCard(IPlayer player, IDominoCard card) {
		// Naruh kartu ke board deck
		if(!IsEmptyBoard()){
			IDominoCard first = _boards.BoardDeck.First();
			IDominoCard last = _boards.BoardDeck.Last();
			if (CanChainCard(card) && (card.End1 == first.End1 || card.End2 == first.End1)) {
				if (card.End2 != first.End1) {
					card.FlipCard();
				}
				_boards.BoardDeck.AddFirst(card);	
			}
			else if (CanChainCard(card) && (card.End1 == last.End2 || card.End2 == last.End2)) {
				if (card.End1 != last.End2) {
					card.FlipCard();
				}
				_boards.BoardDeck.AddLast(card);
			}
		}
		else{
			_boards.BoardDeck.AddLast(card);
		}
		_players[player].PlayerDeck.Remove(card);
		_players[player].AlreadyPutCardThatTurn = true;
		ChangeCardStatus = SetCardStatus;
		ChangeCardStatus(card, CardStatus.BoardDeck);
		return true;
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
	
	public IPlayer NextTurn() {
		_players[GetPlayerPlayed()].AlreadyPutCardThatTurn = false;
		indexTurnPlayer = (indexTurnPlayer % _players.Keys.Count) + 1;
		return GetPlayerPlayed();
	}
	
	public GameStatus GetGameStatus() {
		return _gameStatus;
	}
	
	public IPlayer? RoundWinner() {
		foreach (IPlayer player in _players.Keys) {
			if (IsPlayerHandEmpty(player)) {
				return player;
			}
		}
		return null;
		
	}

	public bool RoundOver(IPlayer player1, IPlayer player2) {
		_players[player1].IncreasePoint(_players[player2].AccumulateCard());
		return true;
	}
	
	public bool PlayerReachMaxPoint() {
		foreach(IPlayer player in _players.Keys) {
			if (_players[player].Point >= maxPoint) {
				return true;
			}
		}
		return false;
	}
	
	public IPlayer? GameWinner() {
		if (PlayerReachMaxPoint()) {
			foreach (IPlayer player in _players.Keys) {
				if (_players[player].Point >= maxPoint) {
					return player;
				}
			}
		}
		return null;
	}
	
	public bool ResetPoint() {
		//reset point setiap player
		if (_players != null){
			foreach(IPlayer player in _players.Keys){
				_players[player].Point = 0;
			}
			return true;
		}
		return false;
	}
	
	public void ResetPlayerCard() {
		//ngosongin player deck, inisialisasi ulang maindeck
		//dipanggil waktu distribute card gaada double
		//dipanggil waktu menang round
		foreach(IPlayer player in _players.Keys) {
			if(!IsPlayerHandEmpty(player)) {
				_players[player].PlayerDeck.Clear();
			}
		}
		
		
		DistributeCards();		
		indexTurnPlayer = WhoHasGreatestDouble().Id;	
	}
	
	public bool ResetBoard() {
		_boards.BoardDeck.Clear();
		return true;
	}
	
	
	
	
	
	
	



	



	

}
