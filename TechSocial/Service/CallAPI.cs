﻿using System;

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
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;

            var client = new HttpClient(handler);
            try
            {
                //if (!TestaConexao())
                //new NetworkErrorAlert();

                var db = new TechSocialDatabase(false);

                client.BaseAddress = new Uri("https://compliance.ciahering.com.br/hering/webservices/app/api/");
                //client.BaseAddress = new Uri("http://techsocial.com.br/hering/webservices/app/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(10);
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

