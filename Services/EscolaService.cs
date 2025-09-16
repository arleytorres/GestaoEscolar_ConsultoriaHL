using BlazorApp1.Components.Pages;
using BlazorApp1.ContextResponse;
using BlazorApp1.Interfaces;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Text.RegularExpressions;

namespace BlazorApp1.Services
{
    public class EscolaService : IEscolaService
    {
        private readonly ILocalStorageService _service;

        public EscolaService(ILocalStorageService service)
        {
            _service = service;
        }

        public IEscolaServiceResponse AddTurma(string ano, string turma)
        {
            try
            {
                if (ano.ToLower().Equals("selecionar"))
                    return new IEscolaServiceResponse(false, "Selecione o ano da turma.");

                int _ano = Convert.ToInt32(ano.Replace("° ano", string.Empty));

                if (!Regex.IsMatch(turma, "^[\\p{L}]+(?: [\\p{L}]+)*$"))
                    return new IEscolaServiceResponse(false, "O nome da turma contém caracteres inválidos.");

                if (turma.Length <= 3)
                    return new IEscolaServiceResponse(false, "O nome da turma é muito curto.");

                if (turma.Length >= 50)
                    return new IEscolaServiceResponse(false, "O nome da turma é muito longo.");

                if (Dados.Turmas.FirstOrDefault(x => x.Ano.Equals(ano) && x.turma.Equals(turma)) is not null)
                    return new IEscolaServiceResponse(false, "Esta turma já existe");

                Dados.Turmas.Add(new TurmaModel(_ano, turma));
                _service.Save();
                return new IEscolaServiceResponse(true, "Turma adicionada com sucesso");
            }
            catch (Exception ex)
            {
                return new IEscolaServiceResponse(false, $"Erro ao adicionar turma: {ex.Message}");
            }
        }
        public IEscolaServiceResponse DelTurma(Guid turmaId)
        {
            if (Dados.Turmas.FirstOrDefault(x => x.Id.Equals(turmaId)) is not TurmaModel turma)
                return new IEscolaServiceResponse(false, "O identificador da turma não é válido.");

            foreach (var professor in Dados.Professores)
                foreach (var aula in professor.Turmas.Where(x => x.turmaId.Equals(turmaId)).ToList())
                    professor.Turmas.Remove(aula);

            foreach (var calendario in Dados.Calendarios.Where(x => x.Turma.Equals(turmaId)).ToList())
                Dados.Calendarios.Remove(calendario);

            Dados.Turmas.Remove(turma);
            _service.Save();
            return new IEscolaServiceResponse(true, "Turma removida");
        }
        public  IEscolaServiceResponse AddProfessor(Guid turmaId, string nome, string materia)
        {
            if (Dados.Turmas.FirstOrDefault(x => x.Id.Equals(turmaId)) is not TurmaModel turma)
                return new IEscolaServiceResponse(false, "O identificador da turma não é válido.");

            if (!Regex.IsMatch(nome, "^[\\p{L}]+(?: [\\p{L}]+)*$"))
                return new IEscolaServiceResponse(false, "O nome do professor contém caracteres inválidos.");

            if (nome.Length <= 3)
                return new IEscolaServiceResponse(false, "O nome do professor é muito curto."); 
            
            if (nome.Length >= 40)
                return new IEscolaServiceResponse(false, "O nome do professor é muito longo.");

            if (!Enum.TryParse(materia, out ProfessorMaterias _materia))
                return new IEscolaServiceResponse(false, "O formato da Matéria inválida");

            if (Dados.Professores.Any(x => x.Turmas.Contains((turmaId, _materia))))
                return new IEscolaServiceResponse(false, "Um professor já leciona esta matéria na turma");

            var professor = Dados.Professores.FirstOrDefault(x => x.Name.Equals(nome));

            if (professor is null)
                Dados.Professores.Add(new ProfessorModel(nome, turmaId, _materia));
            else
                professor.Turmas.Add((turmaId, _materia));

            _service.Save();

            return new IEscolaServiceResponse(true, "Professor adicionado com sucesso");
        }
        public IEscolaServiceResponse AddHorario(Guid turmaId, string day, string ord, string disc)
        {
            try
            {
                if (Dados.Turmas.FirstOrDefault(x => x.Id.Equals(turmaId)) is not TurmaModel turma)
                    return new IEscolaServiceResponse(false, "O identificador da turma não é válido.");

                if (!int.TryParse(ord.Replace("° Aula", string.Empty), out int ordId))
                    return new IEscolaServiceResponse(false, "O identificador de aula não é válido.");

                if (!Enum.TryParse(disc, out ProfessorMaterias materia))
                    return new IEscolaServiceResponse(false, "O formato da Matéria inválida");

                if (Dados.Calendarios.FirstOrDefault(x => x.Turma.Equals(turma.Id) && x.Dia.Equals(day) && x.Posicao.Equals(ordId)) is not null)
                    return new IEscolaServiceResponse(false, "Este horário já existe");

                Dados.Calendarios.Add(new CalendarioModel(ordId, turma.Id, materia, day));
                _service.Save();

                return new IEscolaServiceResponse(true, $"Horário adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new IEscolaServiceResponse(false, $"Erro ao adicionar turma: {ex.Message}");
            }
        }

        public IEscolaServiceResponse AddAluno(Guid turmaId, string nome)
        {
            if (Dados.Turmas.FirstOrDefault(x => x.Id.Equals(turmaId)) is not TurmaModel turma)
                return new IEscolaServiceResponse(false, "O identificador da turma não é válido.");

            if (!Regex.IsMatch(nome, "^[\\p{L}]+(?: [\\p{L}]+)*$"))
                return new IEscolaServiceResponse(false, "O nome do estudante contém caracteres inválidos.");

            if (nome.Length <= 3)
                return new IEscolaServiceResponse(false, "O nome do estudante é muito curto.");

            if (nome.Length >= 40)
                return new IEscolaServiceResponse(false, "O nome do estudante é muito longo.");

            var aluno = Dados.Alunos.FirstOrDefault(x => x.Name.Equals(nome));

            if (aluno is AlunoModel && Dados.Turmas.SelectMany(x => x.Alunos).Contains(aluno.Id))
                return new IEscolaServiceResponse(false, "Este estudante já faz parte de uma turma");

            if (aluno is null)
            {
                aluno = new AlunoModel(nome);
                Dados.Alunos.Add(aluno);
                turma.Alunos.Add(aluno.Id);
            }
            else
                turma.Alunos.Add(aluno.Id);

            _service.Save();

            return new IEscolaServiceResponse(true, "Estudante adicionado com sucesso");
        }

        public void AddAnotacao(ProfessorModel professor, TurmaModel turma, ProfessorMaterias materia, string anotacao, DateTime data)
        {
            if (professor is null)
                return;

            if (turma is null)
                return;

            if (anotacao.Length >= 250)
                return;

            var checkAnotacao = Dados.Anotacoes.FirstOrDefault(x => x.turmaId.Equals(turma.Id) && x.materia.Equals(materia) && x.data.Equals(data));

            if (checkAnotacao is null)
            {
                Dados.Anotacoes.Add(new AnotacaoModel(professor.Id, turma.Id, data, materia, anotacao));
                _service.Save();
                return;
            }

            if (checkAnotacao.anotacao.Equals(anotacao))
                return;

            checkAnotacao.anotacao = anotacao;
            _service.Save();
        }
    }
}