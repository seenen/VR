using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public sealed class ObjManager
{
    public static void Output(_Mesh mesh, string filename)
    {
        using (StreamWriter sw = File.CreateText(filename))
        {
            sw.Write("# {0} vertex {1} face\n", mesh.Vertices.Count, mesh.Faces.Count);

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                sw.Write("v {0} {1} {2} \n", mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
            }

            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                sw.Write("f {0} {1} {2} \n", mesh.Faces[i].P0Index, mesh.Faces[i].P1Index, mesh.Faces[i].P2Index);
            }

            sw.Close();//关闭流
        }
    }

#region Mesh
    public static void Output(Mesh m, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString(m));
        }

        //using (StreamWriter sw = File.CreateText(filename))
        //{
        //    sw.Write("# {0} vertex {1} face\n", mesh.vertexCount, mesh.triangles.Length);

        //    for (int i = 0; i < mesh.vertexCount; i++)
        //    {
        //        sw.Write("v {0} {1} {2} \n", mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z);
        //    }

        //    for (int i = 0; i < mesh.triangles.Length; i += 3)
        //    {
        //        sw.Write("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2} \n", mesh.triangles[i] + 1, mesh.triangles[i+1] + 1, mesh.triangles[i+2] + 1);

        //        i += 3;
        //    }

        //    sw.Close();//关闭流
        //}
    }

    public static string MeshToString(Mesh m)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(m.name).Append("\n");
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        sb.Append("\n");
        for (int i = 0; i < m.triangles.Length; i += 3)
        {
            sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                m.triangles[i] + 1, m.triangles[i + 1] + 1, m.triangles[i + 2] + 1));
        }
        //for (int material = 0; material < m.subMeshCount; material++)
        //{
        //    sb.Append("\n");
        //    sb.Append("usemtl ").Append(mats[material].name).Append("\n");
        //    sb.Append("usemap ").Append(mats[material].name).Append("\n");

            //int[] triangles = m.GetTriangles(material);
            //for (int i = 0; i < triangles.Length; i += 3)
            //{
            //    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
            //        triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            //}
        //}
        return sb.ToString();
    }
#endregion
}
