using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TechSocial
{
    public class CheckListService : ICheckListService
    {
        HttpResponseMessage response;

        #region ICheckListService implementation

        public async Task<JsonObjectModulo> RetornaChecklist(string checklist)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}/{1}", Constants.CheckListEmpresa, checklist));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JsonObjectModulo>(jsonResponse);

                    return json;
                }

                throw new ArgumentException("Erro de acesso ao servidor.");
            }

        }

        #endregion
    }
}

