using System;

namespace TechSocial
{
    public static class Constants
    {
        public const string serverURL = "http://techsocial.com.br/hering/webservices/app/api/";
        // URL base da API.

        public const string LoginMethod = "login.json";
        // Faz o login do usuário.

        public const string AuditoriaEmpresa = "auditing.json/hering";
        // Coletas as auditorias do usuário.

        public const string CheckListEmpresa = "checklist.json";
        // Coleta os módulos da auditoria.

        public const string CheckListDados = "answers.json";
        // Coleta as respostas dos módulos da auditoria.

        public const string Questao = "question.json";
        // Coleta as questões dos módulos da auditoria.

        public const string BaseLegal = "baselegal.json/0/0/0";
        //Coleta as bases legais.
    }
}

