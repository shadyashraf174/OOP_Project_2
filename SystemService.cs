using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;
public class SystemService {

    private static User? activeUser;
    private static Wallet? activeWallet;
    private readonly ILogger logger;

    public SystemService(ILogger logger) {
        this.logger = logger;
    }

    public static void HashPassword(User user) {
        using (SHA256 sha256 = SHA256.Create()) {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            user.SetPassword(BitConverter.ToString(hashedBytes).Replace("-", "").ToLower());
        }
    }

    public static void SaveActiveUser(User user) {
        activeUser = user;
    }

    public static User GetActiveUser() {
        return activeUser;
    }

    public static void SaveActiveWallet(Wallet wallet) {
        activeWallet = wallet;
    }

    public static Wallet GetActiveWallet() {
        return activeWallet;
    }

    public static void RemoveActiveUser() {
        activeUser = null;
    }

    public static void CalculateStatistics(User user, DateTime startDate, DateTime endDate) {
        if (user == null || user.ActiveWallet == null) {
            Console.WriteLine("User or active wallet not available.");
            return;
        }

        Wallet activeWallet = user.ActiveWallet;

        Console.WriteLine($"Calculating statistics for {user.Name} from {startDate.ToShortDateString()} to {endDate.ToShortDateString()}...");

        List<Operation> operationsInRange = GetOperationsInRange(activeWallet, startDate, endDate);

        decimal totalIncome = CalculateTotalIncome(operationsInRange);
        decimal totalExpense = CalculateTotalExpense(operationsInRange);

<<<<<<< HEAD
=======
        
>>>>>>> d50c86d0fd3d8e03398ba520dbf836d606da529e
        decimal totalInitialBalance = activeWallet.InitialBalance;

        Console.WriteLine($"Initial Balance: {totalInitialBalance}");
        Console.WriteLine($"Total Income: {totalIncome}");
        Console.WriteLine($"Total Expense: {totalExpense}");
        Console.WriteLine($"Net Income: {totalIncome - totalExpense + totalInitialBalance}");
    }

    private static List<Operation> GetOperationsInRange(Wallet wallet, DateTime startDate, DateTime endDate) {
        List<Operation> operationsInRange = new List<Operation>();

        foreach (var operation in wallet.Operations) {
            if (operation.Date >= startDate && operation.Date <= endDate) {
                operationsInRange.Add(operation);
            }
        }

        return operationsInRange;
    }

    private static decimal CalculateTotalIncome(List<Operation> operations) {
        return operations.Where(o => o.Amount > 0).Sum(o => o.Amount);
    }

    private static decimal CalculateTotalExpense(List<Operation> operations) {
        return operations.Where(o => o.Amount < 0).Sum(o => Math.Abs(o.Amount));
    }
}
