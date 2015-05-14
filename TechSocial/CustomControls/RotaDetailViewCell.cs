using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class RotaDetailViewCell : ViewCell
    {
        public RotaDetailViewCell()
        {
            var lblNome = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333")
            };
            lblNome.SetBinding(Label.TextProperty, "nomeFantasia");

            var lblEndereco = new Label()
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblEndereco.SetBinding(Label.TextProperty, "Endereco");

            var imgSeta = new Image
            { 
                Source = ImageSource.FromResource("TechSocial.Content.Images.check.png"), 
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.End,
            };

            var rotaDetailLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lblNome, lblEndereco }
            };

            var cellLayout = new StackLayout
            {
                Spacing = 5,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { rotaDetailLayout, imgSeta }
            };

            this.View = cellLayout;
        }
    }
}

