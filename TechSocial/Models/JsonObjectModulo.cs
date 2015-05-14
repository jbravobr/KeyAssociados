using System;
using System.Collections.Generic;

namespace TechSocial
{
    public class JsonObjectModulo
    {
        public Auditorias Auditorias { get; set; }

        public Fornecedores Fornecedores { get; set; }

        public ICollection<Modulos> Modulos { get; set; }
    }
}

