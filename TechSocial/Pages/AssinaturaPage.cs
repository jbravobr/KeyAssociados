﻿using System;
using Xamarin.Forms;
using Acr.XamForms.SignaturePad;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            btnSalvar.Clicked += (sender, e) =>
            {
                var imgSource = ImageSource.FromStream(() =>
                    {
                        var stream = signature.GetImage(ImageFormatType.Jpg);
                        return stream;
                    });

                var dic = new Dictionary<int,ImageSource>();
                dic.Add(audi, imgSource);

                App.Current.Properties["assinatura"] = dic;
                this.Navigation.PopAsync();
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 30,
                Spacing = 14,
                Children = { signature, btnSalvar }
            };

            this.Content = stack;
        }
    }
}

