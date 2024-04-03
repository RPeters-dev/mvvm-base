using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MVVM.Base.WinUI;

namespace MVVM.Base
{
    public class SelectFolderCommand : RoutedCommand
    {
        protected override void Execute(object parameter)
        {

            OpenFolderDialog fd = new OpenFolderDialog();
            fd.DirectoryPath = parameter?.ToString();
            if (fd.ShowDialog() != true)
                return;

            CommandTarget.Execute(fd.DirectoryPath);
        }
    }
    public class SelectFileCommand : RoutedCommand
    {

        protected override void Execute(object parameter)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() != true)
                return;

            CommandTarget.Execute(fd.FileName);
        }
    }
}
