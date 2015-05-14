using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TechSocial
{
    public class AuditoriaService : IAuditoriaService
    {
        HttpResponseMessage response;

        /// <summary>
        /// Retorna as Auditorias do Fornecedor.
        /// </summary>
        /// <param name="fornecedor">fornecedor.</param>
        public async Task<JsonObject> RetornarAuditorias(string fornecedor)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}/{1}", Constants.AuditoriaEmpresa, fornecedor));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JsonObject>(jsonResponse);

                    return json;
                }

                throw new ArgumentException("Erro de acesso ao servidor.");
            }
        }
    }
}

