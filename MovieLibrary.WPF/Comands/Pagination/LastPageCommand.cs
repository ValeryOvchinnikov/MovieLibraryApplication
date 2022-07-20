using MovieLibrary.WPF.ViewModels;
using System;
using System.Windows.Input;

namespace MovieLibrary.WPF.Comands.Pagination
{
    internal class LastPageCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public LastPageCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.CurrentPage != viewModel.TotalPages;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            viewModel.ShowLastPage();
        }
    }
}
