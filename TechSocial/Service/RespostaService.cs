using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace TechSocial
{
	public class RespostaService : IRespostaService
	{
		HttpResponseMessage response;

		/// <summary>
		/// Retorna respostas para módulo.
		/// </summary>
		public async Task<Answers> RetornarRespostasParaAuditoria(int auditoriaId)
		{
			using (var client = CallAPI.RetornaClientHttp())
			{
				response = await client.GetAsync(String.Format("{0}/{1}", Constants.CheckListDados, auditoriaId));

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var json = JsonConvert.DeserializeObject<Answers>(jsonResponse);

					return json;
				}

				throw new ArgumentException("Erro de acesso ao servidor.");
			}
		}

		/// <summary>
		/// Executa o envio da resposta do módulo
		/// </summary>
		public async Task<bool> EnviarResposta(ICollection<Respostas> resp)
		{
			using (var client = CallAPI.RetornaClientHttp())
			{
				var json = JsonConvert.SerializeObject(resp);
				HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

				try
				{
					var result = await client.PostAsync(Constants.PostResposta, content);

					return result.IsSuccessStatusCode ? await Task.FromResult<bool>(true) : await Task.FromResult<bool>(false);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}
	}
}

