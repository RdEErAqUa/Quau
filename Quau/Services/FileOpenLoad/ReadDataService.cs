using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quau.Services.FileOpenLoad
{
    static class ReadDataService
    {

        static public string ReadData(String PATH)
        {
            if (File.Exists(PATH))
            {
                using (StreamReader reader = new StreamReader(PATH))
                {
                    return reader.ReadToEnd();
                }
            }
            else
                return null;
        }
    }
}
