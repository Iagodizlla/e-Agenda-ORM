using eAgenda.Dominio.ModuloTarefa;
using eAgenda.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.WebApp.Models;

public abstract class FormularioTarefaViewModel
{
    [Required(ErrorMessage = "O campo \"Titulo\" é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Titulo\" precisa conter entre 2 e 100 caracteres.")]
    public string Titulo { get; set; }
    public string Prioridade { get; set; } = "Baixa"; // Default value
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataConclusao { get; set; }
    public string StatusConcluida { get; set; } = "Pendente"; // Default value
    public double PercentualConcluida { get; set; } = 0.0;
    public List<Item>? Itens { get; set; }

    public class CadastrarTarefaViewModel : FormularioTarefaViewModel
    {
        public List<string> TitulosItens { get; set; } = new();
        public List<string> StatusConclusoesItens { get; set; } = new();

        public CadastrarTarefaViewModel()
        {
            Itens = new List<Item>();
        }

        public CadastrarTarefaViewModel(
            string titulo,
            string prioridade,
            DateTime dataCriacao,
            string statusConcluida,
            double percentualConcluida,
            List<Item> itens,
            DateTime dataConclusao
        ) : this()
        {
            Titulo = titulo;
            Prioridade = prioridade;
            DataCriacao = dataCriacao;
            DataConclusao = dataConclusao;
            StatusConcluida = statusConcluida;
            PercentualConcluida = percentualConcluida;
        }

        public Tarefa ParaEntidade()
        {
            var tarefa = new Tarefa
            (
                Titulo,
                Prioridade,
                DataCriacao,
                StatusConcluida,
                DataConclusao
            );

            for (int i = 0; i < TitulosItens?.Count; i++)
            {
                var item = new Item(TitulosItens[i], StatusConclusoesItens[i], tarefa);
                tarefa.Itens.Add(item);
            }

            return tarefa;
        }
    }

    public class EditarTarefaViewModel : FormularioTarefaViewModel
    {
        public Guid Id { get; set; }

        public List<string> TitulosItens { get; set; } = new();
        public List<string> StatusConclusoesItens { get; set; } = new();

        public EditarTarefaViewModel()
        {
            // Inicializa as listas para evitar null
            TitulosItens = new List<string>();
            StatusConclusoesItens = new List<string>();
        }

        public EditarTarefaViewModel(Guid id, string titulo, string prioridade, DateTime dataCriacao,
            string statusConcluida, double percentualConcluida, DateTime dataConclusao, List<Item> itens)
            : this() // chama o construtor padrão
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
            DataCriacao = dataCriacao;
            StatusConcluida = statusConcluida;
            PercentualConcluida = percentualConcluida;
            DataConclusao = dataConclusao;

            if (itens != null)
            {
                foreach (var item in itens)
                {
                    TitulosItens.Add(item.Titulo);
                    StatusConclusoesItens.Add(item.Concluido);
                }
            }
        }

        public Tarefa ParaEntidade()
        {
            var tarefa = new Tarefa {
                Titulo = Titulo,
                Prioridade = Prioridade,
                DataCriacao = DataCriacao,
                StatusConcluida = StatusConcluida,
                DataConclusao = DataConclusao
            };

            for (int i = 0; i < TitulosItens?.Count; i++)
            {
                var item = new Item(TitulosItens[i], StatusConclusoesItens[i], tarefa);
                tarefa.Itens.Add(item);
            }

            return tarefa;
        }
    }


    public class ExcluirTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }

        public ExcluirTarefaViewModel() { }

        public ExcluirTarefaViewModel(Guid id, string titulo) : this()
        {
            Id = id;
            Titulo = titulo;
        }
    }

    public class VisualizarTarefaViewModel
    {
        public List<DetalhesTarefaViewModel> Registros { get; }

        public Dictionary<string, List<DetalhesTarefaViewModel>> TarefasAgrupadasPorPrioridade { get; }

        public VisualizarTarefaViewModel(List<Tarefa> tarefas)
        {
            Registros = new List<DetalhesTarefaViewModel>();

            if (tarefas != null)
            {
                foreach (var t in tarefas)
                {
                    var detalhesVM = t.ParaDetalhesVM();
                    Registros.Add(detalhesVM);
                }
            }

            TarefasAgrupadasPorPrioridade = Registros
                .GroupBy(t => t.Prioridade.ToString())
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    public class DetalhesTarefaViewModel
    {
        public Guid Id { get; }
        public string Titulo { get; }
        public string Prioridade { get; }
        public DateTime DataCriacao { get; }
        public DateTime DataConclusao { get; }
        public string StatusConcluida { get; }
        public double PercentualConcluida { get; }
        public List<Item> Itens { get; } = new List<Item>();

        public DetalhesTarefaViewModel(Guid id, string titulo, string prioridade, DateTime dataCriacao, string statusConcluida, double percentualConcluida, List<Item> itens, DateTime dataConclusao)
        {
            Id = id;
            Titulo = titulo;
            Prioridade = prioridade;
            DataCriacao = dataCriacao;
            DataConclusao = dataConclusao;
            StatusConcluida = statusConcluida;
            PercentualConcluida = percentualConcluida;
            Itens = itens ?? new List<Item>(); // <-- ESSA LINHA É ESSENCIAL
        }
    }

    public class SelecionarTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Prioridade { get; }
        public string Titulo { get; set; }

        public SelecionarTarefaViewModel(Guid id, string prioridade, string titulo)
        {
            Id = id;
            Prioridade = prioridade;
            Titulo = titulo;
        }
    } 
}