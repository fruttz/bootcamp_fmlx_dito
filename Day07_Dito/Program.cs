class Program
{
	public static void Main(string[] args)
	{
		Subscriber sub1 = new Subscriber("Jumbo");
		Subscriber sub2 = new Subscriber("Yanda");
		
		Youtuber youtuber1 = new Youtuber("Yotsuba");
		youtuber1.mySub += sub1.GetNotifs;
		youtuber1.UploadVideo();
		
		Youtuber youtuber2 = new Youtuber("Koiwai");
		youtuber2.myFunc += sub2.GetNotifsFunc;
		youtuber2.UploadVideo();
		
		Publisher pub = new Publisher("Asagi");
		pub.mySub += sub1.GetNotifs;
		pub.myAction += sub2.GetNotifsAction;
		pub.UploadArticle();
	}
}

class Publisher
{
	private string _name;
	public Publisher(string name)
	{
		this._name = name;
	}
	public event EventHandler<MyEventArgs>? mySub;
	public event Action<int>? myAction;
	public void UploadArticle()
	{
		this.SendNotifs(25);
		this.SendNotifsAction(25);
	}
	
	public void SendNotifs(int values)
	{
		if (mySub != null)
		{
			mySub.Invoke(this, new MyEventArgs() {data = values});
		}
	}
	
	public void SendNotifsAction(int values)
	{
		if (myAction != null)
		{
			myAction.Invoke(values);
		}
	}
	
	public override string ToString()
	{
		return _name;
	}
}

class Youtuber
{
	private string _name;
	public Youtuber(string name)
	{
		this._name = name;
	}
	public event EventHandler<MyEventArgs>? mySub;
	public event Func<string, int>? myFunc;
	public void UploadVideo()
	{
		Console.WriteLine("Uploading...");
		this.SendNotifs("New Video!");
		this.SendNotifsFunc("25");
		
	}
	
	public void SendNotifs(string values)
	{
		if (mySub != null)
		{
			mySub.Invoke(this, new MyEventArgs() {message = values});	
		}
		
	}
	
	public void SendNotifsFunc(string message)
	{	
		if (myFunc != null)
		{
			Console.WriteLine("Func" + myFunc.Invoke(message));	
		}
		
	}
	
	public override string ToString()
	{
		return _name;
	}
	
}

class Subscriber
{
	private string _name;
	public Subscriber(string name)
	{
		this._name = name;
	}
	
	public void GetNotifs(object? sender, MyEventArgs e)
	{
		if(sender != null && sender.GetType().Name == "Youtuber")
		{
			Console.WriteLine($"{_name} get notified from " + sender.ToString() + " Message: " + e.message);
		}
		else if (sender != null && sender.GetType().Name == "Publisher")
		{
			Console.WriteLine($"{_name} get notified from " + sender.ToString() + " Message: " + e.data);
		}
		
	}
	
	public void GetNotifsAction(int values)
	{
		Console.WriteLine("Action Notif " + values);
	}
	
	public int GetNotifsFunc(string message)
	{
		return int.Parse(message);
	}
}

class MyEventArgs : EventArgs
{
	public string? message;
	public int? data;
}
