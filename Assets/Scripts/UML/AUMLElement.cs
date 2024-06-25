using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUMLElement : MonoBehaviour
{
    [SerializeField]
    protected AUMLElement NextElement;

    [SerializeField]
    private Color32 highlightColor = Color.red;
    private Color32 baseColor;
    private Image image;

    public int EnergyNeeded = 0;

    private TickManager TickManager;

    protected void Start()
    {
        image = GetComponentInChildren<Image>();
        baseColor = image.color;
        TickManager = GameManager.Instance.TickManager;
    }

    public virtual bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        NextElement = NewNextAction;
        return true;
    }

    public IEnumerator Run(UMLActor actor)
    {
        TickManager = GameManager.Instance.TickManager;

        yield return TickManager.WaitForPlayerTickStart();

        if (!GameManager.Instance.UMLIsRunning)
        {
            yield break;
        }

        if (!actor.UMLRunning)
        {
            yield break;
        }

        Highlight();

        if (EnergyNeeded > 0)
        {
            actor.Battery?.LooseEnergy(EnergyNeeded);
        }

        if (!Execute(actor))
        {
            StopHighlight();
            actor.Crash();
            yield break;
        }

        if (EnergyNeeded > 0)
        {
            yield return TickManager.WaitForPlayerTickEnd();
        }

        StopHighlight();

        if (NextElement is UMLTree)
        {
            yield break;
        }

        if (NextElement == null)
        {
            yield break;
        }

        yield return NextElement?.Run(actor);
    }

    protected virtual bool Execute(UMLActor actor)
    {
        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing {name}");
        //Debug.Log("Some Element: " + name);
        return true;
    }

    private void Highlight()
    {
        image.color = highlightColor;
    }

    private void StopHighlight()
    {
        image.color = baseColor;
    }
}
