using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weather
{
    public int id;
    public City city;
    public String temp;
    public String app_temp;
    public String wind_speed;
    public String description;
    public String date;

    public Weather(int id, City city, String temp, String app_temp, String wind_speed, String description, String date)
    {
        this.id = id;
        this.city = city;
        this.temp = temp;
        this.app_temp = app_temp;
        this.wind_speed = wind_speed;
        this.description = description;
        this.date = date;
    }
}
