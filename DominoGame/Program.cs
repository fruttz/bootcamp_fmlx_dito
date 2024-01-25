using System.Linq;
namespace DominoGame;

public class Program{
	public static void Main(string[] args){

		//INITIALIZATION
		DominoBoard board = new DominoBoard(new List<IDominoCard>(), new LinkedList<IDominoCard>());
		
		Console.WriteLine("Player 1 Name: ");
		string? player1Name = Console.ReadLine();
		Console.WriteLine("Player 2 Name: ");
		string? player2Name = Console.ReadLine();


		Player player1 = new Player(player1Name, 1);
		Player player2 = new Player(player2Name, 2);
		List<IPlayer> players = new List<IPlayer>();
		players.Add(player1);
		players.Add(player2);

		Console.WriteLine("Insert Maximum Point: ");
		int maxPoint;
		while(!int.TryParse(Console.ReadLine(), out maxPoint) && maxPoint <= 0){
			Console.WriteLine("Try Again");
		}
		
		



		Game game = new Game(board, players, maxPoint, 7);

		//GAME START

		if(game.IsReadyToPlay()){
			Console.WriteLine("===== GAME START =====");

			List<IDominoCard> mainDeck = new List<IDominoCard>();
			do{
				mainDeck.Clear();
				if(mainDeck.Count == 0) {
					for (int i = 0; i <= 6; i++){
						for(int j = i; j <= 6; j++){
							IDominoCard card = new DominoCard(i, j, CardStatus.MainDeck.ToString());
							mainDeck.Add(card);
						}
					}	
				}
				game.SetMainDeck(mainDeck);
				game.DistributeCards();
			} while(game.WhoHasGreatestDouble() is null);
			game.SetIndexTurn(game.WhoHasGreatestDouble().Id);

			IPlayer currentPlayer = game.GetPlayerPlayed();

			while(!game.PlayerReachMaxPoint()){
				Console.WriteLine("==============================");
				Console.WriteLine($"{currentPlayer.Name}'s Turn");
				Console.WriteLine("==============================");
				Console.WriteLine();

				Console.WriteLine("BOARD:");
				if(game.GetBoardDeck() != null && !game.IsEmptyBoard()){
					foreach(IDominoCard card in game.GetBoardDeck()){
						Console.Write(card.ToString());
					}
				}
				else{
					Console.WriteLine("EMPTY BOARD");
				}
				Console.WriteLine();
				Console.WriteLine("==============================");
				Console.WriteLine();

				Console.WriteLine("YOUR HAND: ");
				if(!game.IsPlayerHandEmpty(currentPlayer)){
					foreach(IDominoCard card in game.GetPlayerCard(currentPlayer)){
						Console.Write(card.ToString());
					}
				}
				Console.WriteLine();
				Console.WriteLine("==============================");
				Console.WriteLine();

				while(game.PlayableCards(currentPlayer).ToList().Count > 0) {
					List<IDominoCard> boardDeck = game.GetBoardDeck().ToList();
					Console.WriteLine("Playable Cards:");
					int i = 0;
					foreach(IDominoCard card in game.PlayableCards(currentPlayer)){
						i++;
						Console.WriteLine($"{i}. {card.ToString()}");
					}
					Console.Write("Choose which cards to play: ");
					int choice;
					while(!int.TryParse(Console.ReadLine(), out choice) || choice > game.PlayableCards(currentPlayer).ToList().Count){
						Console.WriteLine("Try Again");
					}
					var playedCard = game.ChoosePlayableCard(currentPlayer, choice);

					Console.WriteLine("==============================");
					if(game.CanChainCardBothSides(playedCard)){
						Console.WriteLine("Pick a side: ");
						Console.WriteLine("1. Left");
						Console.WriteLine("2. Right");
						int sideChoice;
						while(!int.TryParse(Console.ReadLine(), out sideChoice) || sideChoice > 2){
							Console.WriteLine("Try Again");
						}
						if (sideChoice == 1) {
							game.PlayerPutCardLeft(currentPlayer, playedCard);
							Console.WriteLine();
						} else {
							game.PlayerPutCardRight(currentPlayer, playedCard);
							Console.WriteLine();
						}	
					} else {
						game.PlayerPutCard(currentPlayer, playedCard);
						Console.WriteLine();
					}
					
					Console.WriteLine("==============================");
					Console.WriteLine($"{currentPlayer.Name}'s Turn");
					Console.WriteLine("==============================");
					Console.WriteLine();

					Console.WriteLine("BOARD:");
					if(game.GetBoardDeck() != null && !game.IsEmptyBoard()){
						foreach(IDominoCard card in game.GetBoardDeck()){
							Console.Write(card.ToString());
						}
					}
					else{
						Console.WriteLine("EMPTY BOARD");
					}
					Console.WriteLine();
					Console.WriteLine("==============================");
					Console.WriteLine();
					
					Console.WriteLine("YOUR HAND: ");
					foreach(IDominoCard card in game.GetPlayerCard(currentPlayer)){
						Console.Write(card.ToString());
					}
					Console.WriteLine();
					Console.WriteLine("==============================");
					Console.WriteLine();
				}
				if(!game.GetAlreadyPutCard(currentPlayer)) {
					Console.WriteLine("There are no playable cards...");
					while(game.PlayableCards(currentPlayer).ToList().Count == 0 && !game.IsEmptyMain()){
						Console.WriteLine("Press 'Enter' to draw cards");
						if(Console.ReadKey().Key == ConsoleKey.Enter){
							Console.WriteLine("You drew " + game.DrawCard(currentPlayer).ToString());
							Console.WriteLine("YOUR HAND: ");
							foreach(IDominoCard card in game.GetPlayerCard(currentPlayer)){
								Console.Write(card.ToString());
							}
							Console.WriteLine();
							Console.WriteLine("==============================");
							Console.WriteLine();
						}
						else{
							Console.WriteLine("Try Again");
						}
					}
					if(game.IsEmptyMain()){
						Console.WriteLine("==============================");
						Console.WriteLine("DECK EMPTY");
						Console.WriteLine("==============================");
					}
				}
				
				if((game.IsEmptyMain() && !game.StillCanPlay(currentPlayer)) || currentPlayer == game.RoundWinner()) {
					Console.WriteLine("==============================");
					Console.WriteLine("ROUND OVER");
					Console.WriteLine("==============================");
					Console.WriteLine();
					
					game.RoundOver(currentPlayer, game.GetPlayer((game.GetPlayerPlayed().Id % players.Count) + 1));
					foreach(IPlayer player in players) {
						Console.WriteLine($"{player.Name}'s Points: {game.GetPlayerPoint(player)}");
					}
					Console.WriteLine("Press 'Enter' to Start New Round");
					while(Console.ReadKey().Key != ConsoleKey.Enter) {
						Console.WriteLine("Try Again");
					}

					do{
						mainDeck.Clear();
						for (int i = 0; i <= 6; i++){
							for(int j = i; j <= 6; j++){
								IDominoCard card = new DominoCard(i, j, CardStatus.MainDeck.ToString());
								mainDeck.Add(card);
							}
						}
						game.SetMainDeck(mainDeck);	
						game.ResetPlayerCard();
					} while (game.WhoHasGreatestDouble() is null);
					game.ResetBoard();
				}	
				else {
					currentPlayer = game.NextTurn();
					Console.Clear();	
				}
			}
			Console.WriteLine("==============================");
			Console.WriteLine("GAME OVER");
			Console.WriteLine($"WINNER: {game.GameWinner().Name}");
			Console.WriteLine("==============================");
			
			
		}
		
	}
}
