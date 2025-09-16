using BlazorApp1.Interfaces;
using BlazorApp1.Models;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace BlazorApp1.Components.Pages
{
    public partial class DiarioSub
    {
        [Parameter]
        public int professorId { get; set; }
        [Parameter]
        public int turmaId { get; set; }
        [Parameter]
        public int materiaId { get; set; }

        private ILocalStorageService localService { get; set; }
        private IEscolaService service { get; set; }
        private ProfessorModel professor { get; set; }
        private TurmaModel ThisTurma { get; set; }
        private ProfessorMaterias materia { get; set; }
        private string anotacaoShow = "";

        private DateTime dataAtual = DateTime.Now;
        private string mesAtualFormatado = "";
        private List<List<DateTime?>> semanasDoMes = new List<List<DateTime?>>();

        private int dayInMonth = 0;
        private int indexOfAlunos = 1;

        protected override void OnInitialized()
        {
            GerarCalendario(dataAtual.Year, dataAtual.Month);

            if (Dados.Professores[professorId] is ProfessorModel _professor)
                professor = _professor;

            if (Dados.Turmas[turmaId] is TurmaModel turma)
                ThisTurma = turma;

            materia = (ProfessorMaterias)materiaId;

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                localService = new LocalStorageService(JSRuntime);
                service = new EscolaService(localService);
            }

            indexOfAlunos = 1;
            await base.OnAfterRenderAsync(firstRender);
        }

        private void GerarCalendario(int ano, int mes)
        {
            var cultura = new CultureInfo("pt-BR");
            dataAtual = new DateTime(ano, mes, 1);

            mesAtualFormatado = cultura.TextInfo.ToTitleCase(dataAtual.ToString("MMMM", cultura));

            semanasDoMes.Clear();

            var primeiroDiaDoMes = new DateTime(ano, mes, 1);
            var diasNoMes = DateTime.DaysInMonth(ano, mes);

            int diasEmBranco = (int)primeiroDiaDoMes.DayOfWeek;

            var semanaAtual = new List<DateTime?>();

            for (int i = 0; i < diasEmBranco; i++)
            {
                semanaAtual.Add(null);
            }

            for (int dia = 1; dia <= diasNoMes; dia++)
            {
                semanaAtual.Add(new DateTime(ano, mes, dia));

                if (semanaAtual.Count == 7 || dia == diasNoMes)
                {
                    semanasDoMes.Add(semanaAtual);
                    semanaAtual = new List<DateTime?>();
                }
            }
        }

        private void OnButtonClick(int dia)
        {
            indexOfAlunos = 1;
            dayInMonth = dia;
            anotacaoShow = Dados.Anotacoes.FirstOrDefault(x => x.turmaId.Equals(ThisTurma.Id) && x.materia.Equals(materia) && x.data.Equals(new DateTime(dataAtual.Year, dataAtual.Month, dayInMonth))) is AnotacaoModel anotacaoModel ? anotacaoModel.anotacao : "";
        }

        private void OnTextEnter(ChangeEventArgs e)
        {
            service.AddAnotacao(professor, ThisTurma, materia, e.Value.ToString(), new DateTime(dataAtual.Year, dataAtual.Month, dayInMonth));
            anotacaoShow = e.Value.ToString();
        }
    }
}
