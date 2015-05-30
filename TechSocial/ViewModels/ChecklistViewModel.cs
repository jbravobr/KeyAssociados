using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;

namespace TechSocial
{
	public class ChecklistViewModel
	{
		public ICollection<Modulos> Modulos { get; set; }

		readonly IRespostaService RespostaService;
		readonly IEnvioImagemResposta EnvioImagemService;

		public ChecklistViewModel(IRespostaService respostaService, IEnvioImagemResposta envioImagemService)
		{
			this.RespostaService = respostaService;
			this.EnvioImagemService = envioImagemService;
		}

		public async Task MontarModulos(string auditoria)
		{
			var db = new TechSocialDatabase(false);
			this.Modulos = db.GetModulosByAuditoria(auditoria);

			foreach (var modulo in Modulos)
			{
				if (db.GetRespostaPorAuditoriaModulo(modulo.audi, modulo.modulo).Any())
				{
					var _questoes = db.GetQuestoes().Where(c => c.modulo == modulo.modulo).ToList();

					foreach (var item in _questoes)
					{
						modulo.pontuacao = modulo.pontuacao + item.pontuacao;
					}
				}
			}
		}

		public async Task<ExceptionEnvioRespostas> EnviarRespostas(int audi)
		{
			var db = new TechSocialDatabase(false);

			var modulos = db.GetModulosByAuditoria(audi.ToString());

			var questoes = new List<Questoes>();

			foreach (var modulo in modulos)
			{
				questoes.AddRange(db.GetQuestoes().Where(m => m.modulo == modulo.modulo));
			}
				
			var respostas = db.GetRespostaPorAuditoria(audi);

			if (questoes.Count() == respostas.Count())
			{
				if (await this.RespostaService.EnviarResposta(respostas))
				{
					foreach (var resposta in respostas)
					{
						var img = DependencyService.Get<ISaveAndLoadFile>().GetImageArray(resposta.evidencia);
						var base64Img = Convert.ToBase64String(img);

						this.EnvioImagemService.Enviar(base64Img, resposta.audi, resposta.questao);
					}
					var auditoria = db.GetAuditorias().First(x => x.audi == audi);
					var assinatura = DependencyService.Get<ISaveAndLoadFile>().GetImageArray(auditoria.assinatura);

					return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.Enviado);
				}
				else
					return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.ErroAoEnviar);
			}
			else
				return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.RespostasPendentes);
		}
	}
}

