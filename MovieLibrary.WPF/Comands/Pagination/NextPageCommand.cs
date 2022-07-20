﻿using MovieLibrary.WPF.ViewModels;
using System;
using System.Windows.Input;

namespace MovieLibrary.WPF.Comands.Pagination
{
    internal class NextPageCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public NextPageCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.TotalPages - 1 > viewModel.CurrentPageIndex;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            viewModel.ShowNextPage();
        }
    }
}
