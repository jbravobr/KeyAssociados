using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class FornecedorViewCell : ViewCell
    {
        public FornecedorViewCell()
        {
            var lblCNPJ = new Label()
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblCNPJ.SetBinding(Label.TextProperty, "CNPJ");

            var lblRazaoSocial = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333")
            };
            lblRazaoSocial.SetBinding(Label.TextProperty, "razaoSocial");

            var lblNomeFantasia = new Label
            {
                FontSize = 11,
                FontFamily = "ArialMT",
                TextColor = Color.FromHex("#808080"),
                VerticalOptions = LayoutOptions.End
            };
            lblNomeFantasia.SetBinding(Label.TextProperty, "nomeFantasia");

            var imgSeta = new Image
            { 
                Source = ImageSource.FromResource("TechSocial.Content.Images.info.png"), 
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.End,
            };

            var fornecedorlLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lblRazaoSocial, lblCNPJ, lblNomeFantasia }
            };

            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { fornecedorlLayout, imgSeta }
            };
            
            this.View = cellLayout;
        }
    }
}

