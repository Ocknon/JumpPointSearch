using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using System;

public class GameManager : MonoBehaviour
{
    public bool pathOnUpdate;
    public bool AStar;
    private Grid _grid;
    public Transform spawnPoint;
    private JumpPoint jp;
    Stopwatch sw = new Stopwatch();
    private int _totalTime;
    void Start()
    {
        //Player.Create(spawnPoint.position);
        _grid = Grid.Instance;
        _grid.AStar = AStar;
        jp = GetComponent<JumpPoint>();
        jp.InitializeGrid();
        EventHandler.Instance.Subscribe<PathTimerEvent>(_UpdateTime);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.Debug.Log("Total time for all paths to complete - " + _totalTime + " ms");
        }
    }
    private void _UpdateTime(PathTimerEvent e)
    {
        _totalTime += e.time;
    }
}
