using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using CoffeeShop.Windows;
using Domain.UseCases;

namespace CoffeeShop.DesktopClient.ViewModels
{
	public class MainViewModel
	{
		private readonly IDialogService dialogService;
		private readonly ObservableCollection<ITab> tabs;
		private readonly PositionListFacade positionList;

		public MainViewModel(IDialogService dialogService, PositionListFacade positionList)
		{
			this.positionList = positionList;
			ShowDialogConnectionWindowCommand = new ActionCommand(p => ShowDialogConnectionWindow());
			ShowPositionListCommand = new ActionCommand(p => ShowPositionList());
			ShowConsumablesListCommand = new ActionCommand(p => ShowConsumablesList());
			ShowSellsListCommand = new ActionCommand(p => ShowSellsList());
			ShowStatisticsCommand = new ActionCommand(p => ShowStatistik());
			this.dialogService = dialogService;
			tabs = new ObservableCollection<ITab>();
			tabs.CollectionChanged += Tabs_CollectionChanged;
			Tabs = tabs; 
		}

		public ICommand ShowDialogConnectionWindowCommand { get; }
		public ICommand ShowPositionListCommand { get; }
		public ICommand ShowConsumablesListCommand { get; }
		public ICommand ShowSellsListCommand { get; }
		public ICommand ShowStatisticsCommand { get; }
		public ICollection<ITab> Tabs { get; }

		private void ShowStatistik()
		{
			Tabs.Add(new StatisticsViewModel());
		}

		private void ShowSellsList()
		{
			Tabs.Add(new SellsListViewModel());
		}

		private void ShowConsumablesList()
		{
			Tabs.Add(new ConsumablesListViewModel());
		}

		private void ShowPositionList()
		{
			Tabs.Add(new PositionListViewModel(dialogService, positionList));
		}

		private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			ITab tab;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					tab = (ITab) e.NewItems[0];
					tab.CloseRequested += OnTabCloseRequested;
					break;
				case NotifyCollectionChangedAction.Remove:
					tab = (ITab) e.OldItems[0];
					tab.CloseRequested -= OnTabCloseRequested;
					break;
			}
		}

		private void OnTabCloseRequested(object sender, EventArgs e)
		{
			Tabs.Remove((ITab) sender);
		}


		private void ShowDialogConnectionWindow()
		{
			var viewModel = new ConnectionViewModel();

			bool? result = dialogService.ShowDialog(viewModel).Result;

			if (result.HasValue)
				if (result.Value)
				{
					// Accepted
				}
				else
				{
					// Canceled
				}

		}
	}
}
