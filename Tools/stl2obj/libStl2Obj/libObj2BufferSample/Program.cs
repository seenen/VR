using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libObj2Buffer;
using System.IO;

namespace libObj2BufferSample
{
    class Program
    {
        static void Main(string[] args)
        {
            long before = System.DateTime.Now.Ticks;

            string path = "F:/GitHub/VR/Tools/stl2obj/Resources/BatStl/0_dannang_after.obj";

            string content = File.ReadAllText(path);

            Console.WriteLine("ReadAllText 16 " + (System.DateTime.Now.Ticks - before) / 10000000.0);

            before = System.DateTime.Now.Ticks;

            //
            libObj2Buffer.GeometryBuffer buffer = libObj2Buffer.Buffer.ObjContent2Buffer(content);

            Console.WriteLine("GeometryBuffer " + (System.DateTime.Now.Ticks - before) / 10000000.0);

            Console.ReadLine();
        }
    }
}
