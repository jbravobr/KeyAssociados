﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using PropertyChanged;

namespace TechSocial
{
    [ImplementPropertyChanged]
    public class SemanaViewModel
    {
        public ICollection<Semana> Semanas{ get; private set; }

        public SemanaViewModel()
        {
            this.MontaSemanas();
        }

        private void MontaSemanas()
        {
            var db = new TechSocialDatabase(false);

            this.Semanas = db.GetSemanas().OrderByDescending(x => x.dataInicio).ToList();
        }
    }
}

