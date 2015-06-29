using System;

using Xamarin.Forms;
using Autofac;

namespace TechSocial
{
	public class MenuPage : ContentPage
	{
		public ListView Menu { get; set; }

		public MenuPage()
		{
			Icon = "settings.png";
			Title = "menu"; // The Title property must be set.
			BackgroundColor = Color.FromHex("333333");

			Menu = new MenuListView();

			var menuLabel = new ContentView
			{
				Padding = new Thickness(10, 36, 0, 5),
				Content = new Label
				{
					TextColor = Color.FromHex("AAAAAA"),
					Text = "MENU", 
				}
			};
						
			Menu.ItemTapped += async (sender, e) =>
			{
				if (((MenuMasterItem)e.Item).TargetType == typeof(TestePage))
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
								db.SetConfiguracaoNovo(EnumUrlAtivo.Teste);
								DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Alteração feita com sucesso!");
							}
							else if (obj.Ok && obj.Text != "T&CHSOCI@L!")
							{
								DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Senha incorreta!");
							}
							else
								return;
						});

					DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Prompt(pConfigs);
				}
				else if (((MenuMasterItem)e.Item).TargetType == typeof(ProducaoPage))
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
							}
							else if (obj.Ok && obj.Text != "T&CHSOCI@L!")
							{
								DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Alert("Senha incorreta!");
							}
							else
								return;

						});

					DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().Prompt(pConfigs);
				}
				else
				{
					var model = App.Container.Resolve<LoginViewModel>();

					DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("Atualizado...");
					await model.Refresh();
					await this.Navigation.PopToRootAsync();
					//await this.Navigation.PushModalAsync(new RootPage());
					DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().HideLoading();
				}
			};

			var layout = new StackLayout
			{ 
				Spacing = 0, 
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add(menuLabel);
			layout.Children.Add(Menu);

			Content = layout;
		}
	}
}


