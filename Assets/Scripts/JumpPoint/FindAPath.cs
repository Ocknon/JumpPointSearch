using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class FindAPath
{
    public event Action<int> timer = null;
    private Node _startNode;
    private Node _targetNode;

    private Grid _grid;

    private Heap<Node> openSet;
    private HashSet<Node> openSetContainer;
    private HashSet<Node> closedSet;
    public List<Node> closedList;

    public List<Node> GetPath(Node startNode, Node targetNode)
    {
        _startNode = startNode;
        _targetNode = targetNode;

        _Initialize();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        if (_CalculateShortestPath())
        {
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            UnityEngine.Debug.Log("A* Path - Path found in : " + ts.Milliseconds + " ms");
            EventHandler.Instance.Broadcast(new PathTimerEvent(ts.Milliseconds));
            return _RetracePath();
        }
        else
        {
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            UnityEngine.Debug.Log("A* Path - No path found in : " + ts.Milliseconds + " ms");
            EventHandler.Instance.Broadcast(new PathTimerEvent(ts.Milliseconds));
            return null;
        }
    }

    private void _Initialize()
    {
        _grid = Grid.Instance;
        openSet = new Heap<Node>(_grid.GridSize);
        openSetContainer = new HashSet<Node>();
        closedSet = new HashSet<Node>();
        closedList = new List<Node>();
    }

    private bool _CalculateShortestPath()
    {
        Node currentNode;

        openSet.Add(_startNode);
        openSetContainer.Add(_startNode);

        while (openSet.Count > 0)
        {
            currentNode = openSet.RemoveFirst();
            openSetContainer.Remove(currentNode);
            closedSet.Add(currentNode);
            closedList.Add(currentNode);

            if (currentNode == _targetNode)
                return true;

            foreach (Node neighbour in _grid.GetAStarNeighbours(currentNode))
            {
                if (_grid.UnwalkableNodes.Contains(neighbour) || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + _GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSetContainer.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = _GetDistance(neighbour, _targetNode);
                    neighbour.parent = currentNode;

                    if (!openSetContainer.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        openSetContainer.Add(neighbour);
                    }
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }
        return false;
    }

    private List<Node> _RetracePath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = _targetNode;

        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private int _GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
