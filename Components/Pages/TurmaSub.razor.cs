using BlazorApp1.Models;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Components.Pages
{
    public partial class TurmaSub
    {
        [Parameter]
        public int id { get; set; }
        private int incrementAluno = 1;
        private TurmaModel ThisTurma { get; set; }
        private List<CalendarioModel> Horarios { get; set; }
        private AlertModel AlertaPopup { get; set; }
        private string[][] modalValues = new string[][]
        {
            new string[] { "Selecionar", "Selecionar", "Selecionar" },
            new string[] { "", "Selecionar" },
            new string[] { "" }
        };

        private void OnButtonClick(string button)
        {
            var localService = new Services.LocalStorageService(JSRuntime);
            var service = new Services.EscolaService(localService);

            switch (button.ToLower())
            {
                case "horariobtn":
                    {
                        var response = service.AddHorario(ThisTurma.Id, modalValues[0][0], modalValues[0][1], modalValues[0][2]);

                        AlertaPopup = new AlertModel(response.Message, response.Success ? PopupType.Success : PopupType.Danger);

                        if (Dados.Turmas[id] is not TurmaModel turma)
                            return;

                        ThisTurma = turma;
                        Horarios = Dados.Calendarios.Where(x => x.Turma.Equals(turma.Id)).ToList();
                    }
                    break;
                case "professorbtn":
                    {
                        var result = service.AddProfessor(ThisTurma.Id, modalValues[1][0], modalValues[1][1]);

                        AlertaPopup = new AlertModel(result.Message, result.Success ? PopupType.Success : PopupType.Danger);

                        if (Dados.Turmas[id] is not TurmaModel turma)
                            return;
                        ThisTurma = turma;
                    }
                    break;
                case "estudantebtn":
                    {
                        var result = service.AddAluno(ThisTurma.Id, modalValues[2][0]);
                        AlertaPopup = new AlertModel(result.Message, result.Success ? PopupType.Success : PopupType.Danger);
                    }
                    break;

                case "delturmabtn":
                    {
                        var response = service.DelTurma(ThisTurma.Id);

                        if (!response.Success)
                        {
                            AlertaPopup = new AlertModel(response.Message, PopupType.Danger);
                            return;
                        }
                        ThisTurma = null;
                        NavManager.NavigateTo($"/turma");
                    }
                    break;
            }

        }

        private void inputTextChange(ChangeEventArgs e, string input)
        {
            switch (input.ToLower())
            {
                case "professor":
                    modalValues[1][0] = e.Value?.ToString();
                    break;
                case "estudante":
                    modalValues[2][0] = e.Value?.ToString();
                    break;
            }
        }

        private void GetComboClick(string dropbox, string param)
        {
            switch (dropbox.ToLower())
            {
                case "horario_1":
                    modalValues[0][0] = param;
                    break;
                case "horario_2":
                    modalValues[0][1] = param;
                    break;
                case "horario_3":
                    modalValues[0][2] = param;
                    break;

                case "professor_1":
                    modalValues[1][1] = param;
                    break;
            }
        }

        protected override void OnInitialized()
        {
            if (Dados.Turmas[id] is not TurmaModel turma)
                return;
            ThisTurma = turma;
            Horarios = Dados.Calendarios.Where(x => x.Turma.Equals(turma.Id)).ToList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            incrementAluno = 1;
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
