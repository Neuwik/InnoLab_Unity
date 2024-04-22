using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EUMLActorState { Ready = 0, Running, Stopped, Crashed, Done }

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;
    public float TickRate = 2; //Actions per second
    private Vector3 startPosition;

    public EUMLActorState State { get; private set; } = EUMLActorState.Ready;
    public bool UMLRunning
    {
        get
        {
            switch (State)
            {
                case EUMLActorState.Running:
                    return true;
                case EUMLActorState.Ready:
                case EUMLActorState.Stopped:
                case EUMLActorState.Crashed:
                case EUMLActorState.Done:
                default:
                    return false;
            }
        }
    }

    public IEnumerator StartUML()
    {
        startPosition = transform.position;
        SetActorState(EUMLActorState.Running);
        Debug.Log("Started " + name);

        yield return Tree?.Run(this);

        switch (State)
        {
            case EUMLActorState.Running:
                Debug.Log(name + " is done");
                SetActorState(EUMLActorState.Done);
                break;
            case EUMLActorState.Crashed:
                Debug.Log(name + " has crashed");
                SetActorState(EUMLActorState.Stopped);
                break;
            case EUMLActorState.Stopped:
            case EUMLActorState.Ready:
            case EUMLActorState.Done:
            default:
                Debug.Log(name + " was not running");
                break;
        }
    }

    public void StopUML()
    {
        Debug.Log("UML is Stopping");
        SetActorState(EUMLActorState.Stopped);
        Reset();
    }

    public void Reset()
    {
        Debug.Log("UML Actor is Resetting");
        transform.position = startPosition;
        GetComponent<PlayerController>()?.Reset();

        SetActorState(EUMLActorState.Ready);
    }

    public IEnumerator WaitForTick()
    {
        if (TickRate <= 0)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {

            yield return new WaitForSeconds(1/TickRate);
        }
    }

    public void SetActorState(EUMLActorState newState)
    {
        Debug.Log($"UML Actor ({name}) State changed from {State} to {newState}");
        State = newState;
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
