using System;

public class Operation {
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Category Category { get; set; }

    public Operation(string description, decimal amount, DateTime date, Category category) {
        Description = description;
        Amount = amount;
        Date = date;
        Category = category;
    }
}
