using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    public class BaseLegal
    {
        [PrimaryKey]
        public string id_baselegal { get; set; }

        public string nome { get; set; }

        public string descricao { get; set; }

        public string data { get; set; }

        public string checklist { get; set; }

        public string questao { get; set; }

        public string ativo { get; set; }
    }
}

