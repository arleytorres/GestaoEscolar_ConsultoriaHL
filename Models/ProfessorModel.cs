using System.Security.Cryptography.Xml;

namespace BlazorApp1.Models
{
    public enum ProfessorMaterias { Portugues = 0, Matematica = 1, Historia = 2, Geografia = 3, Artes = 4, Biologia = 5, Filosofia = 6, Sociologia = 7, EdFisica = 8 }

    public class ProfessorModel
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<(Guid turmaId, ProfessorMaterias materia)> Turmas = new List<(Guid, ProfessorMaterias)>();

        public ProfessorModel(string name)
        {
            Name = name;
        }
        public ProfessorModel(string name, Guid turma, ProfessorMaterias materia)
        {
            Name = name;
            Turmas.Add((turma, materia));
        }
    }
}