using UnityEngine;
using System.Collections;
using System.IO;

public sealed  class PlyManager
{
    public static void Output(_Mesh mesh, string filename)
    {
        using (StreamWriter sw = File.CreateText(filename))
        {
            sw.Write("ply\n");
            sw.Write("format ascii 1.0\n");
            sw.Write("comment VCGLIB generated\n");
            sw.Write("element vertex {0}\n", mesh.Vertices.Count);
            sw.Write("property float x\n");
            sw.Write("property float y\n");
            sw.Write("property float z\n");
            sw.Write("property uchar red\n");
            sw.Write("property uchar green\n");
            sw.Write("property uchar blue\n");
            sw.Write("element face {0}\n", mesh.Faces.Count);
            sw.Write("property list int int vertex_indices\n");
            sw.Write("end_header\n");

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                AWriteV(sw, mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z, 255, 255, 255);
            }
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                AWriteF(sw, mesh.Faces[i].P0Index, mesh.Faces[i].P1Index, mesh.Faces[i].P2Index);
            }

            sw.Close();//关闭流
        }
    }

    static void AWriteV(StreamWriter sw, double v1, double v2, double v3, System.Byte r, System.Byte g, System.Byte b)
    {
        sw.Write("{0:0.00} {1:0.0} {2:0.0} {3} {4} {5}\n", v1, v2, v3, r, g, b);
        //sw.Write("%.2f %.2f %.2f %d %d %d\n", v1, v2, v3, r, g, b);
    }

    static void AWriteF(StreamWriter sw, long i1, long i2, long i3)
    {
        sw.Write("{0} {1} {2} {3}\n", 3, i1, i2, i3);
        //sw.Write("%d %d %d %d\n", 3, i1, i2, i3);
    }
}
