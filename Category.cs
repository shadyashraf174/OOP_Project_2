
public class Category {
    public string Name { get; }

    public Category(string name) {
        ValidateName(name);
        Name = name;
    }

    private void ValidateName(string name) {
        if (string.IsNullOrEmpty(name)) {
            throw new ArgumentException("Category name cannot be null or empty.");
        }
    }
}


