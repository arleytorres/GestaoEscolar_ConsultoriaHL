namespace BlazorApp1.Models
{
    public class AlunoModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }

        public AlunoModel(string name)
        {
            Name = name;
        }
    }
}