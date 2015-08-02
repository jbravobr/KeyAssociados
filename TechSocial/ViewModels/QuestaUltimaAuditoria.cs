using System;

namespace TechSocial
{
    public class QuestaUltimaAuditoria
    {
        public RespostaUltima Resposta;


        public QuestaUltimaAuditoria(RespostaUltima r)
        {
            this.ConfiguraUltimaQuestao(r);
        }

        private void ConfiguraUltimaQuestao(RespostaUltima ultimaResposta)
        {

            if (ultimaResposta.atende == null)
            {
                this.Resposta.atende = "NA";
            }

            this.Resposta = ultimaResposta;
        }
    }
}

