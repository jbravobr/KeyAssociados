using System;
using Autofac;

namespace TechSocial
{
	public static class TechSocialModule
	{
		public static IContainer Initialize()
		{
			var builder = new ContainerBuilder();

			// Registrando os serviços.
			builder.RegisterInstance(new LoginService()).As<ILoginService>();
			builder.RegisterInstance(new AuditoriaService()).As<IAuditoriaService>();
			builder.RegisterInstance(new CheckListService()).As<ICheckListService>();
			builder.RegisterInstance(new QuestoesService()).As<IQuestoesService>();
			builder.RegisterInstance(new RespostaService()).As<IRespostaService>();
			builder.RegisterInstance(new BaseLegalService()).As<IBaseService>();
			builder.RegisterInstance(new EnvioImagemService()).As<IEnvioImagemResposta>();

			// Registrando as ViewModels.
			builder.RegisterType<LoginViewModel>();
			builder.RegisterType<AuditoriaViewModel>();
			builder.RegisterType<ChecklistViewModel>();
			builder.RegisterType<QuestoesViewModel>();
			builder.RegisterType<RespostaViewModel>();

			return builder.Build();
		}
	}
}

