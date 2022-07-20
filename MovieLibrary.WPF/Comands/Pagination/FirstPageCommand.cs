﻿using MovieLibrary.WPF.ViewModels;
using System;
using System.Windows.Input;

namespace MovieLibrary.WPF.Comands.Pagination
{
    internal class FirstPageCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public FirstPageCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.CurrentPageIndex != 0;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            viewModel.ShowFirstPage();
        }
    }
}
