namespace Heiwase.App.Infrasturcture.Entities;

public class HallOfFameMember
{
    public string Name { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public List<string> Description { get; set; } = [ ];
    public string ImagePath { get; set; } = String.Empty;
}
