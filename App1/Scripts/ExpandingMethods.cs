using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;

namespace App1.Scripts
{
    public static class ExpandingMethods
    {

        public static Color GetColorFromHex(this string hexString)
        {

            if (!System.Text.RegularExpressions.Regex.IsMatch(hexString, @"[#]([0-9]|[a-f]|[A-F]){6}\b"))
                throw new ArgumentException();

            hexString = hexString.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            if (hexString.Length == 8)
            {
                a = byte.Parse(hexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            r = byte.Parse(hexString.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hexString.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hexString.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public static void changeUiFromAnotherThread(Action action)
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        action?.Invoke();
                    }
                );
        }

    }

  


}
