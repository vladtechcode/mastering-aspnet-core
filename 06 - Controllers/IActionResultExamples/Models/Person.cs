namespace IActionResultExamples.Models;

public class Person(string firstName, string lastName, int age)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? FirstName { get; set; }= firstName;
    public string? LastName { get; set; }= lastName;
    public int? Age { get; set; } = age;
    
}