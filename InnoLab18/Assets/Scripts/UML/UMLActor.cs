using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;
    public bool UMLRunning { get; private set; }
    private Vector3 startPosition;

    public float tickRate = 2; //Actions per second

    public IEnumerator StartUML()
    {
        startPosition = transform.position;
        UMLRunning = true;
        Debug.Log("Started " + name);

        yield return Tree?.Run(this);
        
        if (true) //If UML success
        {
            UMLRunning = false;
            Debug.Log(name + " is done");
        }
        else // If UML crash
        {
            UMLRunning = false;
            Debug.Log(name + " is crashed");
        }
    }

    public bool StopUML()
    {
        Debug.Log("UML is Stopping");
        UMLRunning = false;
        Reset();
        Debug.Log("UML has Stopped");
        return true;
    }

    public void Reset()
    {
        Debug.Log("UML Actor is Resetting");
        transform.position = startPosition;
        GetComponent<PlayerController>()?.Reset();
    }

    public IEnumerator WaitForTick()
    {
        if (tickRate <= 0)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {

            yield return new WaitForSeconds(1/tickRate);
        }
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

    public bool SomeCondition()
    {
        return true;
    }

    public bool RandomCondition()
    {
        return Random.value > 0.5;
    }
}
