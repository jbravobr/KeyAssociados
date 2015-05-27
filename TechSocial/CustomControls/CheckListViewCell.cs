using System;
using Xamarin.Forms;
using Autofac;

namespace TechSocial
{
    public class CheckListViewCell : ViewCell
    {
        public CheckListViewCell()
        {
            var lblModulo = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333"),
            };
            lblModulo.SetBinding(Label.TextProperty, new Binding("nome"){ StringFormat = "Módulo: {0}" });

            var lblMeta = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblMeta.SetBinding(Label.TextProperty, new Binding("meta"){ StringFormat = "Porcentagem mínima para aprovação: {0}" });

            var imgSeta = new Image
            { 
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.End
            };
            imgSeta.SetBinding(Image.SourceProperty, "Image");

            var imgCompleto = new Image
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.Start
            };
            imgCompleto.SetBinding(Image.SourceProperty, "ImgCompleto");

            var rotalLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 3,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblModulo, lblMeta, imgCompleto }
            };

            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { rotalLayout, imgSeta },
            };

            // Menu de ações
            var enviarAction = new MenuItem { Text = "Enviar" };
            enviarAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("modulo"));
            enviarAction.SetBinding(MenuItem.ClassIdProperty, new Binding("audi"));
            enviarAction.Clicked += async (sender, e) =>
            {
                var dialog = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                dialog.ShowLoading("Enviado");
                
                var model = App.Container.Resolve<ChecklistViewModel>();
                var modulo = ((int)((MenuItem)sender).CommandParameter);
                var audi = Convert.ToInt32(((MenuItem)sender).ClassId);
                
                await model.EnviarRespostas(modulo, audi);
                dialog.HideLoading();
            };

            ContextActions.Add(enviarAction);

            this.View = cellLayout;
        }
    }
}

