using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;

public class ClassCreationTimer : MonoBehaviour
{
    public int classesToCreate;
    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            createClasses();
        }
	}

    void createClasses()
    {
        Stopwatch _stopWatch = new Stopwatch();
        UnityEngine.Debug.Log("creating classes");
        _stopWatch.Start();
        for (int i = 0; i < classesToCreate; i++)
        {
            TestClass temp = new TestClass();
        }
        _stopWatch.Stop();
        TimeSpan ts = _stopWatch.Elapsed;
        UnityEngine.Debug.Log(ts.TotalMilliseconds);
    }
}

public class TestClass
{

}
