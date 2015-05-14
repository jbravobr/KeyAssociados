using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
    }
}

