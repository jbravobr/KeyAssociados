using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace TechSocial
{
    public static class CallAPI
    {
        // Retorna uma instância da chamada para a API
        public static HttpClient RetornaClientHttp()
        {
            var client = new HttpClient();
            try
            {
                //if (!TestaConexao())
                //new NetworkErrorAlert();

                client.BaseAddress = new Uri(Constants.serverURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return client;
        }

        // Testa conexão com a internet
        internal static bool TestaConexao()
        {
            return DependencyService.Get<INetworkStatus>().VerificaStatusConexao();
        }
    }
}

