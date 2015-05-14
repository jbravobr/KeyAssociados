using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechSocial
{
    public class QuestoesViewModel
    {
        readonly IQuestoesService service;

        public ICollection<Questoes> Questao { get; set; }

        public ICollection<BaseLegal> BasesLegais { get; set; }

        public Respostas Respostas { get; set; }

        public QuestoesViewModel(IQuestoesService service)
        {
            this.service = service;
        }

        public async Task MontarQuestao(int modulo)
        {
            var db = new TechSocialDatabase(false);
            this.Questao = db.GetQuestoes().Where(x => x.modulo == modulo).ToList();
            this.BasesLegais = db.GetBaseLegal();
        }

        public bool SalvarQuestao(string obs, string evidencia, string criterio, string baseLegalId,
                                  string baseLegalTexto, string data, string imagemEvidencia, 
                                  string audi, string modulo, string questao, string tpprazo, string acoesRequeridas,
                                  string peso, int? _id = null)
        {
            var db = new TechSocialDatabase(false);
            var c = criterio == "Sim" ? "1" : criterio == "Não" ? "0" : criterio;
            var p = peso.Split(':')[1];
            TechSocial.Respostas _resposta;

            if (_id == null || _id == 0)
            {
                _resposta = CriaResposta(obs, evidencia, criterio, baseLegalId,
                    baseLegalTexto, data, imagemEvidencia, audi, modulo, questao, tpprazo, acoesRequeridas);
            }
            else
            {
                _resposta = db.GetRespostaById((int)_id);
                _resposta.observacao = obs;
                _resposta.evidencia = evidencia;
                _resposta.atende = c;
                _resposta.atualizacao = string.Empty;
                _resposta.audi = audi;
                _resposta.dt_prazo = data;
                _resposta.evidencia = imagemEvidencia;
                _resposta.id_baselegal = baseLegalId;
                _resposta.baseLegalTexto = baseLegalTexto;
                _resposta.modulo = modulo;
                _resposta.observacao = obs;
                _resposta.questao = questao;
                _resposta.tp_prazo = tpprazo;
                _resposta.acoesRequeridadas = acoesRequeridas;
                _resposta._id = (int)_id;
                _resposta.respondida = true;
            }
                           
            try
            {
                db.InsertResposta(_resposta);

                var pontuacao = Convert.ToInt32(c) * Convert.ToInt32(p);
                db.AtualizaPontuacaoQuestao(Convert.ToInt32(questao), pontuacao, Convert.ToInt32(modulo), Convert.ToInt32(audi));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static Respostas CriaResposta(string obs, string evidencia, string criterio, string baseLegalId,
                                              string baseLegalTexto, string data, string imagemEvidencia, 
                                              string audi, string modulo, string questao, string tpprazo, string acoesRequeridas)
        {

            var atende = criterio == "Não" ? "0" : criterio == "Sim" ? "1" : criterio;

            return new Respostas
            {
                atende = atende,
                atualizacao = string.Empty,
                audi = audi,
                dt_prazo = data,
                evidencia = imagemEvidencia,
                id_baselegal = baseLegalId,
                baseLegalTexto = baseLegalTexto,
                modulo = modulo,
                observacao = obs,
                questao = questao,
                tp_prazo = tpprazo,
                acoesRequeridadas = acoesRequeridas,
                respondida = true
            };
        }

        public Respostas GetQuestaoResposta(int questao, string audi, string modulo)
        {
            var db = new TechSocialDatabase(false);
            this.Respostas = db.GetRespostaProAuditoriaModuloQuestao(audi, modulo, questao);

            return this.Respostas;
        }
    }
}

