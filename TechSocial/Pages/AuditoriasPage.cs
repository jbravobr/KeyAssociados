using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using Autofac;

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
                HasUnevenRows = true,
                //BackgroundColor = Color.FromHex("#FFFFF0")
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
    }
}

