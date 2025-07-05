using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infraestrutura.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.ModuloTarefa
{
    public class RepositorioTarefaEmArquivo : IRepositorioTarefa
    {
        private readonly ContextoDeDados contexto;
        private readonly List<Tarefa> registros;
        public RepositorioTarefaEmArquivo(ContextoDeDados contexto)
        {
            this.contexto = contexto;
            registros = contexto.Tarefas;
        }

        public List<Tarefa> SelecionarTarefas()
        {
            return registros;
        }

        public void AtualizarPercentual(Guid id)
        {
            Tarefa tarefa = SelecionarPorId(id);
            if (tarefa.Itens == null || tarefa.Itens.Count == 0)
            {
                //tarefa.PercentualConcluido = "2";
                return;
            }

            int quantidadeConcluidos = 0;

            foreach (var item in tarefa.Itens)
            {
                //if (item.StatusConclusao == "Concluído")
                {
                    quantidadeConcluidos++;
                }
            }

            //tarefa.PercentualConcluida = Math.Round((quantidadeConcluidos * 100.0) / tarefa.Items.Count, 2);
            contexto.Salvar();
            return;
        }

        public void AtualizarStatus(Guid id)
        {
            Tarefa tarefa = SelecionarPorId(id);

            if (tarefa.StatusConcluida == "Concluído") return;

            //if (tarefa.PercentualConcluida == 100)
            //{
            //    tarefa.DataConclusao = DateTime.Now;
            //    tarefa.StatusConcluida = "Concluído";
            //}

            contexto.Salvar();
            return;
        }

        public List<Tarefa> SelecionarTarefasConcluidas()
        {
            var tarefasConcluidas = new List<Tarefa>();

            foreach (var item in registros)
            {
                //if (item.PercentualConcluida == 100)
                    tarefasConcluidas.Add(item);
            }

            return tarefasConcluidas;
        }

        public List<Tarefa> SelecionarTarefasPendentes()
        {
            var tarefasPendentes = new List<Tarefa>();

            foreach (var item in registros)
            {
                //if (item.PercentualConcluida != 100)
                    tarefasPendentes.Add(item);
            }

            return tarefasPendentes;
        }

        public void CadastrarTarefa(Tarefa tarefa)
        {
            registros.Add(tarefa);

            contexto.Salvar();
        }

        public Tarefa SelecionarPorId(Guid idRegistro)
        {
            foreach (var item in registros)
            {
                if (item.Id == idRegistro)
                    return item;
            }

            return null;
        }

        public bool EditarTarefa(Guid id, Tarefa tarefa)
        {
            foreach (var item in registros)
            {
                if (item.Id == id)
                {
                    item.AtualizarRegistro(tarefa);

                    contexto.Salvar();

                    return true;
                }
            }

            return false;
        }

        public bool ExcluirTarefa(Guid id)
        {
            Tarefa registroSelecionado = SelecionarPorId(id);

            if (registroSelecionado != null)
            {
                registros.Remove(registroSelecionado);

                contexto.Salvar();

                return true;
            }

            return false;
        }

        public void Cadastrar(Tarefa tarefa)
        {
            throw new NotImplementedException();
        }

        public bool Editar(Guid idTarefa, Tarefa tarefaEditada)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(Guid idTarefa)
        {
            throw new NotImplementedException();
        }

        public void AdicionarItem(Item item)
        {
            throw new NotImplementedException();
        }

        public bool AtualizarItem(Item itemAtualizado)
        {
            throw new NotImplementedException();
        }

        public bool RemoverItem(Item item)
        {
            throw new NotImplementedException();
        }

        public Tarefa? SelecionarTarefaPorId(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}