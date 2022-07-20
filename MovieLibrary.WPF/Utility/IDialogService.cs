namespace MovieLibrary.WPF.Utility
{
    internal interface IDialogService
    {
        string OpenFileDialog(string defaultExt, string filter);
        string SaveFileDiaolog(string defaultExt, string filter);
    }
}
