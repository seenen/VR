using System;
using UnityEngine;
using System.Globalization;

namespace libObj2Buffer
{
    public sealed class Buffer
    {
        private const string O = "o";
        private const string G = "g";
        private const string V = "v";
        private const string VT = "vt";
        private const string VN = "vn";
        private const string F = "f";
        private const string MTL = "mtllib";
        private const string UML = "usemtl";
        private static string mtllib;

        public static GeometryBuffer ObjContent2Buffer(string data)
        {
            GeometryBuffer buffer = new GeometryBuffer();

            string[] lines = data.Split("\n".ToCharArray());

            //Debug.Log("lines = " + lines.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];

                if (l.IndexOf("#") != -1) l = l.Substring(0, l.IndexOf("#"));
                string[] p = l.Split(" ".ToCharArray());

                switch (p[0])
                {
                    case O:
                        buffer.PushObject(p[1].Trim());
                        break;
                    case G:
                        buffer.PushGroup(p[1].Trim());
                        break;
                    case V:
                        buffer.PushVertex(new Vector3(cf(p[1]), cf(p[2]), cf(p[3])));
                        break;
                    case VT:
                        buffer.PushUV(new Vector2(cf(p[1]), cf(p[2])));
                        break;
                    case VN:
                        buffer.PushNormal(new Vector3(cf(p[1]), cf(p[2]), cf(p[3])));
                        break;
                    case F:
                        for (int j = 1; j < p.Length; j++)
                        {
                            string[] c = p[j].Trim().Split("/".ToCharArray());
                            FaceIndices fi = new FaceIndices();
                            fi.vi = ci(c[0]) - 1;
                            if (c.Length > 1 && c[1] != "") fi.vu = ci(c[1]) - 1;
                            if (c.Length > 2 && c[2] != "") fi.vn = ci(c[2]) - 1;
                            buffer.PushFace(fi);
                        }
                        break;
                    case MTL:
                        mtllib = p[1].Trim();
                        break;
                    case UML:
                        buffer.PushMaterialName(p[1].Trim());
                        break;
                }
            }

            return buffer;
        }

        private static float cf(string v)
        {
            return Convert.ToSingle(v.Trim(), new CultureInfo("en-US"));
        }

        private static int ci(string v)
        {
            return Convert.ToInt32(v.Trim(), new CultureInfo("en-US"));
        }

    }
}
