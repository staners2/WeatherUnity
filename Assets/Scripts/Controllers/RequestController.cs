using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public static class RequestController
{
    public static String SCHEMA = "http";
    public static String URL = "127.0.0.1"; //10.0.2.2
    public static String BASE_PATH = "/api/";
    public static Int32 PORT = 8000;
    public static String URI = $"{SCHEMA}://{URL}:{PORT}{BASE_PATH}";

    public static String API_REGISTRATION = "auth/registration";
    public static String API_AUTHORIZE = "auth/login";
    public static String API_LANGUAGES = "languages";
    public static String API_CITIES = "cities";
    public static String API_HISTORIES = "userprofile/{0}/histories"; // 0 - profile_id,
    public static String API_DELETE_HISTORIES = "userprofile/{0}/histories/{1}"; // 0 - profile_id, 1 - history_id
    public static String API_GET_WEATHER = "userprofile/weather/{0}"; // 0 - city_name

    public static IEnumerator getLanguages(Dropdown languagesDropdown)
      {
          Storage.isOperation = true;
          
          UnityWebRequest request = UnityWebRequest.Get(URI+API_LANGUAGES);
          yield return request.SendWebRequest();

          Debug.Log(request.downloadHandler.text);

          String json = "{\"languages\":" + request.downloadHandler.text + "}";


          Debug.Log(json);
          Languages languages = JsonUtility.FromJson<Languages>(json);
          
          Storage.languages = languages.languages.ToList();

          List<Language> items = new List<Language>();
          foreach (var item in Storage.languages)
          {
              items.Add(new Language(item.id, item.title, item.prefix));
          }
          List<String> list = Storage.languages.Select(x => x.title).ToList();
          languagesDropdown.AddOptions(list);
          
          Storage.isOperation = false;
      }

    public static IEnumerator login(String login, String password, Int32 language_id)
    {
        Storage.isOperation = true;
        
        Debug.Log("LOGIN REQUEST START");
        var body = new WWWForm();
        body.AddField("login", login);    
        body.AddField("password", password);  
        body.AddField("language_id", language_id.ToString());
        
        Debug.Log($"BODY = {body}");
        UnityWebRequest request = UnityWebRequest.Post(URI+API_AUTHORIZE, body);
        yield return request.SendWebRequest();
        Debug.Log("LOGIN REQUEST END");
        Debug.Log($"TEXT = {request.downloadHandler.text}");
        if (!request.downloadHandler.text.Contains("errors"))
        {
            UserProfile userProfile = JsonUtility.FromJson<UserProfile>(request.downloadHandler.text);
            Storage.userProfile = userProfile;
        }
        else
        {
            Storage.userProfile = null;
        }
        Storage.isOperation = false;
    }

    public static IEnumerator registration(String login, String password, Int32 language_id)
    {
        Storage.isOperation = true;
        
        Debug.Log("REGISTRATION REQUEST START");
        var body = new WWWForm();
        body.AddField("login", login);    
        body.AddField("password", password);  
        body.AddField("language_id", language_id.ToString());
        
        Debug.Log($"BODY = {body}");
        UnityWebRequest request = UnityWebRequest.Post(URI+API_REGISTRATION, body);
        yield return request.SendWebRequest();
        
        Debug.Log("REGISTRATION REQUEST END");
        Debug.Log($"TEXT = {request.downloadHandler.text}");
        if (!request.downloadHandler.text.Contains("errors"))
        {
            UserProfile userProfile = JsonUtility.FromJson<UserProfile>(request.downloadHandler.text);
            Storage.userProfile = userProfile;
        }
        else
        {
            Storage.userProfile = null;
        }
        Storage.isOperation = false;
    }

    public static IEnumerator getCities(Dropdown citiesDropdown)
    {
        Storage.isOperation = true;
        
        Debug.Log("CITIES REQUEST START");
        UnityWebRequest request = UnityWebRequest.Get(URI+API_CITIES);
        yield return request.SendWebRequest();
        
        Debug.Log("CITIES REQUEST END");
        Debug.Log($"TEXT = {request.downloadHandler.text}");
        
        String json = "{\"cities\":" + request.downloadHandler.text + "}";
        
        if (!request.downloadHandler.text.Contains("errors"))
        {
            Cities cities = JsonUtility.FromJson<Cities>(json);
            Storage.cities = cities.cities.ToList();
            List<String> list = Storage.cities.Select(x => x.title).ToList();
            citiesDropdown.AddOptions(list);
        }

        Storage.isOperation = false;
    }

    public static IEnumerator getWeather(int userProfileId, String cityName, Text tempField, Text appTempField, Text windSpeedField, Text cityField, Text dateField, Text descriptionField)
    {
        Storage.isOperation = true;
        
        var body = new WWWForm();
        body.AddField("userprofile_id", userProfileId);

        String urlApiFact = String.Format(API_GET_WEATHER, cityName);
        Debug.Log($"URI: {URI+urlApiFact}");
        
        Debug.Log($"BODY = {body}");
        
        Debug.Log("GET WEATHER RANDOM REQUEST START");
        UnityWebRequest request = UnityWebRequest.Post(URI+urlApiFact, body);
        yield return request.SendWebRequest();
        
        Debug.Log("GET WEATHER RANDOM REQUEST END");
        Debug.Log($"TEXT = {request.downloadHandler.text}");
        if (!request.downloadHandler.text.Contains("errors"))
        {
            ResponseGetWeather responseWeather = JsonUtility.FromJson<ResponseGetWeather>(request.downloadHandler.text);
            descriptionField.text = responseWeather.weather.description;
            tempField.text = "Температура: " + responseWeather.weather.temp;
            appTempField.text = "Ощущается: " + responseWeather.weather.app_temp;
            windSpeedField.text = "Ветер: " + responseWeather.weather.wind_speed;
            dateField.text = responseWeather.weather.date;
            cityField.text = "Город: " + responseWeather.weather.city.title;
        }
        
        Storage.isOperation = false;
    }

    public static IEnumerator getHistory(HistoriesController controller, int userProfileId, GameObject _object, Transform parent)
    {        

        Storage.isOperation = true;

        String urlHistory = String.Format(API_HISTORIES, userProfileId);
        Debug.Log(urlHistory);
        UnityWebRequest request = UnityWebRequest.Get(URI + urlHistory);
        yield return request.SendWebRequest();
        
        Debug.Log($"TEXT = {request.downloadHandler.text}");
        String json = "{\"histories\":" + request.downloadHandler.text + "}";        
        if (!request.downloadHandler.text.Contains("errors"))
        {
            Histories histories = JsonUtility.FromJson<Histories>(json);
            var list = histories.histories.ToList();
            foreach (var item in list)
            {
                item.weather.date = item.weather.date.Replace("T", " ");
                item.weather.date = item.weather.date.Replace("Z", "");
            }
            Storage.histories = list;
            controller.loadHistories();
        }

        Storage.isOperation = false;
    }

    public static IEnumerator deleteHistory(int historyId)
    {
        Storage.isOperation = true;

        String urlHistory = String.Format(API_DELETE_HISTORIES, Storage.userProfile.id, historyId);
        Debug.Log(urlHistory);
        UnityWebRequest request = UnityWebRequest.Delete(URI + urlHistory);
        yield return request.SendWebRequest();

        Storage.isOperation = false;
    }
}
