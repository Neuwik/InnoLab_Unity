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

    private void Start()
    {
        image = GetComponent<Image>();
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
        //return false; // To test Highlight

        if (!Execute(actor))
        {
            StopHighlight();
            yield break;
        }

        yield return actor.WaitForTick();

        StopHighlight();
        //return false; // To test StopHighlight

        if (NextElement is UMLTree)
        {
            yield break;
        }
        yield return NextElement?.Run(actor);
    }

    protected virtual bool Execute(UMLActor actor)
    {
        Debug.Log("Some Element: " + name);
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
