using BlazorApp1.Interfaces;
using BlazorApp1.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorApp1.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Load()
        {
            try
            {
                var turmas = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "turmas");
                var professores = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "professores");
                var alunos = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "alunos");
                var calendarios = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "calendarios");
                var anotacoes = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "anotacoes");

                if (!string.IsNullOrEmpty(turmas) && JsonSerializer.Deserialize<List<TurmaModel>>(turmas) is List<TurmaModel> listaTurmas)
                    Dados.Turmas = listaTurmas;
                if (!string.IsNullOrEmpty(professores) && JsonSerializer.Deserialize<List<ProfessorModel>>(professores) is List<ProfessorModel> listaProfessores)
                    Dados.Professores = listaProfessores;
                if (!string.IsNullOrEmpty(alunos) && JsonSerializer.Deserialize<List<AlunoModel>>(alunos) is List<AlunoModel> listaAlunos)
                    Dados.Alunos = listaAlunos;
                if (!string.IsNullOrEmpty(calendarios) && JsonSerializer.Deserialize<List<CalendarioModel>>(calendarios) is List<CalendarioModel> listaCalendarios)
                    Dados.Calendarios = listaCalendarios;
                if (!string.IsNullOrEmpty(anotacoes) && JsonSerializer.Deserialize<List<AnotacaoModel>>(anotacoes) is List<AnotacaoModel> listaAnotacoes)
                    Dados.Anotacoes = listaAnotacoes;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                string turmasJson = JsonSerializer.Serialize(Dados.Turmas);
                string professoresJson = JsonSerializer.Serialize(Dados.Professores);
                string alunosJson = JsonSerializer.Serialize(Dados.Alunos);
                string calendariosJson = JsonSerializer.Serialize(Dados.Calendarios);
                string anotacoesJson = JsonSerializer.Serialize(Dados.Anotacoes);

                var turmas = await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "turmas", turmasJson);
                var professores = await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "professores", professoresJson);
                var alunos = await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "alunos", alunosJson);
                var calendarios = await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "calendarios", calendariosJson);
                var anotacoes = await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "anotacoes", anotacoesJson);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
