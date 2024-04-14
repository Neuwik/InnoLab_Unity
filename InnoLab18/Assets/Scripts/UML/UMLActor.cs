using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;
    public bool UMLRunning { get; private set; }
    private Vector3 startPosition;

    public bool StartUML()
    {
        startPosition = transform.position;
        UMLRunning = true;
        Debug.Log("Started " + name);
        if (Tree?.Run(this) ?? false)
        {
            UMLRunning = false;
            Debug.Log(name + " is done");
            return true;
        }
        else
        {
            UMLRunning = false;
            Debug.Log(name + " is crashed");
            return false;
        }
    }

    public bool StopUML()
    {
        Debug.Log("UML is Stopping");
        UMLRunning = false;
        ResetActor();
        Debug.Log("UML has Stopped");
        return true;
    }

    public void ResetActor()
    {
        Debug.Log("UML Actor is Resetting");
        transform.position = startPosition;
    }

    public void DoNothing()
    {
        Debug.Log(name + " is doing nothing");
    }

    public void DoSomething()
    {
        Debug.Log(name + " is doing something");
    }

    public void DoSomethingElse()
    {
        Debug.Log(name + " is doing something else");
    }

    public void MoveUp()
    {
        GetComponent<PlayerController>()?.Move(Vector3.forward);
    }
}
