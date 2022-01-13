using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseGetWeather
{
    public Int32 id;
    public UserProfile user;
    public Weather weather;
}
