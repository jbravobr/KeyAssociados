using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TechSocial
{
    public class LoginService : ILoginService
    {
        HttpResponseMessage response;

        /// <summary>
        /// Executa a rotina de Login do sistema.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="pass">Pass.</param>
        public async Task<JsonObject> ExecutarLogin(string user, string pass)
        {
            using (var client = CallAPI.RetornaClientHttp())
            {
                response = await client.GetAsync(String.Format("{0}/{1}/{2}", Constants.LoginMethod, user, pass));

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

