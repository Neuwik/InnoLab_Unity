using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;

    public bool StartUML()
    {
        Debug.Log("Started " + name);
        if (Tree?.Run(this) ?? false)
        {
            Debug.Log(name + " is done");
            return true;
        }
        else
        {
            Debug.Log(name + " is crashed");
            return false;
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
}
