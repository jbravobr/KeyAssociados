using System;

using Xamarin.Forms;

namespace TechSocial
{
	public class ProducaoPage : ContentPage
	{
		public ProducaoPage()
		{
			var db = new TechSocialDatabase(false);

			var pConfigs = new Acr.XamForms.UserDialogs.PromptConfig();
			pConfigs.CancelText = "Cancelar";
			pConfigs.Message = "Insira a senha para efetuar a troca de URL";
			pConfigs.OkText = "Confirmar";
			pConfigs.OnResult = new Action<Acr.XamForms.UserDialogs.PromptResult>(delegate(Acr.XamForms.UserDialogs.PromptResult obj)
				{
					if (obj.Ok && obj.Text == "T&CHSOCI@L!")
					{
						db.SetConfiguracaoNovo(EnumUrlAtivo.Producao);
						DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Alteração feita com sucesso!");
						Navigation.PushModalAsync(new RootPage());
					}
					else if (!obj.Ok)
					{
						Navigation.PushModalAsync(new RootPage());
					}
					else
						DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Senha incorreta!");

				});

			DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Prompt(pConfigs);
		}
	}
}


