using System;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using Autofac;
using System.Collections.Generic;
using System.IO;
using XLabs.Ioc;

namespace TechSocial
{
	public class AuditoriasPage : ContentPage
	{
		AuditoriaViewModel model = null;
		string fornecedor = String.Empty;
		ListView listViewRotas;

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			model = App.Container.Resolve<AuditoriaViewModel>();
			await model.MontarAuditorias(fornecedor);

			BindingContext = model.Auditorias;
			listViewRotas.ItemsSource = model.Auditorias;
			listViewRotas.ItemTemplate = new DataTemplate(typeof(AuditoriaViewCell));

			if (App.Current.Properties.ContainsKey("assinatura"))
			{
				try
				{
					var dic = App.Current.Properties["assinatura"] as Dictionary<int,ImageSource>;
					await this.Assina(dic);
					App.Current.Properties.Remove("assinatura");
				}
				catch
				{
					return;
				}
			}
		}

		public AuditoriasPage(string fornecedor)
		{
			Title = "Auditorias";
			this.fornecedor = fornecedor;
			this.BackgroundColor = Color.FromHex("#EEEEEE");

			listViewRotas = new ListView
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				SeparatorVisibility = SeparatorVisibility.Default,
				RowHeight = 77,
				HasUnevenRows = true
			};

			MessagingCenter.Subscribe<AuditoriaViewCell,int>(this, "assinar", (sender, audi) =>
				{
					this.Navigation.PushAsync(new AssinaturaPage(audi));
				});

			MessagingCenter.Subscribe<AuditoriaViewCell,int>(this, "finalizar", (sender, audi) =>
				{
					this.Navigation.PushAsync(new QuestoesComProblema(audi.ToString()));
				});

			listViewRotas.ItemTapped += async (sender, e) => await ExibeDetalheAuditoria(e.Item);

			var layout = new StackLayout { Children = { listViewRotas } };

			this.Content = layout;
		}

		async Task ExibeDetalheAuditoria(object item)
		{
			await Navigation.PushAsync(new ChecklistPage(((Auditorias)item).audi.ToString()));
		}

		async Task Assina(Dictionary<int,ImageSource> dic)
		{
			var imgNome = String.Concat(Path.GetRandomFileName(), ".jpg");
			//var salvarImagem = await DependencyService.Get<ISaveAndLoadFile>().SaveImage(dic.Values.First(), imgNome);
			var imagem = imgNome;

			var fileAccess = Resolver.Resolve<IFileAccess>();
			// save the media stream to a file 
			var stream = (Stream)Application.Current.Properties["StreamAssinatura"];
			var salvarImagem = fileAccess.WriteStream(imgNome, stream);

			if (salvarImagem)
			{
				var db = new TechSocialDatabase(false);
				try
				{
					db.SalvarAssinatura(imagem, dic.Keys.First());
				}
				catch
				{
					await DisplayAlert("Erro", "Erro ao salvar imagem", "OK");
				}
			}
		}
	}
}

