using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloCompromisso;

public class RepositorioCompromissoEmOrm : RepositorioBaseEmOrm<Compromisso>, IRepositorioCompromisso
{
    public RepositorioCompromissoEmOrm(eAgendaDbContext contexto) : base(contexto)
    {
    }
}