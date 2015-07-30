using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    [Table("RespostaUltima")]
    public class RespostaUltima
    {
        [AutoIncrement,PrimaryKey]
        public int _id{ get; set; }

        public string questao { get; set; }

        public string label { get; set; }

        public string texto { get; set; }

        public string peso { get; set; }

        public string atende { get; set; }

        public string observacao { get; set; }

        public string acaorequerida { get; set; }
    }
}

