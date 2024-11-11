using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleAnimation : MonoBehaviour
{
    private Animator consoleAnimator;
    private bool isVisible = false;

    void Start()
    {
        consoleAnimator = GetComponent<Animator>();
    }

    public void ToggleConsole()
    {
        isVisible = !isVisible;
        consoleAnimator.SetBool("isVisible", isVisible);
    }
}
