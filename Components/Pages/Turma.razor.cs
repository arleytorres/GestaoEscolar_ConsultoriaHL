using BlazorApp1.Components.ViewElements;
using BlazorApp1.Interfaces;
using BlazorApp1.Models;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Components.Pages
{
    public partial class Turma
    {
        // VARS
        private readonly int[] AnoDaTurma = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public string[] modalValues = { "", "Selecionar" };
        private AlertModel? AlertaPopup { get; set; }

        private void Adicionar_btn()
        {
            var localService = new LocalStorageService(JSRuntime);
            var service = new EscolaService(localService);
            var result = service.AddTurma(modalValues[1], modalValues[0]);

            modalValues = new string[] { "", "Selecionar" };

            if (!result.Success)
            {
                AlertaPopup = new AlertModel(result.Message, PopupType.Danger);
                return;
            }

            AlertaPopup = new AlertModel(result.Message, PopupType.Success);
        }

        private void AtualizarNome(ChangeEventArgs e)
        {
            modalValues[0] = e.Value?.ToString();
        }

        private void GetComboClick(object param)
        {
            if (param is not int ano)
                return;

            modalValues[1] = ano + "° ano";
        }

        private void GoToTurma(int turmaId)
        {
            NavManager.NavigateTo($"/turmaSub/{turmaId}/");
        }
    }
}
