namespace DominoGame;

public class Player : IPlayer
{
    public string Name {get; set;}
    public int Id {get; set;}

    public Player(string name, int id){
        this.Name = name;
        this.Id = id;
    }

}
