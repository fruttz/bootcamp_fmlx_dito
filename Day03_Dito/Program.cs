class Program
{
	public static void Main(string[] args)
	{
		Car car_a = new Car(3);
		Car car_b = car_a;
		car_b.price = car_b.price + 2;
		
		Console.WriteLine(car_a.price);
		Console.WriteLine(car_b.price);
		
	}
}  

class Animal
{
	protected string _name;
	protected string _description;
	public Animal(string name, string description)
	{
		this._name = name;
		this._description = description;
	}
	
	public virtual void MakeSound()
	{
		Console.WriteLine("Sound");
	}
	
	
}

class Cats : Animal
{
	private string _breed;
	public Cats(string breed) : base("cat", "feline")
	{
		this._breed = breed;
	}
	
	public void Get()
	{
		Console.WriteLine(this._name);
		Console.WriteLine(this._description);
		Console.WriteLine(this._breed);
		
	}

	public override void MakeSound()
	{
		Console.WriteLine("Nya :3");
	}
	
}

class Car
{
	public int price;
	public Car(int price)
	{
		this.price = price;
	}
}
