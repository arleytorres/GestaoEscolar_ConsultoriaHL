using BlazorApp1.Models;

namespace BlazorApp1.Components.Pages
{
    public partial class Diario
    {
        private ProfessorModel selectedProfessor = null;

        private void OnButtonClick(string btn, Guid professorId)
        {
            if (Dados.Professores.FirstOrDefault(x => x.Id == professorId) is not ProfessorModel professor)
                return;
            selectedProfessor = professor;
        }

        private void GotoTurmaDetails(int professorId, int turmaId, int materia)
        {
            NavManager.NavigateTo($"/diarioSub/{professorId}/{turmaId}/{materia}");
        }
    }
}
