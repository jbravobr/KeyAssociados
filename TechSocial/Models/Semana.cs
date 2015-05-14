using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    public class Semana
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public string idRota { get; set; }

        public string ano { get; set; }

        public string semana { get; set; }

        public DateTime dataInicio { get; set; }

        public DateTime dataFim { get; set; }

        public string userCriacao { get; set; }

        public string userAlteracao { get; set; }

        public string userAuditor { get; set; }

        public string status { get; set; }

        public string dataCriacao { get; set; }

        public string dataAlteracao { get; set; }

        public string obs { get; set; }

        public string obsNaoRealizada { get; set; }

        public string validade { get; set; }
    }
}

