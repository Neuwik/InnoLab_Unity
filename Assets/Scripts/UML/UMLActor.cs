using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public enum EUMLActorState { Ready = 0, Running, Stopped, Crashed, Done }

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;
    public float TickRate = 1; //Actions per second
    private Vector3 startPosition;
    public LayerMask DangerLayer;

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
        GetComponent<PlayerHealth>()?.Reset();

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

    #region Actions
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
    public void MoveDown()
    {
        GetComponent<PlayerController>()?.Move(Vector3.back);
    }
    public void MoveLeft()
    {
        GetComponent<PlayerController>()?.Move(Vector3.left);
    }
    public void MoveRight()
    {
        GetComponent<PlayerController>()?.Move(Vector3.right);
    }

    public void CollectGarbage()
    {
        GetComponent<GarbageCollector>()?.CollectGarbage();
    }
    #endregion

    #region Conditions
    public bool SomeCondition()
    {
        return true;
    }

    public bool RandomCondition()
    {
        return Random.value > 0.5;
    }

    private bool IsDangerInDirection(Vector3 direction)
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc == null)
        {
            return false;
        }
        RaycastHit hit;
        Vector3 pos = pc.GetNextMovePointPosition(direction) + Vector3.down;
        float distance = Vector3.Distance(transform.position, pos);
        Vector3 posDirection = pos - transform.position;
        if (Physics.Raycast(transform.position, posDirection, out hit, distance, DangerLayer))
        {
            //Debug.DrawLine(transform.position, hit.point, Color.red, 10f);
            //Debug.Log($"Is Danger {hit.transform.gameObject.name}");
            return true;
        }
        //Debug.DrawLine(transform.position, pos, Color.green, 10f);
        return false;
    }

    public bool IsUpDanger()
    {
        return IsDangerInDirection(Vector3.forward);
    }

    public bool IsDownDanger()
    {
        return IsDangerInDirection(Vector3.back);
    }

    public bool IsLeftDanger()
    {
        return IsDangerInDirection(Vector3.left);
    }

    public bool IsRightDanger()
    {
        return IsDangerInDirection(Vector3.right);
    }
    #endregion
}
