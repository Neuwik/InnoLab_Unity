using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.LowLevel;

public enum EUMLActorState
{ 
    Ready = 0, 
    Running = 11, Stopping = 12, 
    Stopped = 21, Crashed = 22, Done = 23
}

public class UMLActor : MonoBehaviour, IResetable
{
    public UMLTree Tree;
    private Vector3 startPosition;
    public LayerMask DangerLayer;
    public LayerMask GarbageLayer;

    public Battery Battery;
    public PlayerController PlayerController;

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
                case EUMLActorState.Stopping:
                case EUMLActorState.Stopped:
                case EUMLActorState.Crashed:
                case EUMLActorState.Done:
                default:
                    return false;
            }
        }
    }
    public bool UMLFinished
    {
        get
        {
            return (int)State >= 20;
        }
    }

    private void Awake()
    {
        GetComponents<ILooseCondition>().ToList().ForEach(c => c.OnLoose = Crash);
        Battery = GetComponent<Battery>();
        PlayerController = GetComponent<PlayerController>();
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
            case EUMLActorState.Stopping:
                Debug.Log(name + " is stopping");
                SetActorState(EUMLActorState.Stopped);
                break;
            case EUMLActorState.Crashed:
                Debug.Log(name + " has crashed");
                break;
            case EUMLActorState.Stopped:
            case EUMLActorState.Ready:
            case EUMLActorState.Done:
            default:
                Debug.Log(name + " was not running");
                break;
        }
    }

    public void Crash()
    {
        SetActorState(EUMLActorState.Crashed);
    }

    public void Stop()
    {
        if (UMLRunning)
        {
            SetActorState(EUMLActorState.Stopping);
        }
        PlayerController?.StopPushes();
    }

    public void Reset()
    {
        GameManager.Instance.Console.Log(State.ToString(), name, "Is resetting");
        transform.position = startPosition;
        SetActorState(EUMLActorState.Ready);
    }

    private void SetActorState(EUMLActorState newState)
    {
        switch (newState)
        {
            case EUMLActorState.Stopped:
                GameManager.Instance.Console.LogWarning(newState.ToString(), name, $"State changed from {State} to {newState}");
                break;
            case EUMLActorState.Crashed:
                GameManager.Instance.Console.LogError(newState.ToString(), name, $"State changed from {State} to {newState}");
                break;
            case EUMLActorState.Ready:
            case EUMLActorState.Running:
            case EUMLActorState.Stopping:
            case EUMLActorState.Done:
            default:
                GameManager.Instance.Console.Log(newState.ToString(), name, $"State changed from {State} to {newState}");
                break;
        }
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
        PlayerController?.Move(Vector3.forward);
    }
    public void MoveDown()
    {
        PlayerController?.Move(Vector3.back);
    }
    public void MoveLeft()
    {
        PlayerController?.Move(Vector3.left);
    }
    public void MoveRight()
    {
        PlayerController?.Move(Vector3.right);
    }
    #endregion

    public void CollectGarbage()
    {
        GetComponent<GarbageCollector>()?.CollectGarbage();
    }

    public void CollectBattery()
    {
        Battery?.CollectBattery();
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
        if (PlayerController == null)
        {
            return false;
        }
        Vector3 pos = PlayerController.GetNextMovePointPosition(direction) + Vector3.down;
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
        if (PlayerController == null)
        {
            return false;
        }
        Vector3 pos = PlayerController.GetNextMovePointPosition(direction) + Vector3.down;
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
