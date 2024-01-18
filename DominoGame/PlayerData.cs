using System.Linq;
namespace DominoGame;

public class PlayerData
{
	public List<IDominoCard>? PlayerDeck {get; internal set;}
	public int Win {get; internal set;}
	public int Point {get; internal set;}
	public bool AlreadyPutCardThatTurn {get; internal set;}
	
	public PlayerData()
	
	{
		Win = 0;
		Point = 0;
		AlreadyPutCardThatTurn = false;
		PlayerDeck = new List<IDominoCard>();
	}

	public int AccumulateCard(){
		if (PlayerDeck != null){
			int value = 0;
			foreach (IDominoCard card in PlayerDeck){
				value += card.CardValue();
			}
			return value;
		}
		else{
			return 0;
		}
	}

	public IDominoCard? FindGreatestDouble(){
		if (PlayerDeck != null){
			List<IDominoCard> doubles = new List<IDominoCard>();
			foreach(IDominoCard card in PlayerDeck){
				if (card.IsDouble()){
					doubles.Add(card);
				}
			}
			IDominoCard? maxDouble = doubles.MaxBy(x => x.CardValue());
			return maxDouble;
		}
		return null;
	}
}
