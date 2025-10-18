// LOGIC/TEST CODE

// Instantiate StoreManager and add initial objects
StoreManager storeManager = StoreManager.Instance;

// Create and add Employees
Employee emp1 = new Employee(
    id: 1, 
    name: "Steve Jobs", 
    salary: 3000);
Employee emp2 = new Employee(
    id: 2, 
    name: "Alan Turing", 
    salary: 3500);
storeManager.AddObject(emp1);
storeManager.AddObject(emp2);

// Create and add Customers
Customer cust1 = new Customer(
    id: 3, 
    name: "Bill Gates", 
    budget: 5000);
Customer cust2 = new Customer(
    id: 4, 
    name: "Ada Lovelace", 
    budget: 700);
storeManager.AddObject(cust1);
storeManager.AddObject(cust2);

// Create and add Items (Laptop, Tablet, Smartphone)
Laptop laptop = new Laptop(
    id: 5, 
    name: "Dell XPS", 
    brand: "Dell", 
    itemPrice: 1200, 
    recommendedFor: "Programming", 
    quantity: 5
);
Tablet tablet = new Tablet(
    id: 6, 
    name: "iPad Pro", 
    brand: "Apple", 
    itemPrice: 800, 
    size: 12, 
    quantity: 3
);
Smartphone smartphone = new Smartphone(
    id: 7, 
    name: "Galaxy S21", 
    brand: "Samsung", 
    itemPrice: 900, 
    batteryLife: "24 hours", 
    quantity: 2
);
storeManager.AddObject(laptop);
storeManager.AddObject(tablet);
storeManager.AddObject(smartphone);

// Display all objects in the store
Console.WriteLine("\nDisplaying all objects in the store:");
storeManager.DisplayObjects();

// Test FindObject method
Console.WriteLine("\nFinding an object by name 'Dell XPS':");
StoreObject? foundObject = storeManager.FindObject("Dell XPS");
if (foundObject != null)
{
    Console.WriteLine($"Found: {foundObject.GetType().Name} - {foundObject.PublicName}");
}

// Testing FindType<Item> to retrieve all items in the store
Console.WriteLine("\nTesting FindType<Item>:");
List<Item> items = storeManager.FindType<Item>();

foreach (var item in items)
{
    Console.WriteLine($"Found Item - Type: {item.GetType().Name}, Name: {item.PublicName}, Price: {item.Price}");
}

// Display details of employees
Console.WriteLine("\nEmployee details:");
emp1.DisplayInfo();
emp2.DisplayInfo();

// Employee Login and Logout
Console.WriteLine("\nTesting Employee login/logout:");
emp1.Login();
emp1.LogOut();

// Display details of customers
Console.WriteLine("\nCustomer details:");
cust1.DisplayInfo();
cust2.DisplayInfo();

// Test Customer Purchase
Console.WriteLine("\nTesting customer purchase:");
cust1.MakePurchase(laptop);  // Success (within budget)
cust2.MakePurchase(tablet);   // Fail (out of budget)

// Remove an object
Console.WriteLine("\nRemoving 'Alan Turing' (Employee) from store.");
storeManager.RemoveObject(emp2);
Console.WriteLine("Goodbye Alan.");

// Display all objects in the store after removal
Console.WriteLine("\nDisplaying all objects in the store after removal:");
storeManager.DisplayObjects();

// Testing discounts on items
Console.WriteLine("\nTesting discounts on items:");

int laptopDiscount = 15; 
int tabletDiscount = 10;
int smartphoneDiscount = 25;

Console.WriteLine($"Applying discounts to items:");

laptop.CalculateDiscount(laptopDiscount);
tablet.CalculateDiscount(tabletDiscount);
smartphone.CalculateDiscount(smartphoneDiscount);
Console.WriteLine();


// LIBRARY CODE

// Singleton pattern for StoreManager
public class StoreManager {
    private static StoreManager _instance;
    public static StoreManager Instance {
        get {
            if (_instance == null) {
                _instance = new StoreManager();
            }
            return _instance;
        }
    }
    
    private List<StoreObject> ObjectsList = new List<StoreObject>();

    // Method to find objects by type
    public List<T> FindType<T>() where T : class {
        List<T> filteredList = new List<T>();
        foreach (var item in ObjectsList) {
            if (item is T t) {
                filteredList.Add(t);
            }
        }
        return filteredList;
    }

    // Method to find object by name
    public StoreObject? FindObject(string name) {
        foreach (var item in ObjectsList) {
            if (item.PublicName == name) {
                return item;
            }
        }
        return null;
    }

    // Method to add and remove objects
    public void AddObject(StoreObject obj) => ObjectsList.Add(obj);
    public void RemoveObject(StoreObject obj) => ObjectsList.Remove(obj);

    // Method to display all objects with their type and name
    public void DisplayObjects() {
        foreach (var obj in ObjectsList) {
            string objectType = obj.GetType().Name;
            Console.WriteLine($"{objectType}: {obj.PublicName}");
        }
    }
}

// Abstract base class for store items and people
public abstract class StoreObject {
    protected int Id;
    protected string Name;
    
    public StoreObject(int id, string name) {
        Id = id;
        Name = name;
    }

    public string PublicName => Name;
}

// Interface for availability check
public interface IAvailable {
    bool CheckAvailability();
}

// Interface for displaying information
public interface IInfo {
    void DisplayInfo();
}

// Base class for all items in the store
public abstract class Item : StoreObject, IAvailable {
    protected string Brand;
    private int ItemPrice;
    private bool IsAvailable;
    private int Quantity;

    public int Price {
        get { return ItemPrice; }
    }

    public void Sell() => Quantity = Quantity - 1;

    public Item(int id, string name, string brand, int itemPrice, int quantity, bool isAvailable = true)
        : base(id, name) {
        Brand = brand;
        ItemPrice = itemPrice;
        IsAvailable = isAvailable;
        Quantity = quantity;
    }

    public bool CheckAvailability() {
        return IsAvailable && Quantity > 0;
    }

    public abstract int CalculateDiscount(int discount);
}

// Laptop, Tablet, Smartphone classes implementing IInfo
public class Laptop : Item, IInfo {
    private string RecommendedFor;

    public Laptop(int id, string name, string brand, int itemPrice, string recommendedFor, int quantity, bool isAvailable = true)
        : base(id, name, brand, itemPrice, quantity, isAvailable) {
        RecommendedFor = recommendedFor;
    }

    public void DisplayInfo() {
        Console.WriteLine($"Laptop {Name}: Brand {Brand}, Recommended for {RecommendedFor}");
    }

    public override int CalculateDiscount(int discount)
    {
        int NewPrice = Price * (100 - discount) / 100;
        Console.WriteLine($"Previous price of the laptop was {Price}. The new price with {discount}% discount is: {NewPrice}");
        return NewPrice;
    }
}

public class Tablet : Item, IInfo {
    private int Size;

    public Tablet(int id, string name, string brand, int itemPrice, int size, int quantity, bool isAvailable = true)
        : base(id, name, brand, itemPrice, quantity, isAvailable) {
        Size = size;
    }

    public void DisplayInfo() {
        Console.WriteLine($"Tablet {Name}: Size {Size}");
    }

    public override int CalculateDiscount(int discount)
    {
        int NewPrice = Price * (100 - discount) / 100;
        Console.WriteLine($"Previous price of the tablet was {Price}. The new price with {discount}% discount is: {NewPrice}");
        return NewPrice;
    }
}

public class Smartphone : Item, IInfo {
    private string BatteryLife;

    public Smartphone(int id, string name, string brand, int itemPrice, string batteryLife, int quantity, bool isAvailable = true)
        : base(id, name, brand, itemPrice, quantity, isAvailable) {
        BatteryLife = batteryLife;
    }

    public void DisplayInfo() {
        Console.WriteLine($"Smartphone {Name}: Battery life {BatteryLife}");
    }

    public override int CalculateDiscount(int discount)
    {
        int NewPrice = Price * (100 - discount) / 100;
        Console.WriteLine($"Previous price of the smartphone was {Price}. The new price with {discount}% discount is: {NewPrice}");
        return NewPrice;
    }
}

// Interface for login capability
public interface ILogin {
    void Login(); 
    void LogOut();
}

// Employee class implementing ILogin and IInfo
public class Employee : StoreObject, IInfo, ILogin {
    private int Salary;
    private bool LoggedIn;

    public Employee(int id, string name, int salary, bool loggedIn = false) 
        : base(id, name) {
        Salary = salary;
        LoggedIn = loggedIn;
    }

    public void DisplayInfo() {
        Console.WriteLine($"Employee {Name}: Salary {Salary}");
    }

    public void Login() {
        LoggedIn = true;
        Console.WriteLine($"{Name} logged in.");
    }

    public void LogOut() {
        LoggedIn = false;
        Console.WriteLine($"{Name} logged out.");
    }
}

// Interface for purchasing capability
public interface IPurchase {
    void MakePurchase(Item item);
}

// Customer class implementing IPurchase and IInfo
public class Customer : StoreObject, IPurchase, IInfo {
    private int Budget;
    private List<Item> CustomerItems = new List<Item>();

    public Customer(int id, string name, int budget) 
        : base(id, name) {
        Budget = budget;
    }

    public void DisplayInfo() {
        Console.WriteLine($"Customer {Name}: Budget {Budget}");
    }

    public void MakePurchase(Item item) {
        if (item.CheckAvailability() && Budget >= item.Price) {
            CustomerItems.Add(item);
            item.Sell();
            Budget -= item.Price;
            Console.WriteLine($"{Name} purchased {item.PublicName}");
        } else if (!item.CheckAvailability()){
            Console.WriteLine($"{item.PublicName} is not available");
        } else {
            Console.WriteLine($"Insufficient budget for {item.PublicName}. Missing {item.Price - Budget}.");
        }
    }
}