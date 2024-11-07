using UnityEngine;
using UnityEngine.UI;

public abstract class AUMLElement : MonoBehaviour
{
    public virtual string Name { get { return $"Element ({name})"; } }

    [SerializeField]
    private AUMLElement _nextElement;
    public AUMLElement NextElement { get { return _nextElement; } protected set { _nextElement = value; } }

    [SerializeField]
    private Color highlightColor = Color.red;
    private Color baseColor;
    protected int highlightCounter = 0;
    public Image Image { get; protected set; }

    public int EnergyNeeded = 0;

    //private TickManager TickManager;

    protected void Start()
    {
        Image = GetComponentInChildren<Image>();
        baseColor = Image.color;
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
        highlightCounter++;
        if (highlightCounter == 1)
        {
            Image.color = highlightColor;
        }
    }

    public void StopHighlight()
    {
        if (highlightCounter > 0)
        {
            highlightCounter--;
        }

        if (highlightCounter == 0)
        {
            Image.color = baseColor;
        }
    }
}
