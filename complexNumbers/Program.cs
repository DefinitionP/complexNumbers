using System;
using System.IO.Ports;
using System.Threading;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using complexNumbers;
using System.Diagnostics;
using System.Reflection.Metadata;

class Program
{
    // *** Variable tools
    public static string resultfolderpath = @"C:\Users\Ни кита\Desktop\complex3";
    public static string resultfilename = "complex.bmp";
    public static int imgSize = 500;
    public static int dotSize = 1; // 1 or 3
    public static double inputSineFrequency = 25; // Hz
    public static double windingFrequency = 50; // Hz
    public static double spins = 1.0;
    public static int textRatio = 40;
    public static int GainAdj = 100;
    public static int Ksplit = 2;

    public static Font font = new("Ubuntu", imgSize / textRatio);
    public static SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
    // ***

    public static int origin = 0;
    public static int split = 0;
    public static List<vector> vectors = new List<vector>();
    public static List<double> data = new List<double>();
    public static PointF point = new PointF(10, imgSize / textRatio);
    public static double frequencyRatio = 0;

    public static List<vector> centers = new List<vector>(); 

    static void Main(string[] args)
    {
        inputSineFrequency = 5;

        double minWindingfreq = 0.0;
        double maxWindingfreq = 10.0;
        double step = 0.5;

        int fileCount = 1; 

        int amount = (int)Math.Floor((maxWindingfreq - minWindingfreq) / step);
        //Bitmap image = new(imgSize, imgSize);
        //Cycle(10000, image);

        Stopwatch stopWatch = new Stopwatch();
        for (double i = minWindingfreq; i <= maxWindingfreq; i += step)
        {
            stopWatch.Start();
            Bitmap image = new(imgSize, imgSize);
            windingFrequency = i;


            Loop(fileCount, image);
            stopWatch.Stop();
            int timeLeft = (int)(stopWatch.Elapsed.TotalMilliseconds / 1000.0 * amount);
            fileCount++;
            Console.WriteLine("Step " + (fileCount - 1) + " done. " + amount + " left (about " + timeLeft + " seconds).");
            amount--;
            stopWatch.Reset();
        }
    }
    static void Loop(int count, Bitmap image)
    {

        for (int i = 0; i < imgSize; i++)
        {
            for (int q = 0; q < imgSize; q++)
            {
                image.SetPixel(i, q, Color.FromArgb(255, 255, 255));
            }
        }
        draw.drawOrigin(image);
        countSplitting();
        draw.drawCircle(image, imgSize / 2 - 2, Color.FromArgb(200, 200, 200));
        vectors.Clear();

        preparingData();
        draw.drawData(image, GainAdj, Color.FromArgb(255, 128, 128));

        //var vec1 = countMassCenter(image, vectors, 1);
        //var vec2 = countMassCenter(image, vec1, 2);
        //var vec3 = countMassCenter(image, vec2, 4);
        //var vec4 = countMassCenter(image, vec3, 8);
        //Mass(vectors);
        vector.CountMass(image, vectors);
        //vector.drawCenters(image);
        vectors.Clear();
        data.Clear();

        draw.drawInfo(image);
        image.Save(resultfolderpath + @"\" + count + resultfilename, ImageFormat.Bmp);

    }
    static double Function(double x)
    {
        double val;
        val = Math.Sin(Math.PI / 2 + x * frequencyRatio); // Н.ф. - п/2, синус колеблется от -1 до 1.
        //val = (Math.Sin(Math.PI / 2 + x * frequencyRatio) + 1.0) / 2.0; // Н.ф. - п/2, синус колеблется от 0 до 1.

        //val = (Math.Sin(Math.PI / 2.0 + x * frequencyRatio) + Math.Sin(Math.PI / 2.0 + x  / 2.0 * frequencyRatio)) / 2.0;
        //val = Meander(x);
        
        //Console.WriteLine(val);
        return val;
    }

    static double Meander (double arg)
    {
        double output = Math.Sin(Math.PI / 2 + arg * frequencyRatio);
        if (output >= 0) output = 0.789;
        if (output < 0) output = -0.789;
        return output;
    }

    static void countSplitting()
    {
        double circle = imgSize * 3.1415;
        split = (int)(circle * Ksplit);
        if ((split % 2) != 0) split++;
        //split = 360;
    }
    public static double currentAngleRAD(int currentSplit)
    {
        double deg = currentSplit * 2.0 * Math.PI / split;
        return deg;
    }
    static void preparingData()
    {
        frequencyRatio = windingFrequency / inputSineFrequency;
        
        for (int i = 0; i < split * spins; i++)
        {
            double angle = currentAngleRAD(i);
            double value = Function(angle);
            data.Add(value);
        }
        foreach (var val in data)
        {
            vector vector = new vector(currentAngleRAD(data.IndexOf(val)), GainAdj * val);
        }
    }
    //static List<vector> countMassCenter(Bitmap image, List<vector> vect, int depth)
    //{
    //    //dotSize = 1;
    //    Console.WriteLine(vect.Count);
    //    Console.WriteLine(split);
    //    List<vector> output = new List<vector>();
    //    int threshold = vect.Count;
    //    int delta = split / depth;
    //    while (threshold > 0)
    //    {
    //        var vec = vect[0];
    //        var opposite = vec;
    //        try { opposite = vect[delta / 2]; }
    //        catch { /*Console.WriteLine("!!!");*/ continue; }
    //        int xCoord = (int)(vec.x - (vec.x + opposite.x) / 2) + origin;
    //        int yCoord = (int)(vec.iy - (vec.iy + opposite.iy) / 2) + origin;
    //        output.Add(new vector(xCoord - origin, yCoord - origin, 1));
    //        if (depth == 1) image.SetPixel(xCoord, yCoord, Color.FromArgb(0, 0, 255));
    //        if (depth == 2) image.SetPixel(xCoord, yCoord, Color.FromArgb(0, 255, 0));
    //        if (depth == 3) image.SetPixel(xCoord, yCoord, Color.FromArgb(0, 255, 255));
    //        if (depth == 4) image.SetPixel(xCoord, yCoord, Color.FromArgb(255, 0, 0));

    //        //image.SetPixel((int)vec.x + origin, (int)vec.iy + origin, Color.FromArgb(0, 255, 0));
    //        //image.SetPixel((int)opposite.x + origin, (int)opposite.iy + origin, Color.FromArgb(0, 255, 0));

    //        vect.Remove(vec);
    //        vect.Remove(opposite);
    //        threshold = vect.Count;
    //        delta -= 2;
    //    }
    //    Console.WriteLine("count mass done");
    //    //dotSize = 3;
    //    return output;
    //}
    //static void Mass (List<vector> vectors)  // Не работает!
    //{
    //    int sum1, sum2;
    //    for (int i = 0; i < imgSize; i++)
    //    {
    //        double q = i * imgSize / split;
    //        sum1 = 0; sum2 = 0;
    //        foreach (var vec in vectors)
    //        {
    //            if (((int)vec.x + origin) >= i) sum1++;
    //            else sum2++;
    //        }
    //        if (Math.Abs(sum1 - sum2) < 5) Console.Write("match ");
    //    }
    //}
}