using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace TechSocial
{
	public class EnvioImagemService : IEnvioImagemResposta
	{
		#region IEnvioImagemResposta implementation

		public async Task<bool> Enviar(string imageArray, string auditoria, string questao)
		{
			using (var client = CallAPI.RetornaClientHttp())
			{
				var json = JsonConvert.SerializeObject(new ImagemPOST{ audi = auditoria, questao = questao, foto = imageArray });
				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

				var result = await client.PostAsync(Constants.EnviaImagemResposta, content);
				return result.IsSuccessStatusCode;
			}

			throw new ArgumentException("Erro ao enviar");
		}


		public async Task<bool> EnviarAssinatura(string imageArray, string audi)
		{
			using (var client = CallAPI.RetornaClientHttp())
			{
				var json = JsonConvert.SerializeObject(new ImagemPOST{ audi = audi, foto = imageArray });
				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

				var result = await client.PostAsync(Constants.EnviarImagemAssinatura, content);
				return result.IsSuccessStatusCode;
			}

			throw new ArgumentException("Erro ao enviar");
		}

		#endregion
	}
}

