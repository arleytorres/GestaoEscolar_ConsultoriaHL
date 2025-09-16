using BlazorApp1.ContextResponse;
using BlazorApp1.Models;

namespace BlazorApp1.Interfaces
{
    public interface IEscolaService
    {
        IEscolaServiceResponse AddTurma(string ano, string turma);
        IEscolaServiceResponse DelTurma(Guid turmaId);
        IEscolaServiceResponse AddProfessor(Guid turmaId, string nome, string materia);
        IEscolaServiceResponse AddAluno(Guid turmaId, string nome);
        IEscolaServiceResponse AddHorario(Guid turmaId, string day, string ord, string disc);
        void AddAnotacao(ProfessorModel professor, TurmaModel turma, ProfessorMaterias materia, string anotacao, DateTime data);
    }
}
