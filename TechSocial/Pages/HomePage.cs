using System;
using Xamarin.Forms;

namespace TechSocial
{
    public class HomePage : TabbedPage
    {
        public HomePage()
        {
            this.Children.Add(new NavigationPage(new FornecedorPage()){ Title = "Fonecedores", Icon = "fornecedor.png" });
            //this.Children.Add(new NavigationPage(new RotaPage()){ Title = "Rotas", Icon = "rota.png" });
            //this.Children.Add(new NavigationPage(new CheckListPage()){ Title = "Checklist", Icon = "checklist.png" });
        }
    }
}

