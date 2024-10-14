using System.Threading.Channels;
using static DelegatesApp.Program;

namespace DelegatesApp
{
    /*Delegates in C#?
     
         What are Delegates?
             A delegate is a type that represents
             references to methods with a specific
             parameter list and return type.

        Why do we use Delegates?
             Delegates provide a way to pass methods
             as parameters, enabling flexible and
             extensible code designs.

        When do we use Delegates?
             Use delegates when you need a way to
             encapsulate a method and pass it as an
             argument.

        Where do we use Delegates?
             Delegates are used in callback
             mechanisms, event handling, and
             designing flexible APIs.

    
             Delegates define a method signature,
             and any method assigned to a delegate must 
             match this signature:
                   
                1. Declaration
                2. Instantiation
                3. Invocation

        ==============================================================
        Generics in C#?
         
        What are Generics?
             Generics are a way to make your code
             more flexible and reusable by allowing it to
             work with any data type.
             Think of generics as templates that you
             can fill in with different types, when you
             use them.

        Why would you use Generics?
             
        1. Flexibility. 
             You can write one method, class, or interface and use it 
             with different data types without writing multiple
             versions.

        2. Type Safety. 
                Generics help catch errors at
                compile time rather than at runtime,
                making your code safer.

         3. Performance
                Generics avoid the need for
                boxing and unboxing when working with
                value types, which can improve
                performance.
        =================================================================
        Multicast Delegates?

            What are Multicast Delegates in C#?
                 A multicast delegate in C# is a delegate
                 that holds references to and can invoke
                 multiple methods.

            Why use Multicast Delegates in C#?
                 You would use a multicast delegate to allow
                 multiple methods to be called in sequence
                 through a single delegate invocation.

            When use Multicast Delegates in C#?
                 You would use a multicast delegate when
                 you need to notify multiple event handlers
                 or execute multiple related methods in
                 response to a single event or operation.
        =================================================================
        Events in C#?

            What is an Event?
                 An event let's one class tell others when
                 something important happens. It uses a
                 special method called a delegate.
                 This means one part of the program can
                 alert others without needing direct
                 connections.

            Why use an Event?
                 Events allow a class to send updates
                 without knowing who gets them. This
                 makes the system more flexible and
                 organized.
                 It helps different parts of the program work
                 together without being tightly connected.

            When would we use an Event?
                 Use events when one object needs to
                 inform others about changes or actions.
                 It's useful for keeping things updated
                 without direct connections.
                 This can be important in many scenarios
                 where multiple parts need to stay in sync.

            Where would we use an Event?
                 Events are common in logging, monitoring,
                 data changes, and button clicks. They are
                 used wherever notifications are needed.
                 Any situation where one action triggers
                 other responses can benefit from events.


            Creating an event is useful if you want to execute code in 
            response to a specific action. 

            The key point is that many events already exist that we 
            didn’t write ourselves, but we can subscribe to them. 
            This allows us to be notified when those events happen.

            For example, when a user presses a key on the keyboard, 
            that can trigger an event, which in turn executes some 
            code in response.

    */

    //Ex4 declearing delegets inside namespase

    public delegate int Comparison<T> (T x, T y);

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }

    }

    public class PersonSorter
    {
        public void Sort(Person[] people, Comparison<Person> comparison)
        {
            for(int i = 0; i < people.Length -1; i++)
            {
                for(int j = i+1; j < people.Length; j++)
                {
                    //Comare people[i] and people[j] using the provided comparsion delegate
                    if (comparison(people[i], people[j]) > 0)
                    {
                        //Swap people i and people j if they are in the wrong order
                        Person temp = people[i];
                        people[i] = people[j];
                        people[j] = temp;
                    }
                }
            }
        }
    }
    //=========================================================
    //#Ex6 Intro to Events - Publishers and Subscribers

    public delegate void Notify(string message);

    public class EventPublisher
    {
        // The "On" prefix makes it immediately clear that the method
        // is associated with an event.
        //It signifies that the method is not just a regular method but
        //one that is called when a specific event occurs.

        public event Notify OnNotify;

        public void RaiseEvent(string message)
        {
            //Invoke the event if are there any Subscribers
            OnNotify?.Invoke(message);
        }
    }

    public class EventSubscribers
    {
        public void OnEventRaised(string message)
        {
            Console.WriteLine("Event received: " + message);
        }
    }
    //=========================================================
    //#Ex7 Real World example - Events - Temperature Monitor
    //public delegate void TemperatureChangedHandler(string message);

    //Ex8 Using the Generic delegat EventHandler<TEventArgs>
    public class TemperatureChangedEventArgs : EventArgs
    {

        //Property holding the Temperature
        public int Temperature { get; }

        //so if you want to set the Temperature you
        //can only do that by a constructor

        public TemperatureChangedEventArgs(int tem)
        {
            Temperature = tem;
        }
    }



    public class TemperatureMonitor
    {
        //create a genric event handler
        public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;

        //public event TemperatureChangedHandler OnTemperatureChanged;

        private int _temperature;

        //get: read the value and set: changed the value.
        public int Temperature
        {
            get { return _temperature; }

            set
            {

                if (_temperature != value)
                {
                    _temperature = value;
                    //Raise Event!
                    OnTemperatureChanged(new TemperatureChangedEventArgs(_temperature));

                }

            }
        }

        //the code that we want to execute
        protected virtual void OnTemperatureChanged(TemperatureChangedEventArgs e)
        {
            TemperatureChanged?.Invoke(this, e);
        }
    }

    //Sebscriber
    public class TemperatureAlert
    {
        public void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            Console.WriteLine($"Alert: temperatuew is {e.Temperature} sender is: {sender} ");
        }
    }

    public class TemperatureAlert2
    {
        public void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
        {
            Console.WriteLine($"Temp Cooling Alert: temperatuew is {e.Temperature} sender is: {sender} ");
        }
    }


    internal class Program
    {
        //#Ex1

        //#1. Delegates Declaration
        public delegate void Notify(string message);


        //2. Delegates Instantiation
        static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        //==========================================
        //Ex2 -> note that delegate have no body

        public delegate void LogHandler(string message);

        public class Logger
        {
            public void LogToConsole(string message)
            {
                Console.WriteLine("Console Log: " + message);
            }

            public void LogToFile(string message)
            {
                Console.WriteLine("File log: " + message);
            }

        }
        //=======================================================
        //#Ex3 -> Generics

        //this example is without using generics
        public static void PrintIntArray(int[] array)
        {
            foreach(int item in array)
            {
                Console.WriteLine(item);
            }
        }

        public static void PrintStringArray(string[] array)
        {
            foreach (string item in array)
            {
                Console.WriteLine(item);
            }
        }


        //Generics Method, that accepts a generics data types
        public static void PrintArray<T>(T[] array)
        {
            foreach (T item in array)
            {
                Console.WriteLine(item);
            }
        }
        //=========================================================



        static void Main(string[] args)
        {
            //#Ex1

            //Instantiation
            //Notify notifyDelegate = ShowMessage;

            //Invocation/calling
            //notifyDelegate("Hi there!");

            //==========================================
            //#Ex2 -> Another example of delegate
            /*Logger logger = new Logger();
            //assinging the methods
            LogHandler logHandler = logger.LogToConsole;

            logHandler("hi osame");

            //now we can assign logHandler to be the output of logFile
            logHandler = logger.LogToFile;
            logHandler("this is for file");*/

            //================================================
            //Ex3 -> Super quick intro to Generics
            /*int[] intArray = { 1, 2, 3, };
            string[] stringArray = { "one", "two", "three" };

            PrintArray(intArray);
            PrintArray(stringArray);*/

            //=================================================
            //#Ex4 -> Combining Generics with Delegates to make
            //a sorting algorithm

            /*Person[] person = {
                new Person { Name = "Osama", Age = 26 },
                new Person { Name = "Khaled", Age = 25 },
                new Person { Name = "Bander", Age = 27 },
                new Person { Name = "Albaydhani", Age = 30 }
            };

            PersonSorter sorter = new PersonSorter();
            
            sorter.Sort(person, CompareByAge);
            foreach(Person people in person)
            {
                Console.WriteLine($"{people.Name}, {people.Age}");
            }

            sorter.Sort(person, CompareByName);
            foreach (Person people in person)
            {
                Console.WriteLine($"{people.Name}, {people.Age}");
            }*/
            //=================================================
            //Ex5 -> Multicast Delegates and how to manage error on it
            //GetlnvocationList() seeing which methods are
            //in a multicast delegate

            /*Logger logger = new Logger();

            //In these two lines we used Multicast Delegates
            //The += operator is used to add a new method to
            //the existing list of methods already attached
            //to an event handler
            LogHandler logHandler = logger.LogToConsole;
            logHandler += logger.LogToFile;

            //invoking the multicast delegate (insted of this)
            logHandler("Log this info");


            //invoking the multicast delegate
            foreach (LogHandler handler in logHandler.GetInvocationList())
            {
                try
                {
                    handler("Event occurd with error handling");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caugt: " + ex.Message);
                }
            }

            if(IsMethodInDelegate(logHandler, logger.LogToFile))
            {
                logHandler -= logger.LogToFile;
                Console.WriteLine("LogToFile Method removed!");
            } else
            {
                Console.WriteLine("LgToFile Method not found!");
            }

            
            if(logHandler != null)
            {
                InvokeSafely(logHandler, "After removing LogToFile Safley!");
            }*/

            //============================================================
            //Ex6 Intro to Events - Publishers and Subscribers

            /*EventPublisher publisher = new EventPublisher();
            EventSubscribers subscriber  = new EventSubscribers();

            publisher.OnNotify += subscriber.OnEventRaised;

            publisher.RaiseEvent("Test");*/
            //=============================================================
            //Ex7 -> Real World example - Events - Temperature Monitor

            TemperatureMonitor monitor = new TemperatureMonitor();
            TemperatureAlert alert = new TemperatureAlert();
            TemperatureAlert2 alret2 = new TemperatureAlert2();

            /*How do you subscribe to an event in C#?
                
            By using the += operator followed by the event 
            handler method.
            */

            monitor.TemperatureChanged += alert.OnTemperatureChanged;
            monitor.TemperatureChanged += alert.OnTemperatureChanged;


            monitor.Temperature = 20;
            Console.WriteLine("Please enter the temperature");
            monitor.Temperature = int.Parse(Console.ReadLine());

            Console.ReadKey();
        }







        //Ex5 here is a method to invoke Multicast Delegates safely 
        static void InvokeSafely(LogHandler logHandler, string message)
        {
            LogHandler tempLogHandler = logHandler;
            if(tempLogHandler != null)
            {
                tempLogHandler(message);
            }
        }

        //another method to manage delegates by GetInvocationList()
        static bool IsMethodInDelegate(LogHandler logHandler, LogHandler method) {
            if (logHandler == null)
            {
                return false;
            }

            foreach(var d in logHandler.GetInvocationList())
            {
                if(d == (Delegate)method)
                {
                    return true;
                }
            }

            return false;
        }



        //Ex4
        static int CompareByAge(Person x, Person y)
        {
            return x.Age.CompareTo(y.Age);
        }

        static int CompareByName(Person x, Person y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}


//#Exrcise 1
/*
 using System; // Importing the System namespace
 
namespace Coding.Exercise // Defining the Coding.Exercise namespace
{
    // Define the delegate that will be used for the event
    public delegate void StockPriceChangedHandler(string message);
 
    // Define the Stock class which includes the event system
    public class Stock
    {
        // Declare the event using the delegate
        public event StockPriceChangedHandler OnStockPriceChanged;
 
        private decimal _price; // Private field to store the stock price
        private decimal _threshold; // Private field to store the threshold
 
        // Property to get and set the stock price
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value; // Set the new price
                // Raise the event if the price drops below the threshold
                if (_price < Threshold)
                {
                    RaiseStockPriceChangedEvent("Stock price is below threshold!");
                }
            }
        }
 
        // Property to get and set the alert threshold
        public decimal Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }
 
        // Method to raise the stock price changed event
        protected virtual void RaiseStockPriceChangedEvent(string message)
        {
            // Invoke the event
            OnStockPriceChanged?.Invoke(message);
        }
    }
 
    // Define the subscriber class which reacts to the event
    public class StockAlert
    {
        // Method that handles the event and prints a message to the console
        public void OnStockPriceChanged(string message)
        {
            Console.WriteLine("Stock Alert: " + message);
        }
    }
 
    // Program class to simulate the stock price changes and test the event system
    class Program
    {
        static void Main(string[] args)
        {
            // Create instances of Stock and StockAlert
            Stock stock = new Stock();
            StockAlert alert = new StockAlert();
 
            // Subscribe to the stock price changed event
            stock.OnStockPriceChanged += alert.OnStockPriceChanged;
 
            // Set the alert threshold
            stock.Threshold = 120m;
 
            // Simulate stock price changes
            stock.Price = 130m; // No alert
            stock.Price = 110m; // Alert triggered
 
            // Wait for user input to close the console
            Console.ReadKey();
        }
    }
 
 */