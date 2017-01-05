using UnityEngine;
using System.Collections;
using System;

public class Node : IHeapItem<Node>
{
    public Node parent;
    public Vector3 worldPosition;
    public float nodeSize;
    public int x;
    public int y;

    private int _heapIndex = 0;
    public int HeapIndex
    {
        get
        {
            return _heapIndex;
        }
        set
        {
            _heapIndex = value;
        }
    }

    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(Vector3 worldPoint, int _x, int _y, float _nodeSize)
    {
        worldPosition = worldPoint;
        nodeSize = _nodeSize;
        x = _x;
        y = _y;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
    public override string ToString()
    {
        return x.ToString() + " , " + y.ToString();
    }
}