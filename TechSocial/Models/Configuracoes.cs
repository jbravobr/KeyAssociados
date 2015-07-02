using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
	public class Configuracoes
	{
		[PrimaryKey, AutoIncrement]
		public int Id{ get; set; }

		[Ignore] 
		public string urlProducao
		{
			get
			{
				return "http://compliance.ciahering.com.br/hering/webservices/app/api/";
			}
		}

		[Ignore]
		public string urlTeste
		{
			get
			{
				return "http://techsocial.com.br/hering/webservices/app/api/";
			}
		}

		public EnumUrlAtivo UrlAtiva { get; set; }
	}
}

