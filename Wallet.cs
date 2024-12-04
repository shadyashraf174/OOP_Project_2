using System;
using System.Collections.Generic;
using System.Text;

public class Wallet {
    public string Name { get; }
    public string Currency { get; }
    private decimal balance;
    public decimal InitialBalance { get; }
    public decimal Balance => balance;
    public List<Operation> Operations => operations;

    private readonly List<Operation> operations = new List<Operation>();

    public Wallet(string name, string currency, decimal initialBalance) {
        ValidateInitialBalance(initialBalance);

        Name = name;
        Currency = currency;
        InitialBalance = initialBalance;
        balance = initialBalance;
    }

    private void ValidateInitialBalance(decimal initialBalance) {
        if (initialBalance < 0) {
            throw new ArgumentException("Initial balance cannot be negative.");
        }
    }

    public void RecordIncome(string description, decimal amount, DateTime date, Category category) {
        operations.Add(new Operation(description, amount, date, category));
        UpdateBalance(amount);
    }

    public void RecordExpense(string description, decimal amount, DateTime date, Category category) {
        if (amount < 0) {
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        }

        operations.Add(new Operation(description, -amount, date, category));
        UpdateBalance(-amount);
    }

    public void AddIncome(string description, decimal amount, DateTime date, Category category) {
        Operations.Add(new Operation(description, amount, date, category));
        UpdateBalance(amount);
    }

    public void AddExpense(string description, decimal amount, DateTime date, Category category) {
        Operations.Add(new Operation(description, -amount, date, category));
        UpdateBalance(-amount);
    }

    public void DisplayWalletInfo() {
        Console.WriteLine($"Wallet Name: {Name}");
        Console.WriteLine($"Currency: {Currency}");
        Console.WriteLine($"Balance: {Balance}");
    }

    public string GetWalletInfo() {
        return $"Wallet Name: {Name}\nCurrency: {Currency}\nBalance: {Balance}";
    }

    public string GetWalletOperations() {
        var result = new StringBuilder($"Wallet Operations for {Name}:\n");
        foreach (var operation in Operations) {
            result.AppendLine($"{operation.Date.ToShortDateString()} - {operation.Description}: {operation.Amount} {Currency}");
        }
        return result.ToString();
    }

    public void DeleteAllOperations() {
        operations.Clear();
        balance = InitialBalance;
        Console.WriteLine("All operations deleted successfully.");
    }

    private void UpdateBalance(decimal amount) {
        balance += amount;
    }
}
