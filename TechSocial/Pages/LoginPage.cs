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

            var controlsLayout = new StackLayout
            {
                Padding = 10,
                Spacing = 8,
                Children =
                {
                    imgLayout,
                    txtLogin,
                    txtSenha,
                    btnAcessar
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

                var netStatus = DependencyService.Get<Acr.XamForms.Mobile.Net.INetworkService>().IsConnected;
                if (!netStatus)
                {
                    var dialog = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                    await dialog.AlertAsync(String.Empty, "Seu primeiro logon deve ser feito online!", "OK");
                    return;
                }
                else
                {
                    var loading = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                    loading.ShowLoading("Carregando dados");

                    if (await model.ExecutarLogin(usuario, senha))
                    {
                        loading.HideLoading();
                        await Navigation.PushModalAsync(new NavigationPage(new SemanaPage()));
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
}

