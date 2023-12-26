class Program
{
	public static void Main(string[] args)
	{
		Car car = new Car();
		Console.WriteLine(Car.Counter());
		Car car1= new Car();
		Console.WriteLine(Car.Counter());
		Car car2= new Car();
		Console.WriteLine(Car.Counter());
		
		
	}

	
}

abstract class Child
{
	protected int value;
	protected string name;
	public Child(int value, string name)
	{
		this.value = value;
		this.name = name;
	}
	public abstract string CheckHobby();
	
}

class Sk8rBoi : Child
{
	public Sk8rBoi(int value, string name) : base(value, name){}
	
	public override string CheckHobby()
	{
		return "Shreddin n Grindin";
	}
	
	
	
}

class BalletGirl : Child
{
	public BalletGirl(int value, string name) : base(value, name){}
	public override string CheckHobby()
	{
		return "Black Swan starring Natalie Portman";
	}
}

class HobbyChecker
{
	public void DisplayHobby(Child child)
	{
		Console.WriteLine(child.CheckHobby());
	}
}


public interface IRizz
{
	void Rizz();
}

public interface IGyatt
{
	void Gyatt();
}

public interface IFanumTax
{
	void FanumTax();
}

public interface ISkibidi : IRizz, IFanumTax, IGyatt {}
public class Rizzler : IRizz
{
	public void Rizz ()
	{
		Console.WriteLine("Rizz");
	}	
}

class Car
{
	public static int count = 0;
	public Car()
	{
		count++;
	}
	
	public static int Counter()
	{
		Console.WriteLine("Vroom");
		return count;
	}
}