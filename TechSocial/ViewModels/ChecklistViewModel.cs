using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechSocial
{
	public class ChecklistViewModel
	{
		public ICollection<Modulos> Modulos { get; set; }

		readonly IRespostaService RespostaService;

		public ChecklistViewModel(IRespostaService respostaService)
		{
			this.RespostaService = respostaService;
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

		public async Task<bool> EnviarRespostas(int audi)
		{
			var db = new TechSocialDatabase(false);

			var respostas = db.GetRespostaPorAuditoria(audi);

			return await this.RespostaService.EnviarResposta(respostas);
		}
	}
}

