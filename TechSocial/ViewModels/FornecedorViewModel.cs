using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using PropertyChanged;

namespace TechSocial
{
    [ImplementPropertyChanged]
    public class FornecedorViewModel
    {
        public ICollection<Fornecedores> Fornecedores { get; private set; }

        public FornecedorViewModel()
        {
            this.GeraListaFornecedores();
        }

        private void GeraListaFornecedores()
        {
            var db = new TechSocialDatabase(false);

            var lista = db.GetFornecedores().ToList();
            this.Fornecedores = lista;
        }
    }
}

