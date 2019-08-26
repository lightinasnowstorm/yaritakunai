//#define FORCE_FIND

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace やりたくない
{
    /// <summary>
    /// This is a helper for accessing files.
    /// </summary>
    internal class FileHelper
    {
        public static FileStream GetStream(string path, FileMode mode, FileAccess access, FileShare share = FileShare.Read)
        {
#if !FORCE_FIND
            try
            {
#endif
                return new FileStream(path, mode, access, share);
#if !FORCE_FIND
            }
            catch
            {
                return null;
            }
#endif
        }
    }
}
