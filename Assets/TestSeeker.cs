using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class TestSeeker : MonoBehaviour
{
    public bool showDiagnalLines;
    public Transform target;
    public LayerMask layer;
    private Grid grid;
    public GameManager manage;
    public List<Node> path = new List<Node>();
    public List<Node> jump = new List<Node>();
    public int counter = 0;
    public bool moving;
    
    CharacterController controller;

    void Start()
    {
        //manage = manager.GetComponent<GameManager>();
        controller = GetComponent<CharacterController>();
        grid = Grid.Instance;
    }

    void Update()
    {
        grid.showDebug = showDiagnalLines;
        if (!manage.pathOnUpdate)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                path = grid.GetPath(this.transform.position, target.position);
                jump = grid.GetJump();
            }
        }
        else
        {
            path = grid.GetPath(this.transform.position, target.position);
            jump = grid.GetJump();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            moving = !moving;
        }
        if (moving)
        {
            Vector3 direction = path[counter].worldPosition - transform.position;
            if (Vector3.Distance(transform.position + (Vector3.down * 1.5f), path[counter].worldPosition) < 0.5f)
            {
                if (counter < path.Count - 1)
                    counter++;
                else
                    moving = false;
            }
            direction = direction.normalized;
            controller.SimpleMove(direction * 10f);
        }

    }

    void OnDrawGizmos()
    {
        if (Selection.activeGameObject == this.gameObject)
        {
            //Gizmos.DrawWireCube(transform.position, new Vector3(10, 1f, 10));
            if (grid != null && grid.WorldGrid.Length > 0)
            {
                foreach (Node n in grid.WorldGrid)
                {
                    if (grid.UnwalkableNodes.Contains(n))
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(new Vector3(n.x, 1, n.y), Vector3.one);
                    }

                    if (grid.AStar && showDiagnalLines)
                    {
                        List<Node> nodes = grid.GetClosed();

                        if (nodes != null)
                        {
                            if (nodes.Contains(n))
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(new Vector3(n.x, 1, n.y), Vector3.one);
                            }
                        }
                    }
                    else
                    {
                        if (grid.GetJump() != null)
                        {
                            if (grid.GetJump().Contains(n))
                            {
                                Gizmos.color = Color.cyan;
                                Gizmos.DrawCube(new Vector3(n.x, 1, n.y), Vector3.one);
                                Gizmos.DrawLine(new Vector3(n.x, 1, n.y), new Vector3(n.parent.x, 1, n.parent.y));
                            }
                        }
                    }

                    if (path != null)
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(new Vector3(n.x, 1, n.y), Vector3.one);
                            Gizmos.DrawLine(new Vector3(n.x, 1, n.y), new Vector3(n.parent.x, 1, n.parent.y));
                        }
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(new Vector3(target.position.x, 1, target.position.z), .1f);
                    ///Handles.Label(new Vector3(n.x, 1, n.y), n.x.ToString() + "," + n.y.ToString());
                }
            }
        }
    }
}
