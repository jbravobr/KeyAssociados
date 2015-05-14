using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TechSocial
{
    public class QuestoesService : IQuestoesService
    {
        HttpResponseMessage response;

        #region IQuestoesService implementation

        public async Task<JsonObjectQuestoes> RetornarQuestoes(int modulo)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}/{1}", Constants.Questao, modulo));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JsonObjectQuestoes>(jsonResponse);

                    return json;
                }

                throw new ArgumentException("Erro de acesso ao servidor.");
            }

        }

        #endregion
    }
}

