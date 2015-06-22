using System;
using Xamarin.Forms;
using TechSocial;
using TechSocial.iOS;

[assembly: Dependency(typeof(NetworkStatus_IOS))]

namespace TechSocial.iOS
{
	public class NetworkStatus_IOS : INetworkStatus
	{
		public NetworkStatus_IOS()
		{

		}

		#region INetworkStatus implementation

		public bool VerificaStatusConexao()
		{
			return VerificaConexao();
		}

		#endregion

		private bool VerificaConexao()
		{
			return (!NetworkCheck.IsHostReachable("http://google.com"));
		}
	}
}

