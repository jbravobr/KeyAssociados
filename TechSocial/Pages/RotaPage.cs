using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace TechSocial
{
    public class RotaPage : ContentPage
    {
        RotaViewModel model;

        public RotaPage(string IdRota)
        {
            Title = "Rotas";
            BindingContext = model = new RotaViewModel(IdRota);
            this.BackgroundColor = Color.FromHex("#EEEEEE");

            var listViewRotas = new ListView
            {
                ItemsSource = model.Fornecedores,
                VerticalOptions = LayoutOptions.StartAndExpand,
                ItemTemplate = new DataTemplate(typeof(RotaViewCell)),
                SeparatorVisibility = SeparatorVisibility.Default,
                RowHeight = 107,
                //BackgroundColor = Color.FromHex("#FFFFF0")
            };

            listViewRotas.ItemTapped += async (sender, e) =>
            {
                await ExibeDetalheRota(e.Item);
                ((ListView)sender).SelectedItem = null;
            };

            var layout = new StackLayout { Children = { listViewRotas } };
            this.Content = layout;
        }

        async Task ExibeDetalheRota(object item)
        {
            await Navigation.PushAsync(new AuditoriasPage(((Fornecedores)item).fornecedor));
        }
    }
}

