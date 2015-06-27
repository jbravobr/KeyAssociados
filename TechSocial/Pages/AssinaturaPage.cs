using System;
using Xamarin.Forms;
using Acr.XamForms.SignaturePad;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TechSocial
{
	public class AssinaturaPage : ContentPage
	{
		int auditoria;
		Image imgAss;
		StackLayout assinado;
		SignaturePadView signature;
		Label infoAuditoriaAssinatura;
		Button btnSalvar;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var db = new TechSocialDatabase(false);

			if (db.GetAuditorias().Any(a => a.audi == auditoria && !String.IsNullOrEmpty(a.assinatura)))
			{
				var assinatura = db.GetAuditorias().First(a => a.audi == auditoria).assinatura;
				var img = DependencyService.Get<ISaveAndLoadFile>().GetImage(assinatura);
				imgAss.Source = ImageSource.FromFile(img);
				assinado.IsVisible = true;
				signature.IsVisible = false;
				//infoAuditoriaAssinatura.IsVisible = true;
				btnSalvar.IsVisible = false;
			}
		}

		public AssinaturaPage(int audi)
		{
			auditoria = audi;
			Application.Current.Properties.Remove("DataAtende");

			Title = "Assinatura";

			imgAss = new Image();
			imgAss.Aspect = Aspect.Fill;

			assinado = new StackLayout
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				Children =
				{
					imgAss
				},
				BackgroundColor = Color.Transparent,
				HeightRequest = 400,
				WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth - 10,
				IsVisible = false
			};
			
			signature = new SignaturePadView();

			// Configurações.
			signature.ClearText = "Apagar";
			signature.SignatureLineColor = Color.Blue;
			signature.StrokeColor = Color.Blue;
			signature.StrokeWidth = 2.5f;
			signature.HeightRequest = 400;
			signature.WidthRequest = DependencyService.Get<Acr.XamForms.Mobile.IDeviceInfo>().ScreenWidth - 10;
			signature.BackgroundColor = Color.FromHex("#F5F5DC");
				
			btnSalvar = new Button
			{
				Text = "Salvar"
			};
			btnSalvar.Clicked += (sender, e) =>
			{
				var imgSource = ImageSource.FromStream(() =>
					{
						var stream = signature.GetImage(ImageFormatType.Jpg);
						return stream;
					});
				Application.Current.Properties["StreamAssinatura"] = signature.GetImage(ImageFormatType.Jpg);
					
				var dic = new Dictionary<int,ImageSource>();
				dic.Add(audi, imgSource);

				App.Current.Properties["assinatura"] = dic;
				this.Navigation.PopAsync();
			};

			infoAuditoriaAssinatura = new Label
			{
				Text = "Ao salvar a assinatura, não será possível alterá-la para esta auditora",
				XAlign = TextAlignment.Center,
				IsVisible = true
			};

			var stack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				Padding = 30,
				Spacing = 14,
				Children =
				{
					new Label
					{
						Text = DateTime.Now.ToString("dd/MM/yyyy"),
						FontSize = 24,
						XAlign = TextAlignment.Center
					},
					new Label
					{
						Text = "Declaro ter ciência das pendências identificadas durante a Auditoria, bem como as atividades e prazos de conclusão do Plano de Ação.",
						FontSize = 24,
						XAlign = TextAlignment.Center
					},
					assinado,
					signature, 
					infoAuditoriaAssinatura,
					btnSalvar
				}
			};

			this.Content = stack;
		}
	}
}

