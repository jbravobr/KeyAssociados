using System;
using Xamarin.Forms;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace TechSocial
{
    public class Modulos
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public string checklist { get; set; }

        public int modulo { get; set; }

        public Guid auditorId { get; set; }

        public int audi { get; set; }

        public string nome { get; set; }

        public int valor_meta { get; set; }

        public string nao_atingida { get; set; }

        public int meta { get; set; }

        public int ord { get; set; }

        public string status { get; set; }

        public double pontuacao
        {
            get;
            set;
        }

        public double valorMaxPontuacao { get; set; }

        public bool respondido { get; set; }

        public bool soUmaEhNA { get; set; }

        public double somaPesos { get; set; }

        public bool completo { get; set; }

        public string compile { get; set; }

        public int? atende { get; set; }

        [Ignore]
        public ICollection<Questoes> Questoes { get; set; }

        [Ignore]
        public ICollection<Respostas> Respostas { get; set; }

        [Ignore]
        public Fornecedores Fornecedores { get; set; }

        [Ignore]
        public Auditorias Auditorias { get; set; }

        [Ignore]
        public ImageSource Image
        {
            get
            {
                //Nm = Nota do módulo = 100 * S / (SP * 2) ou simplesmente 50 * S / SP;
                ImageSource imgSrc = null;
                var pont = this.pontuacao > 0 ? 100 * this.pontuacao / valorMaxPontuacao : 0;


                if (this.soUmaEhNA)
                    imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloVerde.png");
                else if (!this.respondido)
                    imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloCinza.png");
                else if (this.respondido && (pont < Convert.ToDouble(this.meta) && this.nao_atingida.ToLower().Contains("pendente")))
                    imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloLaranja.png");
                else if (this.respondido && (pont < Convert.ToDouble(this.meta) && this.nao_atingida.ToLower().Contains("reprovado")))
                    imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloRed.png");
                else
                    imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloVerde.png");
                return imgSrc;
            }
        }

        [Ignore]
        public ImageSource ImgCompleto
        {
            get
            {
                ImageSource src = null;

                src = this.completo ? ImageSource.FromResource("TechSocial.Content.Images.completo.png") :
                    ImageSource.FromResource("TechSocial.Content.Images.incompleto.png");

                return src;
            }
        }
    }
}
