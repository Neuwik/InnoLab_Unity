using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public enum ETickManagerStatus { None = 0, Started = 1, Stopped = 2, Player = 11, Enemy = 12 }

public class TickManager : MonoBehaviour, IResetable
{
    public float TickRate = 1; //Actions per second
    private List<UMLActor> Actors;
    private List<EnemyMovementController> Enemies;

    private ETickManagerStatus Status = ETickManagerStatus.None;

    private void Start()
    {
        Actors = GameManager.Instance.UMLActors;
        Enemies = GameManager.Instance.Enemies;
    }

    public void Reset()
    {
        Status = ETickManagerStatus.Stopped;
        //Debug.LogWarning("END TICK");
        StopAllCoroutines();
        Status = ETickManagerStatus.Stopped;
    }

    public void StartTicks()
    {
        //Debug.LogWarning("START TICK");
        Actors = GameManager.Instance.UMLActors;
        Enemies = GameManager.Instance.Enemies;
        //Debug.Log("TICK MANAGER ACTORS: " + Actors.Count);
        Status = ETickManagerStatus.Started;
        StartCoroutine(ManageTicks());
    }

    private IEnumerator ManageTicks()
    {
        Status = ETickManagerStatus.Started;
        //Debug.LogWarning("START TICK (WHILE)");

        //Delay to ensure everything started and is listening to the TickManager
        yield return WaitForTick();

        while (GameManager.Instance.UMLIsRunning)
        {
            //Debug.LogWarning("NEXT TICK");
            //Debug.LogWarning("PLAYER TURN");
            Status = ETickManagerStatus.Player;
            yield return WaitForTick();
            yield return WaitForPlayerMovement();

            // Should Probably not happen every Tick
            GameManager.Instance.UpdateLevelProgress();

            //Debug.LogWarning("ENEMY TURN");
            Status = ETickManagerStatus.Enemy;
            yield return WaitForTick();
            yield return WaitForEnemyMovement();

            // Should Probably not happen every Tick
            GameManager.Instance.UpdateLevelProgress();
        }

        //Debug.LogWarning("END TICK (WHILE)");
        Status = ETickManagerStatus.Stopped;
    }

    private IEnumerator WaitForTick()
    {
        if (TickRate <= 0)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(1 / TickRate);
        }
    }

    private IEnumerator WaitForPlayerMovement()
    {
        foreach (UMLActor actor in Actors.Where(a => a.UMLRunning))
        {
            yield return actor.PlayerController?.WaitForMovementFinished();
        }
    }

    private IEnumerator WaitForEnemyMovement()
    {
        foreach (EnemyMovementController enemy in Enemies)
        {
            yield return enemy.WaitForMovementFinished();
        }
    }

    public IEnumerator WaitForPlayerTickStart()
    {
        yield return new WaitUntil(() => Status == ETickManagerStatus.Player || Status == ETickManagerStatus.Stopped);
    }

    public IEnumerator WaitForPlayerTickEnd()
    {
        yield return new WaitUntil(() => Status != ETickManagerStatus.Player);
    }

    public IEnumerator WaitForEnemyTickStart()
    {
        yield return new WaitUntil(() => Status == ETickManagerStatus.Enemy || Status == ETickManagerStatus.Stopped);
    }

    public IEnumerator WaitForEnemyTickEnd()
    {
        yield return new WaitUntil(() => Status != ETickManagerStatus.Enemy);
    }
}
