using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TechSocial
{
    public class Rotas
    {
        [PrimaryKey,AutoIncrement]
        public int _id { get; set; }

        public string IdRota { get; set; }

        public string fornecedor { get; set; }

        public string status { get; set; }

        public string dataCriacao { get; set; }

        public string dataAlteracao { get; set; }

        public string userCriacao { get; set; }

        public string userAlteracao { get; set; }

        public string userAuditor { get; set; }

        public string kmInicial { get; set; }

        public string kmFinal { get; set; }

        public string kmTotal { get; set; }

        public string dataAuditoria { get; set; }

        public string obsNaoRealizada { get; set; }

        [Ignore]
        public Fornecedores Fornecedores { get; set; }
    }
}

