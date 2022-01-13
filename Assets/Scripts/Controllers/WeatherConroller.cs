using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class WeatherConroller : MonoBehaviour
{
    public Dropdown citiesDropdown;
    public Text descriptionField;
    public Text tempField;
    public Text appTempField;
    public Text windSpeedField;
    public Text dateField;
    public Text cityField;


    void Start()
    {
        Debug.Log("START");
        citiesDropdown.ClearOptions();
        StartCoroutine(RequestController.getCities(citiesDropdown));
    }

    public void getWeather()
    {
        String cityName = Storage.cities.Find(x => x.title == citiesDropdown.options[citiesDropdown.value].text).title;
        
        StartCoroutine(RequestController.getWeather(Storage.userProfile.id, cityName, tempField, appTempField, windSpeedField, cityField, dateField, descriptionField));
    }

    public void openHistoryScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quitAccount(String sceneName)
    {
        Storage.cities = null;
        Storage.histories = null;
        Storage.languages = null;
        Storage.userProfile = null;
        
        SceneManager.LoadScene(sceneName);
    }
}
