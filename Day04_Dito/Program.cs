class Program
{
	public static void Main(string[] args)
	{
		Car car = new Car(new Engine(25), "Toyota", 10);
		Car electricCar = new Car(new ElectricEngine(), "Tesla", 25);

		car.Name = "Nissan";
		Console.WriteLine(car.Name);
		
		car.CarStart();
		electricCar.CarStart();
		
		
	}
	
	static int Add(Car a)
	{
		int carPrice = a.GetPrice();
		return carPrice += 5;	
	}
}

public class Car
{
	public Engine engine;
	public string Name { get; set; }
	private int _price;
	public Car(Engine engine, string Name, int price)
	{
		this.Name = Name;
		this.engine = engine;
		this._price = price;
	}
	
	public int GetPrice()
	{
		return this._price;
	}
	
	public void CarStart()
	{
		engine.EngineRun();
	}
}

public class Engine
{
	private int _cylinder;
	
	public Engine(int cylinder)
	{
		this._cylinder = cylinder;
	}
	public virtual void EngineRun()
	{
		Console.WriteLine($"Engine with {_cylinder}");
	}
}

public class ElectricEngine : Engine
{
	public ElectricEngine() : base(0)
	{
		
	}
	public override void EngineRun()
	{
		Console.WriteLine("Electric Engine Running..");
	}
}

