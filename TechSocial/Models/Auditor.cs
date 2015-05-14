using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    public class Auditor
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public string empresa { get; set; }

        public string user { get; set; }

        public string pass { get; set; }

        public string nome { get; set; }

        public string escopo { get; set; }

        public string norma { get; set; }

        public string acesso { get; set; }

        public string acesso_especial { get; set; }

        public string acao_reparacao { get; set; }

        public string acao_corretiva { get; set; }

        public string acao_preventiva { get; set; }

        public string verificacao_eficacia { get; set; }

        public string funcao { get; set; }

        public string nivel { get; set; }

        public string email { get; set; }

        public string telefone { get; set; }

        public string cargo { get; set; }

        public string origem { get; set; }

        public string ocorrencia { get; set; }

        public string aviso { get; set; }

        public string gestor { get; set; }

        public string visao { get; set; }

        public string observacoes { get; set; }

        public string modulo_ocur { get; set; }

        public string modulo_frnc { get; set; }

        public string modulo_diag { get; set; }

        public string modulo_plan { get; set; }

        public string modulo_sa { get; set; }

        public string modulo_sso { get; set; }

        public string modulo_sga { get; set; }

        public string modulo_lex { get; set; }

        public string modulo_mc { get; set; }

        public string perfil { get; set; }

        public string SUPER_USER { get; set; }

        public string MASTER_USER { get; set; }

        public string free_access { get; set; }

        public string frequent_clients { get; set; }

        public string terceiro { get; set; }

        public string codigo_funcional { get; set; }

        public string navegador { get; set; }

        public string criado { get; set; }

        public string abertura_escopo { get; set; }
    }
}
