using System;
using System.Collections.Generic;
using System.Linq;

class Program {
    static void Main() {
        User? user = null;

        Console.WriteLine("Welcome to the Personal Finance Management System! \n------first Register than Login------");

        while (true) {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Register\n2. Login\n0. Exit");

            int initialChoice = GetIntInput("Enter your choice (0-2): ", 0, 2);

            switch (initialChoice) {
                case 1:
                    user = Register(); 
                    break;
                case 2:
                    while (Login(user)) {
                        Console.WriteLine("Invalid email or password. Please try again or register first.");
                    }
                    Console.WriteLine("Login !!!");
                    MainMenu(user); 
                    break;
                case 0:
                    Console.WriteLine("Exiting the Personal Finance Management System...");
                    return;
            }
        }
    }


    static void MainMenu(User user) {
        while (true) {
            if (user == null) {
                Console.WriteLine("User not logged in. Please log in or register first.");
                return;
            }

            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Create Wallet\n2. Choose Wallet\n3. Add Income\n4. Add Expense\n5. Delete Wallet\n6. View Statistics\n7. Log Out\n0. Exit");

            int mainChoice = GetIntInput("Enter your choice: ", 0, 7);

            switch (mainChoice) {
                case 1:
                    CreateWallet(user);
                    break;
                case 2:
                    ChooseActiveWallet(user);
                    break;
                case 3:
                    user.AddIncome();
                    break;
                case 4:
                    user.AddExpense();
                    break;
                case 5:
                    DeleteWallet(user);
                    break;
                case 6:
                    ViewStatistics(user);
                    break;
                case 7:
                    SystemService.RemoveActiveUser();
                    Console.WriteLine("Logged out successfully!");
                    return;
                case 0:
                    Console.WriteLine("\n Exiting the Personal Finance Management System...");
                    return;
            }
        }
    }

    static User? Register() {
        Console.WriteLine("Registration:");
        string name = GetStringInput("Enter your name: ");
        string email = GetStringInput("Enter your email: ");
        string password = GetStringInput("Enter your password: ");
        DateTime birthday = GetDateInput("Enter your birthday (yyyy-mm-dd): ");
        Console.WriteLine("\n Register successfully! \n Now Login");
        return new User(name, email, password, birthday);
    }

    static void CreateWallet(User user) {
        Console.WriteLine("\nCreate Wallet:");
        string name = GetStringInput("Enter the wallet name: ");

        Console.WriteLine("Select Currency:");
        Console.WriteLine("1. Dollar ($)");
        Console.WriteLine("2. Euro (€)");
        Console.WriteLine("3. Ruble (₽)");

        int currencyChoice = GetIntInput("Enter your choice (1-3): ", 1, 3);

        string currency;
        switch (currencyChoice) {
            case 1:
                currency = "USD";
                break;
            case 2:
                currency = "EUR";
                break;
            case 3:
                currency = "RUB";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currencyChoice), "Invalid currency choice.");
        }

        decimal initialBalance = GetDecimalInput("Enter the initial balance: ");

        user.CreateWallet(name, currency, initialBalance);
        Console.WriteLine($"Wallet '{name}' created successfully!");
        ChooseActiveWallet(user);
    }

    static void ChooseActiveWallet(User user) {
        Console.WriteLine("\n Choose Active Wallet:");

        List<Wallet> userWallets = user.GetWallets();
        if (userWallets.Count == 0) {
            Console.WriteLine("\n No wallets available. Create a wallet first.");
            return;
        }

        Console.WriteLine("Wallet List:");
        for (int i = 0; i < userWallets.Count; i++) {
            Console.WriteLine($"{i + 1}. {userWallets[i].Name} ({userWallets[i].Currency})");
        }

        int walletChoice = GetIntInput("Choose a wallet (0 to cancel): ", 0, userWallets.Count);

        if (walletChoice > 0) {
            user.SetActiveWallet(userWallets[walletChoice - 1]);

            Console.WriteLine($"Wallet '{userWallets[walletChoice - 1].Name}' set as active.");
        }
    }

    static void DeleteWallet(User user) {
        Console.WriteLine("\nDelete Wallet:");

        List<Wallet> userWallets = user.GetWallets();
        if (userWallets.Count == 0) {
            Console.WriteLine("\n No wallets available. Create a wallet first.");
            return;
        }

        Console.WriteLine("Wallet List:");
        for (int i = 0; i < userWallets.Count; i++) {
            Console.WriteLine($"{i + 1}. {userWallets[i].Name} ({userWallets[i].Currency})");
        }

        int walletChoice = GetIntInput("Choose a wallet to delete (0 to cancel): ", 0, userWallets.Count);

        if (walletChoice > 0) {
            user.DeleteWallet(userWallets[walletChoice - 1]);
            Console.WriteLine($"Wallet '{userWallets[walletChoice - 1].Name}' deleted successfully.");
        }
    }

    static bool Login(User user) {
        Console.WriteLine("Login:");
        string enteredEmail = GetStringInput("Enter your email: ");
        string enteredPassword = GetStringInput("Enter your password: ");
        User authenticatedUser = AuthenticateUser(enteredEmail, enteredPassword);

        if (authenticatedUser != null) {
            SystemService.SaveActiveUser(authenticatedUser);
            Console.WriteLine("Login successful!");
            return true;
        }

        Console.WriteLine("\n :>");
        return false;
    }

    static User? AuthenticateUser(string enteredEmail, string enteredPassword) {
        foreach (User? registeredUser in RegisteredUsers) {
            if (registeredUser.Email == enteredEmail && CheckPassword(enteredPassword, registeredUser.Password)) {
                return registeredUser;
            }
        }

        return null;
    }

    static List<User> RegisteredUsers = new List<User>();

    static bool IsValidEmail(string email) {
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        } catch {
            return false;
        }
    }

    static bool CheckPassword(string enteredPassword, string hashedPassword) {
        return hashedPassword == GetHashedPassword(enteredPassword);
    }

    static int GetIntInput(string prompt, int minValue, int maxValue) {
        int input;
        do {
            Console.Write(prompt);
        } while (!int.TryParse(Console.ReadLine(), out input) || input < minValue || input > maxValue);

        return input;
    }

    static string GetStringInput(string prompt) {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static string GetHashedPassword(string prompt) {
        Console.Write(prompt);
        return Console.ReadLine(); 
    }

    static DateTime GetDateInput(string prompt) {
        DateTime input;
        do {
            Console.Write(prompt);
        } while (!DateTime.TryParse(Console.ReadLine(), out input));

        return input;
    }

    static decimal GetDecimalInput(string prompt) {
        decimal input;
        do {
            Console.Write(prompt);
        } while (!decimal.TryParse(Console.ReadLine(), out input));

        return input;
    }

    static void ViewStatistics(User user) {
        Console.WriteLine("View Statistics:");

        DateTime startDate = GetDateInput("Enter the start date (yyyy-mm-dd): ");
        DateTime endDate = GetDateInput("Enter the end date (yyyy-mm-dd): ");

        SystemService.CalculateStatistics(user, startDate, endDate);
    }


}

