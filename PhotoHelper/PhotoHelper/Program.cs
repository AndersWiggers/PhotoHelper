using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DTO;

namespace PhotoHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"E:\ICloudPhotos";

            

            List<string> allfiles = Directory
                .GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(r => !r.EndsWith(".json") && !r.EndsWith(".html") && !r.EndsWith(".MOV") && !r.EndsWith(".mp4") && !r.EndsWith(".ini")).ToList();

            List<ImageProperties> props = new List<ImageProperties>();

            var watch1 = Stopwatch.StartNew();

            foreach (var imagepath in allfiles)
            {
                Image image = new Bitmap(imagepath);

                PropertyItem[] propItems = image.PropertyItems;
                ASCIIEncoding ascii = new ASCIIEncoding();

                string manufacturer = ascii.GetString(propItems[1].Value);
                manufacturer = manufacturer.Remove(manufacturer.Length - 1);

                props.Add(new ImageProperties
                {
                    Manufacturer = manufacturer
                });

            }

            watch1.Stop();

            Console.WriteLine("foreach took " + watch1.ElapsedMilliseconds / 1000);

            var watch2 = Stopwatch.StartNew();

            Parallel.ForEach(allfiles, (currentfile) =>
            {
                Image image = new Bitmap(currentfile);

                PropertyItem[] propItems = image.PropertyItems;
                ASCIIEncoding ascii = new ASCIIEncoding();

                string manufacturer = ascii.GetString(propItems[1].Value);
                manufacturer = manufacturer.Remove(manufacturer.Length - 1);

                props.Add(new ImageProperties
                {
                    Manufacturer = manufacturer

                });

            });

            watch2.Stop();

            Console.WriteLine("parallel ForEack took " + watch2.ElapsedMilliseconds/1000);

            var query = props.GroupBy(r => r.Manufacturer);



        }
    }
}
