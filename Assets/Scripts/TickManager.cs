using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TickManager : MonoBehaviour, IResetable
{
    public float TickRate = 1; //Actions per second
    private List<UMLActor> Actors;
    private List<Enemy_Movement> Enemies;

    private bool PlayerTurn = false;
    private IEnumerator ManageTicksEnumerator;

    private void Start()
    {
        Actors = GameManager.Instance.UMLActors;
        Enemies = GameManager.Instance.Enemies;
    }

    public void Reset()
    {
        StopCoroutine(ManageTicksEnumerator);
        PlayerTurn = false;
    }

    public void StartTicks()
    {
        PlayerTurn = true;
        StartCoroutine(ManageTicksEnumerator = ManageTicks());
    }

    private IEnumerator ManageTicks()
    { 
        while (GameManager.Instance.UMLIsRunning)
        {
            //Debug.LogWarning("PLAYER TURN");
            PlayerTurn = true;
            yield return WaitForTick();
            yield return WaitForPlayerMovement();

            //Debug.LogWarning("ENEMY TURN");
            PlayerTurn = false;
            yield return WaitForTick();
            yield return WaitForEnemyMovement();
        }
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
        foreach (Enemy_Movement enemy in Enemies)
        {
            yield return enemy.WaitForMovementFinished();
        }
    }

    public IEnumerator WaitForPlayerTickStart()
    {
        yield return new WaitUntil(() => PlayerTurn);
    }

    public IEnumerator WaitForPlayerTickEnd()
    {
        yield return new WaitUntil(() => !PlayerTurn);
    }
}
