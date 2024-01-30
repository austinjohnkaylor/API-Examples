namespace Core.Entities;

/// <summary>
/// Represents a book in Library's Catalog
/// </summary>
public class Book
{
    public long Id { get; set; } 
    public string Title { get; set; }
    public string Author { get; set; }
    public Guid ISBN { get; set; }
    public float Price { get; set; }
}