using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class RotaViewCell : ViewCell
    {
        public RotaViewCell()
        {
            var lblRazaoSocial = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333"),
            };
            lblRazaoSocial.SetBinding(Label.TextProperty, "razaoSocial");

            var lblEndereco = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblEndereco.SetBinding(Label.TextProperty, "Endereco");

            var lblResponsavel = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblResponsavel.SetBinding(Label.TextProperty, new Binding("resp_nome") { StringFormat = "Responsável : {0}" });

            var lblTelefones = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            lblTelefones.SetBinding(Label.TextProperty, "Telefones");

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
                Children = { lblRazaoSocial, lblEndereco, lblResponsavel, lblTelefones }
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

