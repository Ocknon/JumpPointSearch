using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class FindPath
{
    
    public List<Node> jumpNodes;

    private Node _startNode;
    private Node _targetNode;

    private Grid _grid;

    private Heap<Node> openSet;
    private HashSet<Node> openSetContainer;
    private HashSet<Node> closedSet;
    private bool _forced;

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
            UnityEngine.Debug.Log("Jump Point Path - Path found in : " + ts.Milliseconds + " ms");
            EventHandler.Instance.Broadcast(new PathTimerEvent(ts.Milliseconds));
            return _RetracePath();
        }
        else
        {
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            UnityEngine.Debug.Log("Jump Point Path - No path found in : " + ts.Milliseconds + " ms");
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
        jumpNodes = new List<Node>();
    }

    private bool _CalculateShortestPath()
    {
        Node currentNode;

        openSet.Add(_startNode);
        openSetContainer.Add(_startNode);

        while (openSet.Count > 0)
        {
            currentNode = openSet.RemoveFirst();
            openSetContainer.Remove(_startNode);
            
            if (currentNode == _targetNode)
            {
                return true;
            }
            else
            {
                closedSet.Add(currentNode);
                List<Node> Nodes = _GetSuccessors(currentNode);

                foreach (Node node in Nodes)
                {
                    jumpNodes.Add(node);

                    if (closedSet.Contains(node))
                        continue;

                    int newGCost = currentNode.gCost + _GetDistance(currentNode, node);
                    if (newGCost < node.gCost || !openSetContainer.Contains(node))
                    {
                        node.gCost = newGCost;
                        node.hCost = _GetDistance(node, _targetNode);
                        node.parent = currentNode;

                        if (!openSetContainer.Contains(node))
                        {
                            openSetContainer.Add(node);
                            openSet.Add(node);
                        }
                        else
                        {
                            openSet.UpdateItem(node);
                        }
                    }
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

    private List<Node> _GetSuccessors(Node currentNode)
    {
        Node jumpNode;
        List<Node> successors = new List<Node>();
        List<Node> neighbours = _grid.GetNeighbours(currentNode);

        foreach (Node neighbour in neighbours)
        {
            int xDirection = neighbour.x - currentNode.x;
            int yDirection = neighbour.y - currentNode.y;

            jumpNode = _Jump(neighbour, currentNode, xDirection, yDirection);

            if (jumpNode != null)
                successors.Add(jumpNode);
        }
        return successors;
    }
    private Node _Jump(Node currentNode, Node parentNode, int xDirection, int yDirection)
    {
        if (currentNode == null || !_grid.IsWalkable(currentNode.x, currentNode.y))
            return null;
        if (currentNode == _targetNode)
        {
            _forced = true;
            return currentNode;
        }

        _forced = false;
        if (xDirection != 0 && yDirection != 0)
        {
            if ((!_grid.IsWalkable(currentNode.x - xDirection, currentNode.y) && _grid.IsWalkable(currentNode.x - xDirection, currentNode.y + yDirection)) ||
                (!_grid.IsWalkable(currentNode.x, currentNode.y - yDirection) && _grid.IsWalkable(currentNode.x + xDirection, currentNode.y - yDirection)))
            {
                return currentNode;
            }


            Node nextHorizontalNode = _grid.GetNodeFromIndex(currentNode.x + xDirection, currentNode.y);
            Node nextVerticalNode = _grid.GetNodeFromIndex(currentNode.x, currentNode.y + yDirection);
            if (nextHorizontalNode == null || nextVerticalNode == null)
            {
                bool found = false;
                if (nextHorizontalNode != null && !_grid.UnwalkableNodes.Contains(_grid.GetNodeFromIndex(currentNode.x + xDirection, currentNode.y + yDirection)))
                {
                    found = true;
                }
                if (nextVerticalNode != null && !_grid.UnwalkableNodes.Contains(_grid.GetNodeFromIndex(currentNode.x + xDirection, currentNode.y + yDirection)))
                {
                    found = true;
                }

                if(!found)
                    return null;
            }

            if (_Jump(nextHorizontalNode, currentNode, xDirection, 0) != null || _Jump(nextVerticalNode, currentNode, 0, yDirection) != null)
            {
                if (!_forced)
                {
                    UnityEngine.Debug.Log(currentNode);
                    Node temp = _grid.GetNodeFromIndex(currentNode.x + xDirection, currentNode.y + yDirection);
                    if (temp != null && _grid.showDebug)
                        UnityEngine.Debug.DrawLine(new Vector3(currentNode.x, 1, currentNode.y), new Vector3(temp.x, 1, temp.y), Color.green, Mathf.Infinity);
                    return _Jump(temp, currentNode, xDirection, yDirection);
                }
                else
                {
                    return currentNode;
                }
            }
        }
        else
        {
            if (xDirection != 0)
            {
                if ((_grid.IsWalkable(currentNode.x + xDirection, currentNode.y + 1) && !_grid.IsWalkable(currentNode.x, currentNode.y + 1)) ||
                    (_grid.IsWalkable(currentNode.x + xDirection, currentNode.y - 1) && !_grid.IsWalkable(currentNode.x, currentNode.y - 1)))
                {
                    _forced = true;
                    return currentNode;
                }
            }
            else
            {
                if ((_grid.IsWalkable(currentNode.x + 1, currentNode.y + yDirection) && !_grid.IsWalkable(currentNode.x + 1, currentNode.y)) ||
                    (_grid.IsWalkable(currentNode.x - 1, currentNode.y + yDirection) && !_grid.IsWalkable(currentNode.x - 1, currentNode.y)))
                {
                    _forced = true;
                    return currentNode;
                }
            }
        }
        Node nextNode = _grid.GetNodeFromIndex(currentNode.x + xDirection, currentNode.y + yDirection);
        if (nextNode!= null && _grid.showDebug)
            UnityEngine.Debug.DrawLine(new Vector3(currentNode.x, 1, currentNode.y), new Vector3(nextNode.x, 1, nextNode.y), Color.green, Mathf.Infinity);
        return _Jump(nextNode, currentNode, xDirection, yDirection);
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