public class User {
    public string Name { get; }
    public string Email { get; }
    public string Password { get; private set; }
    public DateTime Birthday { get; }
    public Wallet ActiveWallet { get; private set; }
    private readonly List<Wallet> wallets = new List<Wallet>();

    public User(string name, string email, string password, DateTime birthday) {
        Name = name;
        Email = email;
        Password = password;
        Birthday = birthday;
    }

    internal void SetPassword(string hashedPassword) {
        Password = hashedPassword;
    }

    public void CreateNewWallet(string name, string currency, decimal initialBalance) {
        ValidateNewWalletParams(name, initialBalance);
        var newWallet = new Wallet(name, currency, initialBalance);
        Wallets.Add(newWallet);
        ActiveWallet = newWallet;
    }

    public void CreateWallet(string name, string currency, decimal initialBalance) {
        ValidateNewWalletParams(name, initialBalance);
        var newWallet = new Wallet(name, currency, initialBalance);
        Wallets.Add(newWallet);
        ActiveWallet = newWallet;
    }

    public List<Wallet> GetWallets() {
        return Wallets;
    }

    private void ValidateNewWalletParams(string name, decimal initialBalance) {
        if (string.IsNullOrEmpty(name)) {
            throw new ArgumentException("Wallet name cannot be null or empty.", nameof(name));
        }

        if (initialBalance < 0) {
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));
        }
    }

    public void SetActiveWallet(Wallet activeWallet) {
        ActiveWallet = activeWallet;
    }

    public void AddIncome() {
        Console.WriteLine("Add Income:");
        string description = GetStringInput("Enter a description: ");
        decimal amount = GetDecimalInput("Enter the income amount: ");
        DateTime date = GetDateInput("Enter the date (yyyy-mm-dd): ");

        if (ActiveWallet != null) {
            Category category = ChooseCategory("Choose income category: ", true); 
            ActiveWallet.AddIncome(description, amount, date, category);
        } else {
            Console.WriteLine("No active wallet. Please create or choose a wallet first.");
        }
    }

    public void AddExpense() {
        Console.WriteLine("Add Expense:");
        string description = GetStringInput("Enter a description: ");
        decimal amount = GetDecimalInput("Enter the expense amount: ");
        DateTime date = GetDateInput("Enter the date (yyyy-mm-dd): ");

        if (ActiveWallet != null) {
            Category category = ChooseCategory("Choose expense category: ", false); 
            ActiveWallet.AddExpense(description, amount, date, category);
        } else {
            Console.WriteLine("No active wallet. Please create or choose a wallet first.");
        }
    }

    private Category ChooseCategory(string prompt, bool isIncome) {
        Console.WriteLine($"{prompt}");

        if (isIncome) {
            Console.WriteLine("1. Salary\n2. Scholarship\n3. Other");
        } else {
            Console.WriteLine("1. Food\n2. Restaurants\n3. Medicine\n4. Sport\n5. Taxi\n6. Rent\n7. Investments\n8. Clothes\n9. Fun\n10. Other");
        }

        int choice = GetIntInput("Enter the corresponding number: ", 1, isIncome ? 3 : 10);

        switch (choice) {
            case 1: return new Category(isIncome ? "Salary" : "Food");
            case 2: return new Category(isIncome ? "Scholarship" : "Restaurants");
            case 3: return new Category(isIncome ? "Other" : "Medicine");
            case 4: return new Category("Sport");
            case 5: return new Category("Taxi");
            case 6: return new Category("Rent");
            case 7: return new Category("Investments");
            case 8: return new Category("Clothes");
            case 9: return new Category("Fun");
            case 10: return new Category("Other");
            default: throw new ArgumentOutOfRangeException(nameof(choice), "Invalid category choice.");
        }
    }

    private int GetIntInput(string prompt, int minValue, int maxValue) {
        int input;
        do {
            Console.Write(prompt);
        } while (!int.TryParse(Console.ReadLine(), out input) || input < minValue || input > maxValue);

        return input;
    }

    private decimal GetDecimalInput(string prompt) {
        decimal input;
        do {
            Console.Write(prompt);
        } while (!decimal.TryParse(Console.ReadLine(), out input));

        return input;
    }

    private string GetStringInput(string prompt) {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    private DateTime GetDateInput(string prompt) {
        DateTime input;
        do {
            Console.Write(prompt);
        } while (!DateTime.TryParse(Console.ReadLine(), out input));

        return input;
    }

    public List<Wallet> Wallets => wallets;

    public void DeleteWallet(Wallet walletToDelete) {
        wallets.Remove(walletToDelete);
    }


}

