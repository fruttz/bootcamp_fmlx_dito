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

        game.InitializeMainDeck();

        //GAME START

        //game.PrintMainDeck();

        game.DistributeCards();
        game.PrintPlayerHand(player1);




        

    }
}
