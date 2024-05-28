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

    protected void Start()
    {
        image = GetComponentInChildren<Image>();
        baseColor = image.color;
    }

    public virtual bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        NextElement = NewNextAction;
        return true;
    }

    public IEnumerator Run(UMLActor actor)
    {
        if (!actor.UMLRunning)
        {
            yield break;
        }

        Highlight();

        actor.Battery?.LooseEnergy(EnergyNeeded);

        if (!Execute(actor))
        {
            StopHighlight();
            actor.Crash();
            yield break;
        }

        yield return actor.WaitForTick();

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
