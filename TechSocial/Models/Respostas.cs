using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    public class Respostas
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public string audi { get; set; }

        public string modulo { get; set; }

        public string questao { get; set; }

        public string atende { get; set; }

        public string observacao { get; set; }

        public string acoesRequeridadas { get; set; }

        public string imagem { get; set; }

        public string evidencia { get; set; }

        public string id_baselegal { get; set; }

        public string baseLegalTexto { get; set; }

        public string atualizacao { get; set; }

        public string tp_prazo { get; set; }

        public string dt_prazo { get; set; }

        public bool respondida { get; set; }
    }
}

