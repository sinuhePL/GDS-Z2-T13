using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEffect
{

    private string _description;

    public AbstractEffect(string myDescription)
    {
        _description = myDescription;
    }

    public string GetDescription()
    {
        return _description;
    }
}
