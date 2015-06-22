using System;
using SQLite.Net.Attributes;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using Acr.XamForms.SignaturePad;

namespace TechSocial
{
	public class Auditorias
	{
		[PrimaryKey,AutoIncrement]
		public int _id { get; set; }

		public string empresa { get; set; }

		public string fornecedor { get; set; }

		public int audi { get; set; }

		public string assinatura { get; set; }

		public double? nota { get; set; }

		public int status { get; set; }

		public double pontuacao
		{
			get
			{
				var db = new TechSocialDatabase(false);
				var auditoria = this.audi.ToString();

				if (db.GetModulosByAuditoria(auditoria).Any())
				{
					var _modulos = db.GetModulosByAuditoria(auditoria).ToList();

					foreach (var item in _modulos)
					{
						return +item.pontuacao;
					}
				}

				return 0;
			}
		}

		public DateTime data { get; set; }

		public int resultado { get; set; }

		public string obs { get; set; }

		public bool enviado { get; set; }

		[Ignore]
		public ImageSource Imagem
		{
			get
			{
				ImageSource imgSrc = null;

				switch (this.status)
				{
					case 0:
					case 5:
					case 6:
						imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloRed.png");
						break;
					case 1:
						imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloVerde.png");
						break;
					case 2:
						imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloLaranja.png");
						break;
					case 3:
						imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloCinza.png");
						break;
					case 4:
						imgSrc = ImageSource.FromResource("TechSocial.Content.Images.circuloAzul.png");
						break;
				}

				return imgSrc;            
			}
		}

		[Ignore]
		public bool Ativo
		{
			get
			{
				return this.status != 1; 
			}
		}
	}
}

