using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using PropertyChanged;

namespace TechSocial
{
    [ImplementPropertyChanged]
    public class RotaViewModel
    {
        public ICollection<Rotas> Rotas{ get; private set; }

        public ICollection<Fornecedores> Fornecedores { get; private set; }

        public RotaViewModel(string IdRota)
        {
            this.MontaRotas(IdRota);
        }

        private void MontaRotas(string IdRota)
        {
            var db = new TechSocialDatabase(false);

            this.Rotas = db.GetRotasById(IdRota);
            this.Fornecedores = db.GetFornecedoresByRotaId(IdRota)
				.OrderBy(x => x.razaoSocial).ToList();
        }
    }
}

