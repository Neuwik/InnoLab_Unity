using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AUMLElement : MonoBehaviour, IResetable
{
    public virtual string Name { get { return $"Element ({name})"; } }

    [SerializeField]
    private AUMLElement _nextElement;
    public AUMLElement NextElement { get { return _nextElement; } protected set { _nextElement = value; } }

    [SerializeField]
    private Color highlightColor = Color.red;
    private Color baseColor;
    protected int highlightCounter = 0;

    [SerializeField]
    private Image _image;
    public Image Image { get { return _image; } }

    public int EnergyNeeded = 0;

    //private TickManager TickManager;

    [SerializeField]
    protected TMP_Dropdown dropDown;
    protected bool _dropDownIsSeeded = false;

    protected void Awake()
    {
        dropDown?.onValueChanged.AddListener(SelectedValueChanged);
        SeedDropDownOptions();
    }

    protected void Start()
    {
        baseColor = Image.color;
    }

    public virtual void Reset()
    {
        if (dropDown != null && !dropDown.interactable)
        {
            dropDown.interactable = true;
        }
    }

    public virtual bool ChangeNextElement(AUMLElement NewNextAction, bool condition = true)
    {
        NextElement = NewNextAction;
        return true;
    }

    public virtual AUMLElement GetNextElement(bool condition = true)
    {
        return condition ? NextElement : null;
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

        if (dropDown != null && dropDown.interactable)
        {
            dropDown.interactable = false;
        }

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

    protected virtual void SelectedValueChanged(int index)
    {
        //Debug.LogWarning("AUMLElement: SelectedValueChanged - " + name);
    }

    protected virtual void SeedDropDownOptions()
    {
        /*
        if (_dropDownIsSeeded)
        {
            return;
        }
        _dropDownIsSeeded = true;
        Debug.LogWarning("AUMLElement: UpdateDropDownOptions - " + name);
        */
    }

    public virtual long GetElementLongValue()
    {
        //Debug.LogWarning("AUMLElement: GetDropdownLongValue - " + name);
        return 0;
    }

    protected virtual bool SetElementLongValue(long value)
    {
        //Debug.LogWarning("AUMLElement: SetDropdownByLongValue - " + name);
        return true;
    }

    public virtual bool ApplySimpleData(UMLElementData data)
    {
        SeedDropDownOptions();
        Debug.Log("UMLElement ApplySimpleData: Value -> " + data.value);
        if (!SetElementLongValue(data.value))
        {
            return false;
        }

        Vector3 pos = new Vector3(data.position[0], data.position[1], data.position[2]);
        transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
        return true;
    }

    public virtual bool ApplyConnectionData(UMLElementData data, Dictionary<UMLElementData, AUMLElement> elements)
    {
        CreateArrow createArrow = GetComponent<CreateArrow>();

        createArrow.CanDraw = true;

        AUMLElement tNextElement = elements.FirstOrDefault(e => e.Key.ID == data.nextTID).Value;

        if (tNextElement != null)
        {
            if (!createArrow.DrawArrowToElement(tNextElement.GetComponent<CreateArrow>(), true))
            {
                return false;
            }
        }

        AUMLElement fNextElement = elements.FirstOrDefault(e => e.Key.ID == data.nextFID).Value;
        if (fNextElement != null)
        {
            if (!createArrow.DrawArrowToElement(fNextElement.GetComponent<CreateArrow>(), false))
            {
                return false;
            }
        }

        return true;
    }
}
