using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformTrayIcon
{
    public static class PlatformExtensions
    {
        public static Windows.Graphics.RectInt32 GetCursorPosition(int windowWidth, int windowHeight)
        {
            var point = Cursor.Position;
            var x = point.X - (windowWidth / 2);
            var y = point.Y - (windowHeight + 50);
            var width = windowWidth;
            var height = windowHeight;
            return new Windows.Graphics.RectInt32(x, y, width, height);
        }
    }
}
