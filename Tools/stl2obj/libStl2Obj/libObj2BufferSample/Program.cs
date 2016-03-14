using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libObj2Buffer;
using System.IO;
using System.Runtime.InteropServices;

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

            UseCSharpDll(content);

            UseCPPDll(content);

            Console.ReadLine();
        }

        static void UseCSharpDll(string content)
        {
            long before = Environment.TickCount;

            //
            libObj2Buffer.GeometryBuffer buffer = libObj2Buffer.Buffer.ObjContent2Buffer(content);

            Console.WriteLine("GeometryBuffer " + (Environment.TickCount - before) / 10000000.0);

        }

        [DllImport("F:/GitHub/VR/Tools/stl2obj/libStl2Obj/dllCppTest/x64/Debug/libObj2BufferCpp.dll", 
                    EntryPoint = "BuffContent", 
                    ExactSpelling = false, 
                    CallingConvention = CallingConvention.StdCall)]
        private extern static int _BuffContent(string filename);

        static void UseCPPDll(string content)
        {

            long before = Environment.TickCount;

            //
            int buffer = _BuffContent(content);

            Console.WriteLine("_BuffContent " + (Environment.TickCount - before) / 10000000.0);
        }
    }
}
