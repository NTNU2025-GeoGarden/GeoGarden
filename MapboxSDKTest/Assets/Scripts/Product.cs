public class Product
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    public Item productAsItem
    
    public Product(int id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }
}

public abstract class Products
{
    
}