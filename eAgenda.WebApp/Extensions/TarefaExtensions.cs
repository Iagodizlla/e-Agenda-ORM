using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApp.Models;
using static eAgenda.WebApp.Models.FormularioTarefaViewModel;

namespace eAgenda.WebApp.Extensions
{
    public static class TarefaExtensions
    {
        public static Tarefa ParaEntidade(this FormularioTarefaViewModel formularioVM)
        {
            return new Tarefa(formularioVM.Titulo, formularioVM.Prioridade, formularioVM.DataCriacao, formularioVM.PercentualConcluida, formularioVM.StatusConcluida, formularioVM.DataConclusao);
        }

        public static DetalhesTarefaViewModel ParaDetalhesVM(this Tarefa tarefa)
        {

            return new DetalhesTarefaViewModel(
                tarefa.Id,
                tarefa.Titulo,
                tarefa.Prioridade,
                tarefa.DataCriacao,
                tarefa.StatusConcluida,
                tarefa.PercentualConcluido,
                tarefa.Itens,
                tarefa.DataConclusao
            );
        }
    }
}
