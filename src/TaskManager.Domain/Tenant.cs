namespace TaskManager.Domain;

public class Tenant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Domain { get; set; }
}