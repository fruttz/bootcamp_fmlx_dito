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
        int maxPoint = int.Parse(Console.ReadLine());

        Game game = new Game(board, players, maxPoint, 7);

        //GAME START

        if(game.IsReadyToPlay()){
            Console.WriteLine("===== GAME START =====");

            IPlayer currentPlayer = game.GetPlayerPlayed();

            while(game.FirstCard(currentPlayer) is null){
                game.ResetPlayerCard();
                game.DistributeCards();
            }

            while(!game.PlayerReachMaxPoint()){
                Console.WriteLine("==============================");
                Console.WriteLine($"{currentPlayer.Name}'s Turn");
                Console.WriteLine("==============================");
                Console.WriteLine();

                Console.WriteLine("BOARD:");
                game.PrintBoardDeck();
                Console.WriteLine();
                Console.WriteLine("==============================");
                Console.WriteLine();

                Console.WriteLine("YOUR HAND: ");
                game.PrintPlayerHand(currentPlayer);
                Console.WriteLine();
                Console.WriteLine("==============================");
                Console.WriteLine();

                while(game.PlayableCards(currentPlayer).Count > 0) {
                    Console.WriteLine("Playable Cards:");
                    game.PrintPlayableCards(currentPlayer);
                    Console.Write("Choose which cards to play: ");
                    int choice;
                    while(!int.TryParse(Console.ReadLine(), out choice) || (choice - 1) > game.PlayableCards(currentPlayer).Count){
                        Console.WriteLine("Try Again");
                    }

                    Console.WriteLine("==============================");
                    game.PlayerPutCard(currentPlayer, game.ChoosePlayableCard(currentPlayer, choice));
                    Console.WriteLine();

                    Console.WriteLine("BOARD:");
                    game.PrintBoardDeck();
                    Console.WriteLine();
                    Console.WriteLine("==============================");
                    Console.WriteLine();
                }

                Console.WriteLine("There are no playable cards...");
                while(game.StillCanPlay(currentPlayer)){
                    Console.WriteLine("Press 'Enter' to draw cards");
                    if(Console.ReadKey().Key == ConsoleKey.Enter){
                        Console.WriteLine("You drew " + game.DrawCard(currentPlayer).ToString());
                    }
                    else{
                        Console.WriteLine("Try Again");
                    }
                }
               

                currentPlayer = game.NextTurn();
            }
            

        }
        




        

    }
}
