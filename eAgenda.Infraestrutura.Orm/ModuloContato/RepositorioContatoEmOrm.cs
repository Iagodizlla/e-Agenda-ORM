using eAgenda.Dominio.ModuloContato;
using eAgenda.Infraestrutura.Orm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infraestrutura.Orm.ModuloContato;

public class RepositorioContatoEmOrm : RepositorioBaseEmOrm<Contato>, IRepositorioContato
{
    public RepositorioContatoEmOrm(eAgendaDbContext contexto) : base(contexto)
    {
    }
}