using System;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using Autofac;

namespace TechSocial
{
	public class LoginPage : ContentPage
	{
		LoginViewModel model = null;

		public LoginPage()
		{
			var txtLogin = new Entry { Placeholder = "Usuário", WidthRequest = 350, HorizontalOptions = LayoutOptions.Center };
			var txtSenha = new Entry { Placeholder = "Senha", WidthRequest = 350, HorizontalOptions = LayoutOptions.Center, IsPassword = true };
			var btnAcessar = new Button { Text = "Acessar", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)) };

			btnAcessar.Clicked += async (sender, e) =>
			{
				var _usuario = txtLogin.Text;
				var _senha = txtSenha.Text;
				await TrataCliqueBtnAcessar(_usuario, _senha);
			};
			
			var imgLogo = new Image
			{ 
				Source = ImageSource.FromResource("TechSocial.Content.Images.logo.png"), 
				VerticalOptions = LayoutOptions.Center 
			};

			var imgLayout = new StackLayout
			{
				Padding = 20,
				Spacing = 0,
				Children = { imgLogo }
			};

			var swUrlAPI = new Switch();
			var lblSwitchUrlAPI = new Label { Text = "Trocar endereço da API" };
			swUrlAPI.Toggled += async (sender, e) =>
			{
				if (!e.Value)
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
				else
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
					
			};
			var grid = new Grid();
			grid.Children.Add(lblSwitchUrlAPI, 0, 0);
			grid.Children.Add(swUrlAPI, 1, 0);
			grid.ColumnSpacing = -350;
			grid.HorizontalOptions = LayoutOptions.CenterAndExpand;

			var controlsLayout = new StackLayout
			{
				Padding = 10,
				Spacing = 8,
				Children =
				{
					imgLayout,
					txtLogin,
					txtSenha,
					btnAcessar,
					//grid
				},
				VerticalOptions = LayoutOptions.Center
			};

			this.Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);
			this.Content = new ScrollView { Content = controlsLayout };
		}

		async Task TrataCliqueBtnAcessar(string usuario, string senha)
		{
			if (String.IsNullOrEmpty(usuario) || String.IsNullOrEmpty(senha))
				await DisplayAlert("Erro", "É necessário informa um usuário e senha!", "OK");
			else
			{
				model = App.Container.Resolve<LoginViewModel>();

				var loading = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
				loading.ShowLoading("Carregando dados");

				if (await model.ExecutarLogin(usuario, senha))
				{
					Application.Current.Properties["usuario"] = usuario;
					Application.Current.Properties["senha"] = senha;

					loading.HideLoading();
					await Navigation.PushModalAsync(new SemanaPage());
				}
				else
				{
					loading.HideLoading();
					await DisplayAlert("Erro", "Usuário ou senha inválidos", "OK");
				}
			}
		}
	}
}