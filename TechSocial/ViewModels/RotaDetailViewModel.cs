using System;
using System.Collections.Generic;

namespace TechSocial
{
    public class RotaDetailViewModel
    {
        public ICollection<RotaDetail> Detalhes { get; private set; }

        public RotaDetailViewModel(int fornecedor)
        {
            RetornarRotaDetails();
        }

        private void RetornarRotaDetails()
        {
			
        }
    }
}

