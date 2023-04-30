using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Program;

namespace complexNumbers
{
    public class draw
    {
        public static void drawData(Bitmap image, int rad, Color color)
        {
            foreach (var vec in vectors)
            {
                try { drawDot(image, (int)vec.x + origin, (int)vec.iy + origin, color, dotSize); }
                catch { Console.WriteLine("dot is out of range!"); continue; }
            }
        }
        public static void drawOrigin(Bitmap image)
        {
            origin = imgSize / 2 - 1;
            for (int i = 0; i < imgSize; i++)
            {
                image.SetPixel(origin, i, Color.FromArgb(0, 0, 0));
                image.SetPixel(i, origin, Color.FromArgb(0, 0, 0));
            }
        }
        public static void drawCircle(Bitmap image, int rad, Color color)
        {
            int buffer = dotSize;
            dotSize = 3;
            int len = 0;
            for (int i = 0; i < split; i++)
            {
                //len = map(i, split, rad);
                len = rad;
                //Console.WriteLine(len);
                vector vector = new vector(currentAngleRAD(i), len);
            }

            foreach (var vec in vectors)
            {
                try { drawDot(image, (int)vec.x + origin, (int)vec.iy + origin, color, dotSize); }
                catch { Console.WriteLine("dot is out of range!");  continue; }
            }
            dotSize = buffer;
        }
        public static void drawDot(Bitmap image, int x, int y, Color color, int dot)
        {
            if (dot == 1)
            {
                image.SetPixel(x, y , color);
                return;
            }
            if (dot == 3)
            {
                for (int i = x - 1; i < (x + 2); i++)
                {
                    for (int q = y - 1; q < (y + 2); q++)
                    {
                        try { image.SetPixel(i, q, color); }
                        catch { continue; }
                    }
                }
                return;
            }
        }
        public static void drawInfo (Bitmap image)
        {
            string info = "winding / input = " + string.Format("{0:N4}", windingFrequency / inputSineFrequency);
            string info2 = "input = " + string.Format("{0:N4}", inputSineFrequency);
            string info3 = "winding = " + string.Format("{0:N4}", windingFrequency);
            string info4 = "spins  = " + spins;

            using (Graphics g = Graphics.FromImage(image))
            {
                point.Y = imgSize / textRatio;
                g.DrawString(info, font, brush, point);
                point.Y += font.Height;
                g.DrawString(info2, font, brush, point);
                point.Y += font.Height;
                g.DrawString(info3, font, brush, point);
                point.Y += font.Height;
                g.DrawString(info4, font, brush, point);
            }
        }
    }
}
