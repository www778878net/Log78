using System;
using System.Collections.Generic;
using System.Text;

namespace www778878net.log
{
    public interface IFileLog78
    {        
        string menu { get; set; }
        void LogToFile(string message="");
        void Clear();
    }
}
