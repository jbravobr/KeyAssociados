using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
	public class Questoes
	{
		[PrimaryKey, AutoIncrement]
		public int _id { get; set; }

		public int modulo { get; set; }

		public int audi { get; set; }

		public int questao { get; set; }

		public string label { get; set; }

		public string texto { get; set; }

		public string criterio { get; set; }

		public string evidencia { get; set; }

		public int pontuacao { get; set; }

		public string BaseLegalId { get; set; }

		public string BaseLegalNome { get; set; }

		public string BaseLegalDescricao { get; set; }

		[Ignore]
		public string Pergunta
		{
			get
			{
				return String.Format("{0} - {1}", this.label, this.texto);
			}
		}

		[Ignore]
		public string PesoPergunta
		{
			get
			{
				return String.Format("Peso: {0}", this.peso);
			}
		}

		public int peso { get; set; }

		[Ignore]
		public virtual Modulos Modulos { get; set; }
	}
}

