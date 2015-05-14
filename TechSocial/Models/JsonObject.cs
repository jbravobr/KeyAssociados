using System;
using System.Collections.Generic;

namespace TechSocial
{
    public class JsonObject
    {
        public Auditor Auditor { get; set; }

        public ICollection<Rotas> Rotas { get; set; }

        public ICollection<Auditorias> Auditorias { get; set; }

        public ICollection<Modulos> Modulos { get; set; }

        public ICollection<Semana> Semanas { get; set; }

        public ICollection<BaseLegal> BaseLegal { get; set; }
    }
}

