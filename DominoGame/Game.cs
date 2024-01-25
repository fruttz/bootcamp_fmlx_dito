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

	/// <summary>
	/// Initializes a new instance of the <see cref="Game"/> class.
	/// </summary>
	/// <param name="board">The domino board for the game.</param>
	/// <param name="players">The list of players participating in the game.</param>
	/// <param name="maxPoint">The maximum point at which the game ends.</param>
	/// <param name="numberOfDistributeCard">The number of cards to distribute to each player.</param>
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
	
	/// <summary>
	/// Gets the cards held by the specified player.
	/// </summary>
	/// <param name="player">The player for whom to retrieve the cards.</param>
	/// <returns>
	/// A collection of domino cards held by the specified player.
	/// Returns <c>null</c> if the player is not found or has no cards.
	/// </returns>		
	public IEnumerable<IDominoCard>? GetPlayerCard(IPlayer player){
		return _players[player].PlayerDeck;
	}
	
	/// <summary>
	/// Gets the accumulated points of the specified player.
	/// </summary>
	/// <param name="player">The player for whom to retrieve the accumulated points.</param>
	/// <returns>
	/// An integer representing the total points accumulated by the specified player.
	/// </returns>
	public int GetPlayerPoint(IPlayer player) {
		return _players[player].Point;
	}
	
	/// <summary>
	/// Gets a value indicating whether the specified player has already put a card during the current turn.
	/// </summary>
	/// <param name="player">The player for whom to check the card-putting status.</param>
	/// <returns>
	///   <c>true</c> if the player has already put a card during the current turn; otherwise, <c>false</c>.
	/// </returns>
	public bool GetAlreadyPutCard(IPlayer player) {
		return _players[player].AlreadyPutCardThatTurn;
	}
	
	/// <summary>
	/// Gets the collection of players participating in the game.
	/// </summary>
	/// <returns>An enumerable collection of players.</returns>
	public IEnumerable<IPlayer> GetPlayers() {
		return _players.Keys;
	}
	
	/// <summary>
	/// Gets the main deck of domino cards in the game.
	/// </summary>
	/// <returns>An enumerable collection of domino cards representing the main deck.</returns>
	public IEnumerable<IDominoCard> GetMainDeck() {
		return _boards.MainDeck;
	}

	/// <summary>
	/// Sets the main deck of domino cards in the game.
	/// </summary>
	/// <param name="deck">A list of domino cards to set as the main deck.</param>
	public void SetMainDeck(List<IDominoCard> deck){
		_boards.MainDeck = deck;
	}
	
	/// <summary>
	/// Retrieves the current board deck containing domino cards.
	/// </summary>
	/// <returns>An IEnumerable collection of domino cards representing the current state of the board deck.</returns>
	public IEnumerable<IDominoCard> GetBoardDeck() {
		return _boards.BoardDeck;
	}

	/// <summary>
	/// Sets the index of the current player's turn.
	/// </summary>
	/// <param name="index">The index representing the player's turn.</param>
	public void SetIndexTurn(int index) {
		indexTurnPlayer = index;
	}

	/// <summary>
	/// Checks if the board deck is empty.
	/// </summary>
	/// <returns>True if the board deck is empty; otherwise, false.</returns>
	public bool IsEmptyBoard(){
		return _boards.BoardDeck.Count == 0;
	}

	/// <summary>
	/// Checks if the main deck is empty.
	/// </summary>
	/// <returns>True if the main deck is empty; otherwise, false.</returns>
	public bool IsEmptyMain(){
		return _boards.MainDeck.Count == 0;
	}

	/// <summary>
	/// Checks if the specified player's hand is empty.
	/// </summary>
	/// <param name="player">The player to check.</param>
	/// <returns>True if the player's hand is empty; otherwise, false.</returns>
	public bool IsPlayerHandEmpty(IPlayer player){
		return _players[player].PlayerDeck.Count == 0;
	}

	/// <summary>
	/// Retrieves the player with the specified ID.
	/// </summary>
	/// <param name="id">The ID of the player to retrieve.</param>
	/// <returns>The player with the specified ID.</returns>
	/// <exception cref="Exception">Thrown if the player with the specified ID is not found.</exception>
	public IPlayer? GetPlayer(int id){
		IPlayer player = GetPlayers().ToList().FirstOrDefault(player => player.Id == id);
		if(player is null){
			throw new Exception();
		}
		return player;
	}
	
	/// <summary>
	/// Tries to retrieve the player with the specified ID.
	/// </summary>
	/// <param name="id">The ID of the player to retrieve.</param>
	/// <param name="player">When this method returns, contains the player with the specified ID, if found; otherwise, null.</param>
	/// <returns>True if the player with the specified ID is found; otherwise, false.</returns>
	public bool TryGetPlayer(int id, out IPlayer? player) {
		player = GetPlayers().ToList().FirstOrDefault(player => player.Id == id);
		if (player is null){
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Gets the player whose turn it is based on the current turn index.
	/// </summary>
	/// <returns>The player whose turn it is.</returns>
	public IPlayer GetPlayerPlayed() {
		IPlayer player;
		TryGetPlayer(indexTurnPlayer, out player);
		return player;
	}

	/// <summary>
	/// Gets the chain status indicating whether cards can be chained in the current turn.
	/// </summary>
	/// <returns>An IEnumerable of booleans representing the chain status.</returns>
	public IEnumerable<bool> GetChainStatus() {
		return _chainStatus;

	}

	/// <summary>
	/// Creates a new player and adds it to the game.
	/// </summary>
	/// <param name="player">The player to create and add to the game.</param>
	/// <returns>True if the player is created and added successfully; otherwise, false.</returns>
	public bool CreatePlayer(IPlayer player) {
		_players.Add(player, new PlayerData());
		return true;
	}

	/// <summary>
	/// Checks if the game is ready to start with the minimum required number of players.
	/// </summary>
	/// <returns>True if the game is ready to play; otherwise, false.</returns>
	public bool IsReadyToPlay() {
		return _players.Count() >= 2;
	}

	/// <summary>
	/// Checks if the specified player is allowed to draw a card in the current turn.
	/// </summary>
	/// <param name="player">The player to check.</param>
	/// <returns>True if the player can draw a card; otherwise, false.</returns>
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
	
	/// <summary>
	/// Generates a random number within the specified range.
	/// </summary>
	/// <param name="list">The list from which to generate a random number.</param>
	/// <returns>A random number within the range of the list indices.</returns>
	public int RandomizeFromListRange(List<IDominoCard> list) {
		int number;
		Random rng = new Random();
		number = rng.Next(0, list.Count());
		return number;
		
	}
	
	/// <summary>
	/// Sets the status of a domino card.
	/// </summary>
	/// <param name="card">The domino card to set the status for.</param>
	/// <param name="cardStatus">The status to set for the domino card.</param>
	public void SetCardStatus(IDominoCard card, CardStatus cardStatus) {
		card.status = cardStatus.ToString();
	}
	
	/// <summary>
	/// Draws a card from the main deck based on the specified ID.
	/// </summary>
	/// <param name="id">The ID of the card to draw from the main deck.</param>
	/// <returns>The drawn card from the main deck, or null if the card with the specified ID is not found.</returns>
	public IDominoCard? GetCardFromMainDeck(int id) {
		if (_boards.MainDeck != null && _boards.MainDeck.ElementAtOrDefault(id) != null) {
			IDominoCard card = _boards.MainDeck[id];
			_boards.MainDeck.RemoveAt(id);
			return card;	
		}
		return null;
	}

	/// <summary>
	/// Draws a card from the main deck and adds it to the player's hand.
	/// </summary>
	/// <param name="player">The player drawing the card.</param>
	/// <returns>The drawn card, or <c>null</c> if the main deck is empty.</returns>
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

	/// <summary>
	/// Distributes cards to all players.
	/// </summary>
	/// <returns><c>true</c> if distribution is successful, <c>false</c> otherwise.</returns>
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
	
	/// <summary>
	/// Checks if any player has a double card in their hand.
	/// </summary>
	/// <returns>True if at least one player has a double card; otherwise, false.</returns>
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

	/// <summary>
	/// Determines the player with the greatest double card in their hand.
	/// </summary>
	/// <returns>The player with the greatest double card; null if no player has a double card.</returns>
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
	
	/// <summary>
	/// Gets the first card of the specified player, considering doubles.
	/// </summary>
	/// <param name="player">The player to get the first card for.</param>
	/// <returns>The first card of the player; null if no card is found.</returns>
	public IDominoCard FirstCard(IPlayer player) {
		return _players[player].FindGreatestDouble();
	}
	
	/// <summary>
	/// Checks if the specified player still has cards in their hand.
	/// </summary>
	/// <param name="player">The player to check.</param>
	/// <returns>True if the player still has cards; otherwise, false.</returns>
	public bool StillHaveCard(IPlayer player) {
		return _players[player].PlayerDeck.Count != 0;
	}
	
	/// <summary>
	/// Checks if the specified player can still play in the current turn.
	/// </summary>
	/// <param name="player">The player to check.</param>
	/// <returns>
	/// True if the player can play by chaining cards or drawing a card;
	/// otherwise, false.
	/// </returns>
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
	
	/// <summary>
	/// Checks if the specified domino card can be chained to the current board.
	/// </summary>
	/// <param name="card">The domino card to check for chaining.</param>
	/// <returns>True if the card can be chained; otherwise, false.</returns>
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
	
	/// <summary>
	/// Checks if the specified domino card can be chained to both ends of the current board.
	/// </summary>
	/// <param name="card">The domino card to check for chaining on both sides.</param>
	/// <returns>True if the card can be chained on both sides; otherwise, false.</returns>
	public bool CanChainCardBothSides(IDominoCard card) {
		if(!IsEmptyBoard()){
			IDominoCard first = _boards.BoardDeck.First();
			IDominoCard last = _boards.BoardDeck.Last();
			if ((card.End1 == first.End1 || card.End2 == first.End1) && (card.End1 == last.End2 || card.End2 == last.End2)) {
				return true;
			}
			else {
				return false;
			}
		} else {
			return false;	
		}
	}

	/// <summary>
	/// Retrieves a collection of domino cards that the specified player can play in the current turn.
	/// </summary>
	/// <param name="player">The player for whom to find playable cards.</param>
	/// <returns>
	/// A collection of playable domino cards; an empty collection if the player's hand is empty or no cards are playable.
	/// </returns>
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

	/// <summary>
	/// Chooses a playable domino card from the specified player's collection.
	/// </summary>
	/// <param name="player">The player who chooses a playable card.</param>
	/// <param name="i">The index of the chosen card in the playable cards collection.</param>
	/// <returns>The chosen playable domino card.</returns>
	public IDominoCard ChoosePlayableCard(IPlayer player, int i){
		return PlayableCards(player).ElementAt(i-1);
	}
	
	/// <summary>
	/// Plays a card from the player's hand onto the board deck.
	/// </summary>
	/// <param name="player">The player putting the card.</param>
	/// <param name="card">The card to be played.</param>
	/// <returns><c>true</c> if the card is successfully played, <c>false</c> otherwise.</returns>
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
	
	/// <summary>
	/// Places a domino card on the left side of the board.
	/// </summary>
	/// <param name="player">The player putting the card.</param>
	/// <param name="card">The card to be placed on the left side of the board.</param>
	/// <returns>True if the card is successfully placed; false otherwise.</returns>
	public bool PlayerPutCardLeft(IPlayer player, IDominoCard card) {
		IDominoCard first = _boards.BoardDeck.First();
		if (CanChainCard(card) && (card.End1 == first.End1 || card.End2 == first.End1)) {
			if (card.End2 != first.End1) {
				card.FlipCard();
			}
			_boards.BoardDeck.AddFirst(card);
			_players[player].PlayerDeck.Remove(card);
			_players[player].AlreadyPutCardThatTurn = true;
			ChangeCardStatus = SetCardStatus;
			ChangeCardStatus(card, CardStatus.BoardDeck);
			return true;	
		}
		return false;
	}
	
	/// <summary>
	/// Places a domino card on the right side of the board.
	/// </summary>
	/// <param name="player">The player putting the card.</param>
	/// <param name="card">The card to be placed on the right side of the board.</param>
	/// <returns>True if the card is successfully placed; false otherwise.</returns>
	public bool PlayerPutCardRight(IPlayer player, IDominoCard card) {
		IDominoCard last = _boards.BoardDeck.Last();
		if (CanChainCard(card) && (card.End1 == last.End2 || card.End2 == last.End2)) {
			if (card.End1 != last.End2) {
				card.FlipCard();
			}
			_boards.BoardDeck.AddLast(card);
			_players[player].PlayerDeck.Remove(card);
			_players[player].AlreadyPutCardThatTurn = true;
			ChangeCardStatus = SetCardStatus;
			ChangeCardStatus(card, CardStatus.BoardDeck);
			return true;
		}
		return false;

	}
	
	/// <summary>
	/// Checks if all players cannot chain a card in the current turn.
	/// </summary>
	/// <returns>True if all players cannot chain; false otherwise.</returns>
	public bool AllPlayerCannotChain() {
		return _chainStatus.All(x => x == _chainStatus[0]);
	}
	
	/// <summary>
	/// Adds a chain status to the collection.
	/// </summary>
	/// <param name="status">The chain status to be added.</param>
	public void AddChainStatus(bool status) {
		if (status) {
			_chainStatus.Clear();
		}
		else {
			_chainStatus.Add(status);
		}
	}
	
	/// <summary>
	/// Calculates the total points of a player based on their accumulated cards.
	/// </summary>
	/// <param name="player">The player for whom to calculate the points.</param>
	/// <returns>The total points of the player.</returns>
	public int CalculatePoint(IPlayer player) {
		return _players[player].AccumulateCard();
	}
	
	/// <summary>
	/// Advances the turn to the next player in the game.
	/// </summary>
	/// <returns>The player who plays the next turn.</returns>
	public IPlayer NextTurn() {
		_players[GetPlayerPlayed()].AlreadyPutCardThatTurn = false;
		indexTurnPlayer = (indexTurnPlayer % _players.Keys.Count) + 1;
		return GetPlayerPlayed();
	}
	
	/// <summary>
	/// Retrieves the current status of the game.
	/// </summary>
	/// <returns>The current status of the game.</returns>
	public GameStatus GetGameStatus() {
		return _gameStatus;
	}
	
	/// <summary>
	/// Determines the winner of the current round.
	/// </summary>
	/// <returns>The player who has won the round; null if no winner yet.</returns>
	public IPlayer? RoundWinner() {
		foreach (IPlayer player in _players.Keys) {
			if (IsPlayerHandEmpty(player)) {
				return player;
			}
		}
		return null;
	}

	/// <summary>
	/// Ends the current round and updates player scores.
	/// </summary>
	/// <param name="player1">The first player involved in the round.</param>
	/// <param name="player2">The second player involved in the round.</param>
	/// <returns>True if the round is successfully over; false otherwise.</returns>
	public bool RoundOver(IPlayer player1, IPlayer player2) {
		_players[player1].IncreasePoint(_players[player2].AccumulateCard());
		return true;
	}
	
	/// <summary>
	/// Checks if any player has reached the maximum points required to win the game.
	/// </summary>
	/// <returns>True if a player has reached the maximum points; false otherwise.</returns>
	public bool PlayerReachMaxPoint() {
		foreach(IPlayer player in _players.Keys) {
			if (_players[player].Point >= maxPoint) {
				return true;
			}
		}
		return false;
	}
	
	/// <summary>
	/// Determines the winner of the entire game.
	/// </summary>
	/// <returns>The player who has won the game.</returns>
	public IPlayer? GameWinner() {
		foreach (IPlayer player in _players.Keys) {
			if (_players[player].Point >= maxPoint) {
				return player;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Resets the points of all players to zero.
	/// </summary>
	/// <returns>True if the points are successfully reset; false otherwise.</returns>
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
	
	/// <summary>
	/// Resets the player decks and initializes the main deck.
	/// </summary>
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
	
	/// <summary>
	/// Resets the board by clearing the board deck.
	/// </summary>
	/// <returns>True if the board is successfully reset; false otherwise.</returns>
	public bool ResetBoard() {
		_boards.BoardDeck.Clear();
		return true;
	}
}
