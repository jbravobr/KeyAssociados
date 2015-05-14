using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace TechSocial
{
    public class SemanaPage : ContentPage
    {
        SemanaViewModel model;

        public SemanaPage()
        {
            Title = "Semanas";
            BindingContext = model = new SemanaViewModel();
            this.BackgroundColor = Color.FromHex("#EEEEEE");

            var listViewRotas = new ListView
            {
                ItemsSource = model.Semanas,
                VerticalOptions = LayoutOptions.StartAndExpand,
                ItemTemplate = new DataTemplate(typeof(SemanaViewCell)),
                SeparatorVisibility = SeparatorVisibility.Default,
                RowHeight = 107
            };

            listViewRotas.ItemTapped += async (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
                await ExibeDetalheRota(e.Item);
            };

            var layout = new StackLayout { Children = { listViewRotas } };
            this.Content = layout;
        }

        async Task ExibeDetalheRota(object item)
        {
            await Navigation.PushAsync(new RotaPage(((Semana)item).idRota));
        }
    }
}

