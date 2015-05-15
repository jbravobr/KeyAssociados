using System;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using Autofac;
using System.Collections.Generic;
using System.IO;

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
            var salvarImagem = false;
            salvarImagem = await DependencyService.Get<ISaveAndLoadFile>().SaveImage(dic.Values.First(), imgNome);
            var imagem = imgNome;

            if (salvarImagem)
            {
                var db = new TechSocialDatabase(false);
                try
                {
                    db.SalvarAssinatura(imagem, dic.Keys.First());
                    await this.Navigation.PopAsync();
                }
                catch
                {
                    await DisplayAlert("Erro", "Erro ao salvar imagem", "OK");
                }
            }
        }
    }
}

