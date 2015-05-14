using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class CheckListDetailPage : ContentPage
    {
        CheckListModuloItem model;

        public CheckListDetailPage(CheckListModuloItem item)
        {
            BindingContext = model = item;

            var lblRequisito = new Label
            {
                FontFamily = "ArialHebrew",
                FontSize = 14,
                TextColor = Color.FromHex("#333333")
            };

            var entryEvidencia = new Entry
            {
                Placeholder = "Evidência",
                HeightRequest = 200,
                WidthRequest = 200
            };

            var lblPeso = new Label
            {
                FontFamily = "ArialHebrew",
                FontSize = 14,
                TextColor = Color.FromHex("#333333")
            };

            var lblPontuacaoPicker = new Label { Text = "Pontuação obtida na auditoria" };

            var pontuacaoPicker = new Picker();
            pontuacaoPicker.Items.Add("Sim");
            pontuacaoPicker.Items.Add("Não");

            var entruObservacao = new Entry
            {
                Placeholder = "Observação",
                HeightRequest = 200,
                WidthRequest = 200
            };

            var formLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = 10,
                Spacing = 5,
                Children =
                {
                    lblRequisito, entryEvidencia, lblPeso, lblPontuacaoPicker, pontuacaoPicker, entruObservacao
                }
            };

            this.Content = formLayout;
        }
    }
}

