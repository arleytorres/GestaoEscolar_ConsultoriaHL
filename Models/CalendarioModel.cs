namespace BlazorApp1.Models
{
    public class CalendarioModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int Posicao { get; set; }
        public Guid Turma { get; set; }
        public ProfessorMaterias Materia { get; set; }
        public string Dia { get; set; } = string.Empty;

        public CalendarioModel(int pos, Guid turma, ProfessorMaterias materia, string day)
        {
            Posicao = pos;
            Turma = turma;
            Materia = materia;
            Dia = day;
        }
    }
}
