using UnityEngine;
using System.Collections;

public class _Triangle
{
    public long P0Index;
    public long P1Index;
    public long P2Index;

    public _Triangle(long p0index, long p1index, long p2index)
    {
        P0Index = p0index;
        P1Index = p1index;
        P2Index = p2index;
    }

    public _Triangle()
    {
        P0Index = -1;
        P1Index = -1;
        P2Index = -1;
    }

    public bool HasVertex(long index)
    {
        return P0Index == index || P1Index == index || P2Index == index;
    }

    public long GetOtherIndex(long i1, long i2)
    {
        return P0Index + P1Index + P2Index - i1 - i2;
    }
}
