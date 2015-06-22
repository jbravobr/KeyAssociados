using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechSocial
{
	public class QuestoesViewModel
	{
		readonly IQuestoesService service;
		readonly IBaseService serviceBaseLegais;

		public ICollection<Questoes> Questao { get; set; }

		public ICollection<BaseLegal> BasesLegais { get; set; }

		public Respostas Respostas { get; set; }

		public QuestoesViewModel(IQuestoesService service, IBaseService serviceBaseLegais)
		{
			this.service = service;
			this.serviceBaseLegais = serviceBaseLegais;
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
			var c = criterio == "Sim" ? "2" : criterio == "Não" ? "0" : criterio == "NA" ? "0" : criterio;
			var p = peso.Split(':')[1];
			TechSocial.Respostas _resposta;

			var pontuacaoSimNao = true;
			var pontuacaoAnterior = 0;
			var SomaDoPeso = 0;
			var criterioENA = false;
			var criterioStringAnterior = string.Empty;

			if (criterio == "NA")
				criterioENA = true;

			if (_id == null || _id == 0)
			{
				_resposta = CriaResposta(obs, evidencia, c, baseLegalId,
					baseLegalTexto, data, imagemEvidencia, audi, modulo, questao, tpprazo, acoesRequeridas, criterio);
			}
			else
			{
				_resposta = db.GetRespostaById((int)_id);

				pontuacaoAnterior = Convert.ToInt32(_resposta.atende);
				criterioStringAnterior = _resposta.criterio;

				if (_resposta.atende == c)
					pontuacaoSimNao = false;

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
				_resposta.acoesRequeridas = acoesRequeridas;
				_resposta._id = (int)_id;
				_resposta.respondida = true;
				_resposta.criterio = criterio;
			}
                           
			try
			{
				db.InsertResposta(_resposta);

				if (pontuacaoSimNao)
				{
					if (pontuacaoAnterior > 0)
					{
						var subtrair = pontuacaoAnterior * Convert.ToInt32(p);
						db.SubtraiPontuacaoAntesDeAtualizar(subtrair, Convert.ToInt32(audi), Convert.ToInt32(modulo), criterioENA);
					}

					if (criterioENA)
					{
						if (criterioStringAnterior != "NA")
						{
							SomaDoPeso = (Convert.ToInt32(criterioStringAnterior) * 2) * -1;
							db.SubtraiSomaPesoModulo(Convert.ToInt32(modulo), SomaDoPeso, Convert.ToInt32(audi));
						}
						else
							SomaDoPeso = 0;
					}
					else
						SomaDoPeso = Convert.ToInt32(p);

					var pontuacao = Convert.ToInt32(c) * Convert.ToInt32(p);
					db.AtualizaPontuacaoQuestao(Convert.ToInt32(questao), pontuacao, Convert.ToInt32(modulo), Convert.ToInt32(audi), SomaDoPeso, criterioENA);
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private static Respostas CriaResposta(string obs, string evidencia, string criterio, string baseLegalId,
		                                      string baseLegalTexto, string data, string imagemEvidencia, 
		                                      string audi, string modulo, string questao, string tpprazo, string acoesRequeridas,
		                                      string _criterio)
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
				acoesRequeridas = acoesRequeridas,
				respondida = true,
				criterio = _criterio
			};
		}

		public Respostas GetQuestaoResposta(int questao, string audi, string modulo)
		{
			var db = new TechSocialDatabase(false);
			this.Respostas = db.GetRespostaProAuditoriaModuloQuestao(audi, modulo, questao);

			return this.Respostas;
		}

		public bool AtualizarRespostaQuestaoComProblema(int idResposta, string tp_prazo, string data, string obs, string acoesRequeridas)
		{
			var dbResposta = new TechSocialDatabase(false);
			var _resposta = dbResposta.GetRespostaById(idResposta);

			_resposta.acoesRequeridas = acoesRequeridas;
			_resposta.tp_prazo = tp_prazo;
			_resposta.dt_prazo = data;
			_resposta.observacao = obs;

			try
			{
				dbResposta.InsertResposta(_resposta);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}

