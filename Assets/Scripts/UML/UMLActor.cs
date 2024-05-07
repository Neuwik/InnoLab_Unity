using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.PlayerSettings;

public enum EUMLActorState { Ready = 0, Running, Stopped, Crashed, Done }

public class UMLActor : MonoBehaviour
{
    public UMLTree Tree;
    public float TickRate = 1; //Actions per second
    private Vector3 startPosition;
    public LayerMask DangerLayer;
    public LayerMask GarbageLayer;

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
        //Debug.Log("UML is Stopping");
        SetActorState(EUMLActorState.Stopped);
        Reset();
    }

    public void Reset()
    {
        GameManager.Instance.Console.Log(State.ToString(), name, $"Is resetting");
        //Debug.Log("UML Actor is Resetting");
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
        GameManager.Instance.Console.Log(newState.ToString(), name, $"State changed from {State} to {newState}");
        //Debug.Log($"UML Actor ({name}) State changed from {State} to {newState}");
        State = newState;
    }

    #region Actions
    public void DoNothing()
    {
        //Debug.Log(name + " is doing nothing");
    }

    #region Move
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
    #endregion

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

    #region IsDanger
    public bool IsUpDanger()
    {
        return RaycastInMoveDirection(Vector3.forward, DangerLayer);
    }

    public bool IsDownDanger()
    {
        return RaycastInMoveDirection(Vector3.back, DangerLayer);
    }

    public bool IsLeftDanger()
    {
        return RaycastInMoveDirection(Vector3.left, DangerLayer);
    }

    public bool IsRightDanger()
    {
        return RaycastInMoveDirection(Vector3.right, DangerLayer);
    }

    public bool IsThisDanger()
    {
        return RaycastOnPosition(transform.position + Vector3.down, DangerLayer);
    }
    #endregion

    #region IsGarbage
    public bool IsUpGarbage()
    {
        return RaycastOnMoveDirectionEndpoint(Vector3.forward, GarbageLayer);
    }

    public bool IsDownGarbage()
    {
        return RaycastOnMoveDirectionEndpoint(Vector3.back, GarbageLayer);
    }

    public bool IsLeftGarbage()
    {
        return RaycastOnMoveDirectionEndpoint(Vector3.left, GarbageLayer);
    }

    public bool IsRightGarbage()
    {
        return RaycastOnMoveDirectionEndpoint(Vector3.right, GarbageLayer);
    }

    public bool IsThisGarbage()
    {
        return RaycastOnPosition(transform.position + Vector3.down, GarbageLayer);
    }
    #endregion

    #endregion

    private bool RaycastInMoveDirection(Vector3 direction, LayerMask layer)
    {
        // Raycast auf dem "layer" von dem Actor zu dem Feld wo der Actor stehen wird, wenn er in die "direction" geht
        PlayerController pc = GetComponent<PlayerController>();
        if (pc == null)
        {
            return false;
        }
        Vector3 pos = pc.GetNextMovePointPosition(direction) + Vector3.down;
        float distance = Vector3.Distance(transform.position, pos);
        Vector3 posDirection = pos - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, posDirection, out hit, distance, layer))
        {
            //Debug.DrawLine(transform.position, hit.point, Color.red, 10f);
            //Debug.Log($"Is Danger {hit.transform.gameObject.name}");
            return true;
        }
        //Debug.DrawLine(transform.position, pos, Color.green, 10f);
        return false;
    }

    private bool RaycastOnMoveDirectionEndpoint(Vector3 direction, LayerMask layer)
    {
        // Raycast auf dem "layer" auf das Feld wo der Actor stehen wird, wenn er in die "direction" geht
        PlayerController pc = GetComponent<PlayerController>();
        if (pc == null)
        {
            return false;
        }
        Vector3 pos = pc.GetNextMovePointPosition(direction) + Vector3.down;
        return RaycastOnPosition(pos, layer);
    }

    private bool RaycastOnPosition(Vector3 position, LayerMask layer)
    {
        // Raycast auf dem "layer" auf das Feld an der "position"
        float hight = 10;
        Vector3 abovePosition = position + Vector3.up * hight;
        RaycastHit hit;
        if (Physics.Raycast(abovePosition, Vector3.down, out hit, hight, layer))
        {
            //Debug.DrawLine(position, hit.point, Color.red, 10f);
            //Debug.Log($"Is Danger {hit.transform.gameObject.name}");
            return true;
        }
        //Debug.DrawLine(abovePosition, position, Color.green, 10f);
        return false;
    }
}
