using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TechSocial
{
    public class UltimaAuditoriaService : IServiceUltimaAuditoria
    {
        HttpResponseMessage response;

        #region IServiceUltimaAuditoria implementation

        public async Task<JsonObjectUltimaAuditoria> GetUltimaAuditoria(string fornecedorId)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}{1}", Constants.UltimaAuditoria, fornecedorId));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JsonObjectUltimaAuditoria>(jsonResponse);

                    return json;
                }

                throw new ArgumentException("Erro de acesso ao servidor.");
            }
        }

        #endregion
    }
}

