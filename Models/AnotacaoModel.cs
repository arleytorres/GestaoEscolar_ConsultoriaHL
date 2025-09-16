namespace BlazorApp1.Models
{
    public class AnotacaoModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime data { get; private set; }
        public Guid turmaId { get; private set; }
        public Guid professorId { get; private set; }
        public string anotacao { get; set; }
        public ProfessorMaterias materia { get; private set; }
        public DateTime dataCriacao { get; init; } = DateTime.Now;

        public AnotacaoModel(Guid professor, Guid turma, DateTime dia, ProfessorMaterias mat, string txt)
        {
            turmaId = turma;
            professorId = professor;
            data = dia;
            materia = mat;
            anotacao = txt;
        }
    }
}
