using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Language
{
    public int id;
    public string title;
    public string prefix;
    
    public Language(int id, string title, string prefix)
    {
        this.id = id;
        this.title = title;
        this.prefix = prefix;
    }
}
