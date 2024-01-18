namespace DominoGame;

public interface IDominoCard
{
    public int End1 {get; set;}
    public int End2 {get; set;}
    public int CardValue();
    public bool IsDouble();
    public override string ToString();
}
