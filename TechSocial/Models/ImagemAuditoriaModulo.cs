using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
	[Table("ImagemAuditoriaModulo")]
	public class ImagemAuditoriaModulo
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }

		public string AuditoriaId { get; set; }

		public string ModoloId { get; set; }

		public string Questao { get; set; }

		public string NomeImagem { get; set; }
	}
}

