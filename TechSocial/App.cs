using System;

using Xamarin.Forms;
using Autofac;

namespace TechSocial
{
	public class App : Application
	{
		public static IContainer Container { get; set; }

		public App()
		{
			// Inicializa Autofac.
			App.Container = TechSocialModule.Initialize();

			// Cria a base de dados se esta já não existir.
			CriaBD();

			// The root page of your application
			MainPage = new LoginPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		static void CriaBD()
		{
			// Cria banco de dados
			var db = new TechSocialDatabase(true);
			db.CriaSchema();
		}
	}
}

