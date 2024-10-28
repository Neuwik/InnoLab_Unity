using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUMLElement : MonoBehaviour
{
    public virtual string Name { get { return $"Element ({name})"; } }

    [SerializeField]
    private AUMLElement _nextElement;
    public AUMLElement NextElement { get { return _nextElement; } protected set { _nextElement = value; } }

    [SerializeField]
    private Color32 highlightColor = Color.red;
    private Color32 baseColor;
    private Image image;

    public int EnergyNeeded = 0;

    //private TickManager TickManager;

    protected void Start()
    {
        image = GetComponentInChildren<Image>();
        baseColor = image.color;
        //TickManager = GameManager.Instance.TickManager;
    }

    public virtual bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        NextElement = NewNextAction;
        return true;
    }

    /*
    public IEnumerator Run(UMLActor actor)
    {
        Highlight();

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
    */

    public virtual bool Execute(UMLActor actor)
    {
        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing {Name}");
        //Debug.Log("Some Element: " + name);
        return true;
    }

    public void Highlight()
    {
        image.color = highlightColor;
    }

    public void StopHighlight()
    {
        image.color = baseColor;
    }
}
