using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILooseCondition
{
    public Action OnLoose { get; set; }
}
