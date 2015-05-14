using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechSocial
{
    public class ChecklistViewModel
    {
        public ICollection<Modulos> Modulos { get; set; }

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
    }
}

