using System;
using System.Collections.Generic;

namespace TechSocial
{
    public class JsonObjectQuestoes
    {
        public Modulos Modulos { get; set; }

        public ICollection<Questoes> Questoes { get; set; }

        public ICollection<Respostas> Respostas { get; set; }
    }
}

