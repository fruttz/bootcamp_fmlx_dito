public enum Controls
{
	UP,
	DOWN,
	LEFT,
	RIGHT
}


class Program
{
	public delegate int MyDelegate(int x, int y);
	public static void Main(string[] args)
	{
		MyDelegate del = Add;
		del += Mul;
		
		int result = del(3,4);
		int result2 = del.Invoke(3,4);
		
		result.Print();
		result2.Print();
		
		
	}
	
	static int Add(int x, int y)
	{
		return x + y;
	}
	
	static int Mul(int x, int y)
	{
		return x * y;
	}
}

class Controller
{
	private Controls _direction;
	public Controller(Controls direction)
	{
		this._direction = direction;
	}
	
	public Controls GetDirection()
	{
		return this._direction;
	}
}

class MyCollection<T>
{
	public T[] values;
	public int count
	;
	public MyCollection(int size)
	{
		values = new T[size];
		count = -1;	
	}
	
	public bool AddObject(T obj)
	{
		if (count == values.Length - 1)
		{
			return false;
		}
		count++;
		values[count] = obj;
		return true;
	}
	
	public T GetData(int index)
	{
		if (index > values.Length - 1)
		{
			;
		}
		return values[index];
	}
	
	public int GetLenght()
	{
		return this.values.Length;
	}
}

public static class ExtensionClass
{
	public static void Print(this object message)
	{
		Console.WriteLine(message);
	}
}