using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TechSocial
{
    public class BaseLegalService : IBaseService
    {
        HttpResponseMessage response;

        /// <summary>
        /// Retorna as Bases Legais.
        /// </summary>
        public async Task<JsonObject> RetornarBasesLegais()
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}", Constants.BaseLegal));

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

