using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.FileOpenLoad.Base
{
    internal interface IDialogService
    {
        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFileDialog();
        bool SaveFileDialog();
    }
}
