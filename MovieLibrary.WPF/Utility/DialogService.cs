using Microsoft.Win32;

namespace MovieLibrary.WPF.Utility
{
    internal class DialogService  : IDialogService
    {
        public string OpenFileDialog(string defaultExt, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = defaultExt;
            ofd.Filter = filter;

            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }

        public string SaveFileDiaolog(string defaultExt, string filter)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = defaultExt;
            sfd.Filter = filter;

            if (sfd.ShowDialog() == true)
            {
                return sfd.FileName;
            }
            return string.Empty;
        }
    }
}
