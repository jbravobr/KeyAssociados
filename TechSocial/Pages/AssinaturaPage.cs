using System;
using Xamarin.Forms;
using Acr.XamForms.SignaturePad;

namespace TechSocial
{
    public class AssinaturaPage : ContentPage
    {
        public AssinaturaPage(int audi)
        {
            Title = "Assinatura";

            var signature = new SignaturePadView();

            // Configurações.
            signature.ClearText = "Apagar";
            signature.SignatureLineColor = Color.Blue;
            signature.StrokeColor = Color.Blue;
            signature.StrokeWidth = 2.5f;
            signature.HeightRequest = 400;
            signature.WidthRequest = 420;
            signature.BackgroundColor = Color.FromHex("#F5F5DC");

            var btnSalvar = new Button
            {
                Text = "Salvar"
            };
            btnSalvar.Clicked += async (sender, e) =>
            {
                //var db = new TechSocialDatabase(false);
                //var auditoria = db.
                await this.Navigation.PopAsync();
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 30,
                Spacing = 5,
                Children = { signature, btnSalvar }
            };

            this.Content = stack;
        }
    }
}

