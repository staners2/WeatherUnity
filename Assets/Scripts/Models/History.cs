using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class History
{
    public int id;
    public Weather weather;
    public UserProfile userProfile;

    public History(int id, UserProfile userProfile, Weather weather)
    {
        this.id = id;
        this.userProfile = userProfile;
        this.weather = weather;
    }
}
