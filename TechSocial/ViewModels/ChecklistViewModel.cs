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
            this.Modulos = db.GetModulosByAuditoria(auditoria).ToList();
            var qqq = db.GetQuestoes();

            var maxPont = 0;
            foreach (var _mods in Modulos)
            {
                if (maxPont > 0)
                    db.AtualizarModulo(_mods);

                maxPont = 0;
                foreach (var qq in qqq.Where(q=>q.modulo == _mods.modulo))
                {
                    maxPont += qq.peso * 2;
                    _mods.valorMaxPontuacao = maxPont;
                }
            }

            foreach (var modulo in Modulos)
            {
                modulo.completo = db.TrocaStatusModuloCompleto(Convert.ToInt32(auditoria), modulo.modulo);

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

            //if (questoes.Count() == respostas.Count())
            //{
            if (await this.RespostaService.EnviarResposta(respostas))
            {
                var imagens = db.GetImagensAuditoria(audi.ToString());

                if (imagens != null && imagens.Any())
                {
                    foreach (var imagem in imagens)
                    {
                        var img = DependencyService.Get<ISaveAndLoadFile>().GetImageArray(imagem.NomeImagem);
                        var base64Img = Convert.ToBase64String(img);

                        if (await this.EnvioImagemService.Enviar(base64Img, audi.ToString(), imagem.Questao))
                            continue;
                    }
                }
                var auditoria = db.GetAuditorias().First(x => x.audi == audi);

                if (!String.IsNullOrEmpty(auditoria.assinatura))
                {
                    var assinatura = DependencyService.Get<ISaveAndLoadFile>().GetImageArray(auditoria.assinatura);
                    var base64Img = Convert.ToBase64String(assinatura);
                    await this.EnvioImagemService.EnviarAssinatura(base64Img, auditoria.audi.ToString());
                }

                return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.Enviado);
//				}
//				else
//					return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.ErroAoEnviar);
//			}
//			else
//				return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.RespostasPendentes);
            }
            return await Task.FromResult<ExceptionEnvioRespostas>(ExceptionEnvioRespostas.ErroAoEnviar);
        }
    }
}

