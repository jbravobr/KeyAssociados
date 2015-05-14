using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class SemanaViewCell : ViewCell
    {
        public SemanaViewCell()
        {
            var lblSemana = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333"),
            };
            lblSemana.SetBinding(Label.TextProperty, new Binding("semana"){ StringFormat = "Semana {0}" });

            var lblInicio = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblInicio.SetBinding(Label.TextProperty, new Binding("dataInicio"){ StringFormat = "Data de Início {0:dd/MM/yyyy}" });

            var lblFim = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblFim.SetBinding(Label.TextProperty, new Binding("dataFim") { StringFormat = "Data Final : {0:dd/MM/yyyy}" });

            var imgSeta = new Image
            { 
                Source = ImageSource.FromResource("TechSocial.Content.Images.setaParaDireita.png"), 
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.End
            };

            var rotalLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lblSemana, lblInicio, lblFim }
            };

            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { rotalLayout, imgSeta }
            };

            this.View = cellLayout;
        }
    }
}

