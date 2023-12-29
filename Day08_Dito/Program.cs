using System.Collections;

class Program
{
	public static void Main(string[] args)
	{
		MyEnumeration enumeration = new MyEnumeration();
		enumeration.Run();
	}
}

//EXCEPTION HANDLING
class Car
{
	private string _brand = null;
	public int GetBrandCount()
	{
		try
		{
			return _brand.Length;
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message + "Bozo");
			return 0;
		}
	}
}

//LAMDA EXPRESSION
class Lambda
{
	Func<int,int,int> myFunc = (int a, int b) => { return a + b;};
	
	public void Run(int a, int b)
	{
		Console.WriteLine(myFunc(a,b));		
	}
}

//ENUMERABLE
class MyEnumeration
{
	int[] intArray = {1,2,3,4,5};
	public void Run()
	{
		IEnumerator enumerator = intArray.GetEnumerator();
		enumerator.MoveNext();
		for(int i = 0; i < intArray.Length; i++)
		{
			Console.WriteLine(enumerator.Current);
			enumerator.MoveNext();
		}
		enumerator.Reset();
		
	}
}