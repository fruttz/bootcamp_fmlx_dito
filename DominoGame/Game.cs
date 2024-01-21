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

	public Game(IDominoBoard board, List<IPlayer> players,  int maxPoint, int numberOfDistributeCard){
		this._boards = board;
		this.maxPoint = maxPoint;
		this.numberDistributeCards = numberOfDistributeCard;
		this._players = new Dictionary<IPlayer, PlayerData>();
		foreach(IPlayer player in players) {
			CreatePlayer(player);
		}
		InitializeMainDeck();
        DistributeCards();
		this.indexTurnPlayer = WhoHasGreatestDouble().Id;
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

	public void InitializeMainDeck(){
		//List<IDominoCard> deck = new List<IDominoCard>();
		for (int i = 0; i <= 6; i++){
			for(int j = i; j <= 6; j++){
				IDominoCard card = new DominoCard(i, j, CardStatus.MainDeck.ToString());
				_boards.MainDeck.Add(card);
			}
		}
	}
	
	public IEnumerable<IDominoCard> GetMainDeck() {
		return _boards.MainDeck;
	}
	
	public IEnumerable<IDominoCard> GetBoardDeck() {
		return _boards.BoardDeck;
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

	public void PrintBoardDeck(){
		if(_boards.BoardDeck != null && !IsEmptyBoard()){
			foreach(IDominoCard card in _boards.BoardDeck){
				Console.Write(card.ToString());
			}
		}
		else{
			Console.WriteLine("EMPTY BOARD");
		}
	}

	public void PrintPlayerHand(IPlayer player){
		if(_players[player].PlayerDeck != null){
			foreach(IDominoCard card in _players[player].PlayerDeck){
				Console.Write(card.ToString());
			}
		}
	}

	public void PrintPlayableCards(IPlayer player){
		int i = 0;
		foreach(IDominoCard card in PlayableCards(player)){
			i++;
			Console.WriteLine($"{i}. {card.ToString()}");
		}

	}

	public void PrintMainDeck(){
		if (_boards.MainDeck != null){
			foreach(IDominoCard card in _boards.MainDeck){
				Console.Write(card.ToString());
			}
		}
		
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
		
		if (!IsEmptyMain() && !_players[player].AlreadyPutCardThatTurn) {
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
		if (PlayerHasDouble()){
			Dictionary<IPlayer, IDominoCard> playerDoubles = new Dictionary<IPlayer, IDominoCard>();
			foreach (IPlayer player in _players.Keys){
				playerDoubles.Add(player, _players[player].FindGreatestDouble());
			}
			IPlayer greatestDouble = playerDoubles.OrderByDescending(x => x.Value.CardValue()).FirstOrDefault().Key;
			return greatestDouble;
		}
		else {
			return GetPlayer(1);
		}
	}
	
	public IDominoCard FirstCard(IPlayer player) {
		return _players[player].FindGreatestDouble();
	}
	
	public bool StillHaveCard(IPlayer player) {
		return _players[player].PlayerDeck != null;
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
		if (card.End1 == first.End1 || card.End2 == first.End2 || card.End1 == last.End1 || card.End2 == last.End2) {
			return true;
		}
		else {
			return false;
		}
	}

	public List<IDominoCard> PlayableCards(IPlayer player){
		List<IDominoCard> cards = new List<IDominoCard>();
		if(!IsEmptyBoard()){
			foreach(IDominoCard card in GetPlayerCard(player)){
				if(CanChainCard(card)){
					cards.Add(card);
				}
			}
		}
		else {
			cards.Add(FirstCard(player));
		}
		return cards;

	}

	public IDominoCard ChoosePlayableCard(IPlayer player, int i){
		return PlayableCards(player)[i-1];
	}
	
	public bool PlayerPutCard(IPlayer player, IDominoCard card) {
		// Naruh kartu ke board deck
		if(!IsEmptyBoard()){
			IDominoCard first = _boards.BoardDeck.First();
			IDominoCard last = _boards.BoardDeck.Last();
			if (CanChainCard(card) && (card.End1 == first.End1 || card.End2 == first.End2)) {
				if (card.End1 != first.End1) {
					card.FlipCard();
				}
				_boards.BoardDeck.AddFirst(card);	
			}
			else if (CanChainCard(card) && (card.End1 == last.End1 || card.End2 == last.End2)) {
				if (card.End1 != last.End1) {
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
	
	public bool ResetPlayerCard() {
		//ngosongin player deck, inisialisasi ulang maindeck
		//dipanggil waktu distribute card gaada double
		//dipanggil waktu menang round

		foreach(IPlayer player in _players.Keys) {
			if(_players[player].PlayerDeck != null) {
				_players[player].PlayerDeck.Clear();
			}
			else{
				return false;
			}
		}
		
		_boards.MainDeck.Clear();
		InitializeMainDeck();
		return true;

		
	}
	
	public bool ResetBoard(IDominoBoard board) {
		_boards = board;
		return true;
	}
	
	
	
	
	
	
	



	



	

}
