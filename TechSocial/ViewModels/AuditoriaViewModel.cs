using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechSocial
{
    public class AuditoriaViewModel
    {
        public ICollection<Auditorias> Auditorias { get; set; }

        public async Task MontarAuditorias(string fornecedor)
        {   
            var db = new TechSocialDatabase(false);
            this.Auditorias = db.GetAuditorias().Where(x => x.fornecedor == fornecedor).OrderByDescending(c => c.data).ToList();
        }
    }
}

