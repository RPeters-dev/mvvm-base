

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Markup;

namespace MVVM.Base
{
    [MarkupExtensionReturnType(typeof(System.Windows.Input.Cursor))]
    public class xCursor : MarkupExtension
    {
        private static string defaultPath;
        public static bool cash;

        static xCursor()
        {
            defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Cursors");
        }

        public xCursor(string path)
        {
            Cursor = path;
        }

        public string Cursor { get; set; }
        public string CursorPath { get; set; } = defaultPath;
        public Dictionary<string, Cursor> Cach { get; private set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!Path.HasExtension(Cursor))
            {
                Cursor += ".cur";
            }
            var itemPath = Path.Combine(CursorPath, Cursor);
            Cursor result = null;
            _= cash && Cach.TryGetValue(itemPath, out result);
       
            if (result == null && File.Exists(itemPath))
            {
                result = new Cursor(itemPath, true);
            }

            return result;
        }
    }
}
