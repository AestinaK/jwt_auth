namespace jwt.Models;

public class GenericModel
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}