using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class City
{
    public int id;
    public String title;
    public String country_code;

    public City(int id, String title, String country_code)
    {
        this.id = id;
        this.title = title;
        this.country_code = country_code;
    }
}
