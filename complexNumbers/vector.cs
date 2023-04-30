using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using static Program;

namespace complexNumbers
{
    public class vector
    {
        public double x {  get; set; }
        public double iy { get; set; }
        public double argRAD { get; set; }
        public double length { get; set; }

        public vector(double argRAD, double length)
        {
            if ((length == 0) || (argRAD == 0))
            {
                this.argRAD = 0;
                this.length = 0;
            }
            else
            {
                this.argRAD = argRAD;
                this.length = length;
                x = 1 + Math.Cos(argRAD) * length;
                iy = 1 + Math.Sin(argRAD) * length;
            }
            vectors.Add(this);
        }

        public vector(double x, double iy, int q)
        {
            this.x = x;
            this.iy = iy;
            //argRAD = Math.Atan(Math.Abs(iy / x));
            //length = iy / Math.Acos(argRAD);
            centers.Add(this);
        }
        public static void drawCenters(Bitmap image)
        {
            foreach (var v in centers)
            {
                draw.drawDot(image, (int)v.x, (int)v.iy, Color.FromArgb(255, 0, 0), 3);
            }
        }

        public static void CountMass(Bitmap image, List<vector> vects)
        {
            int x = center(vects, "x");
            int y = center(vects, "y");
            draw.drawDot(image, x, y, Color.FromArgb(255, 0, 0), 3);
            //vector v = new vector(x, y, 1);
        }

        public static int center (List<vector> vects, string mode)
        {
            int[] arr = new int[imgSize];
            for (int i = 0; i < imgSize; i++) { arr[i] = 0; }
            if (mode == "x")
            {
                foreach (vector vec in vects)
                {
                    int coord = (int)vec.x + origin;
                    arr[coord]++;
                }
            }
            else
            {
                foreach (vector vec in vects)
                {
                    int coord = (int)vec.iy + origin;
                    arr[coord]++;
                }
            }

            //CHECK!
            //int finalCount = 0;
            //for (int i = 0; i < imgSize; i++) { finalCount += arr[i]; }
            //if (finalCount != vects.Count) Console.WriteLine("WTF? error");
            // end CHECK

            //for (int i = 0; i < imgSize; i++) { Console.WriteLine(i + ": " + arr[i] + ", "); }

            int minDelta = (int)(split * spins); int Index = 0;
            for (int i = 0; i < imgSize; i++)
            {
                int sumLeft = 0; int sumCounted = 0;
                for (int c = 0; c < i; c++) { sumCounted += arr[c]; }
                for (int l = i + 1; l < imgSize; l++) { sumLeft += arr[l]; }
                int delta = Math.Abs(sumLeft - sumCounted);
                //Console.WriteLine(delta);
                if (delta < minDelta) { minDelta = delta; Index = i; }
            }
            return Index;
        }
    }
}
