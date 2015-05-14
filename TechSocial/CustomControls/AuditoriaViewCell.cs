using System;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;

namespace TechSocial
{
    public class AuditoriaViewCell : ViewCell
    {
        public AuditoriaViewCell()
        {
            var lblIdAuditoria = new Label
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333")
            };
            lblIdAuditoria.SetBinding(Label.TextProperty, "audi");

            var lblAuditoria = new Label()
            {
                FontFamily = "ArialHebrew-Bold",
                FontSize = 18,
                TextColor = Color.FromHex("#333333"),
                Text = "Auditoria: Manutencao"
            };

            var lblData = new Label
            {
                FontFamily = "HelveticaNeue-Thin",
                FontSize = 14,
                TextColor = Color.FromHex("#666")
            };
            lblData.SetBinding(Label.TextProperty, new Binding("data"){ StringFormat = "Data: {0:dd/MM/yyyy}" });

            var lnlNota = new Label
            {
                FontFamily = "GeezaPro-Bold",
                FontSize = 20,
                TextColor = Color.FromHex("#333333")
            };
            lnlNota.SetBinding(Label.TextProperty, new Binding("nota"){ StringFormat = "Nota: {0}" });

            var imgSeta = new Image
            { 
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.End
            };
            imgSeta.SetBinding(Image.SourceProperty, "Imagem");

            var rotalLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 3,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lblAuditoria, lblData, lnlNota, lblIdAuditoria }
            };

            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { rotalLayout, imgSeta },
            };
            cellLayout.SetBinding(StackLayout.IsEnabledProperty, "Ativo");

            var assinaturaAction = new MenuItem { Text = "Assinar", IsDestructive = true };
            assinaturaAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("audi"));
            assinaturaAction.Clicked += (sender, e) =>
            {
                var audi = ((int)((MenuItem)sender).CommandParameter);
                MessagingCenter.Send<AuditoriaViewCell,int>(this, "assinar", audi);
            };

            var enviarAction = new MenuItem { Text = "Enviar" };
            enviarAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            enviarAction.Clicked += (sender, e) =>
            {
                var dialog = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                dialog.ShowLoading("Enviado ...");
                Device.StartTimer(TimeSpan.FromMilliseconds(750), () =>
                    {
                        dialog.HideLoading();
                        return true;
                    });
            };

            ContextActions.Add(assinaturaAction);
            ContextActions.Add(enviarAction);

            this.View = cellLayout;
        }
    }
}

