using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TechSocial
{
    public class RotaDetalhePage : ContentPage
    {
        RotaDetailViewModel model;

        public RotaDetalhePage(int fornecedor)
        {
            BindingContext = model = new RotaDetailViewModel(fornecedor);

            var rotlist = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(RotaDetailViewCell)),
                ItemsSource = model.Detalhes,
                SeparatorColor = Color.FromHex("#ddd")
            };
                    
            var statusLabel = new Label
            {
                Text = "Status visita",
                FontSize = 12
            };

            var statusPicker = new Picker();

            foreach (var item in StatusVisita.GeStatusvisita().Select(x=>x.Value))
            {
                statusPicker.Items.Add(item);
            }
            
            var dataLabel = new Label
            {
                Text = "Data Visita",
                FontSize = 12
            };

            var dataPicker = new DatePicker
            {
                Format = "D"
            };

            var btnSalvar = new Button { Text = "Salvar", HorizontalOptions = LayoutOptions.End };

            var modal = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Padding = 5,
                Spacing = 5,
                Children = { statusLabel, statusPicker, dataLabel, dataPicker, btnSalvar },
                WidthRequest = 300,
                HeightRequest = 300
            };

            var frame = new Frame
            {
                Content = modal,
                HasShadow = true,
                OutlineColor = Color.Silver,
                WidthRequest = 305,
                HeightRequest = 205,
                IsVisible = false
            };

            btnSalvar.Clicked += (sender, e) => TrataCliqueModal(frame);
                        
            var box = new BoxView
            {
                Color = Color.Black.MultiplyAlpha(.8f),
                IsVisible = false
            };

            var absLayout = new AbsoluteLayout();
            absLayout.Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);
            absLayout.Children.Add(rotlist);

            AbsoluteLayout.SetLayoutFlags(box, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(box, new Rectangle(0, 0, 1, 1));
            absLayout.Children.Add(box);

            AbsoluteLayout.SetLayoutFlags(frame, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(frame, new Rectangle(0.5, 0.2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absLayout.Children.Add(frame);
            
            rotlist.ItemTapped += (sender, e) => TrataClique(frame);

            this.Content = absLayout;
        }

        static void TrataClique(Frame f)
        {
            f.IsVisible = true;
        }

        static void TrataCliqueModal(Frame f)
        {
            f.IsVisible = false;
        }
    }
}

