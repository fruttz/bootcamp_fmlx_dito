namespace DominoGame;

public class DominoCard : IDominoCard
{
	public int End1 {get; set;}
	public int End2 {get; set;}
	public CardStatus status;

	public DominoCard(int end1, int end2){
		this.End1 = end1;
		this.End2 = end2;
		this.status = CardStatus.MainDeck;
	}

	public int CardValue(){
		return End1 + End2;
	}

	public bool IsDouble(){
		return End1 == End2;
	}
	
	public void FlipCard() {
		(End1,End2) = (End2,End1);
	}
	
	public override string ToString()
	
	{
		return $"[{End1}|{End2}]";
	}
}
