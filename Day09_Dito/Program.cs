//COLLECTION
using System.Collections;
using System.Collections.Generic;

class Program
{
	public static void Main(string[] args)
	{
		//Array
		//Fix Size
		//Type Safety
		//Accessible by Index
		int[] arrayInt = { 1, 2};
		int arrayResult = arrayInt[0];
		Console.WriteLine("array: " + arrayResult);
		
		//ArrayList
		//Not Type Safety
		//Dynamic Size
		//Accessible by Index
		ArrayList myArrayList = new ArrayList();
		myArrayList.Add(1);
		myArrayList.Add(2);
		var arrayListResult = myArrayList[0];
		Console.WriteLine("arraylist: " + arrayListResult);		
		
		//List<T>
		//Type Safety
		//Dynamic Size
		//Accessible by Index
		List<int> myList = new List<int>();
		myList.Add(3);
		myList.Add(4);
		int listResult = myList[0];
		Console.WriteLine("list:" + listResult);	
		
		//HashSet
		//Type Safety
		//Dynamic Size
		//Unique Values
		//Accessible by TryGetValue(index, out int value)
		HashSet<int> set = new HashSet<int>();
		set.Add(1);
		set.Add(2);
		bool condition = set.TryGetValue(1, out int hashSetResult);
		Console.WriteLine("hashset: " + hashSetResult);
		
		//Queue<T>
		//FIFO (First In First Out)
		Queue<int> queue = new Queue<int>();
		queue.Enqueue(1);
		queue.Enqueue(2);
		Console.WriteLine("queue: " + queue.Dequeue());
		Console.WriteLine("queue2: " + queue.Dequeue());
		
		//Stack<T>
		//LIFO (Last In First Out)
		Stack<int> stack = new Stack<int>();
		stack.Push(1);
		stack.Push(2);
		Console.WriteLine("stack: " + stack.Pop());
		Console.WriteLine("stack2: " + stack.Pop());
		
		//Dictionary<TKey, TValue>
		Dictionary<int, string> dictionary = new Dictionary<int,string>();
		dictionary.Add(1, "one");
		dictionary.Add(2, "two");
		
		foreach(KeyValuePair<int, string> pair in dictionary)
		{
			Console.WriteLine(pair.Key);
			Console.WriteLine(pair.Value);
		}
	}
}
