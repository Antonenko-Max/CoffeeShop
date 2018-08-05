using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CoffeeShop.Data;
using CoffeeShop.DesktopClient.ViewModels;
using CoffeeShop.DesktopClient.Views;
using CoffeeShop.Windows;
using Domain.UseCases;

namespace CoffeeShop.DesktopClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);	
			
			#region DialogService
			//DialogService dialogService = new DialogService(MainWindow);
			DialogService dialogService = new DialogService();
			dialogService.Register<ConnectionViewModel, ConnectionView>();
			dialogService.Register<SizeDialogViewModel, SizeDialogView>();
			dialogService.Register<IngredientDialogViewModel, IngredientDialogView>();
			dialogService.Register<PositionDialogViewModel, PositionDialogView>();
			#endregion

			#region CompositionRoot

			var dataHolder = new PositionListDataHolder(new PositionRepositoryEF(new Mapper()));
			var facade = new PositionListFacade(new PositionListValidator(new PositionListFeedback(dataHolder)),
												new SizeListValidator(new SizeListFeedback(dataHolder)),
												new IngredientListValidator(new IngredientListFeedback(dataHolder)));

			#endregion

			MainWindow window = new MainWindow()
			{
				DataContext = new MainViewModel(dialogService, facade)
			};
			
			window.Show();
		}
	}
}
