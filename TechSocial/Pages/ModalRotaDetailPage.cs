using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class ModalRotaDetailPage : ContentPage
    {
        TapGestureRecognizer tapStatus;

        public ModalRotaDetailPage()
        {
            var statusPicker = new Picker
            {
                Title = "Selecione uma opção", 
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            foreach (var nomeStatus in StatusVisita.GeStatusvisita())
            {
                statusPicker.Items.Add(nomeStatus.Value);
            }

            var dataPicker = new DatePicker
            {
                Format = "D",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var btnSalvar = new Button { Text = "Salvar" };
            btnSalvar.Clicked += (sender, e) => Navigation.PopModalAsync(true);

            var layout = new StackLayout
            {
                Padding = 30,
                Spacing = 2,
                Children = { statusPicker, dataPicker, btnSalvar },
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            this.Content = layout;
        }
    }
}

