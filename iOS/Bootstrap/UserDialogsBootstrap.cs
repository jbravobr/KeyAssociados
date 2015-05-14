// HACK: this is to deal with the linker nuking the assembly
using Acr.XamForms.UserDialogs.iOS;

namespace TechSocial.iOS.Bootstrap
{
    public class UserDialogsBootstrap 
    {
        public UserDialogsBootstrap() 
        {
            new UserDialogService();
        }
    }
}