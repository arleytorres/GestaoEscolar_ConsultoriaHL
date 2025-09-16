namespace BlazorApp1.Models
{
    public class TurmaModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int Ano { get; set; }
        public string turma { get; set; }
        public List<Guid> Alunos { get; set; } = new List<Guid>();

        public TurmaModel(int ano, string _turma)
        {
            Ano = ano;
            turma = _turma;
        }
    }
}
