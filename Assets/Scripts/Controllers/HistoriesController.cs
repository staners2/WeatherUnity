using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoriesController : MonoBehaviour
{
    public GameObject _object;
    public Transform parent;
    public List<GameObject> listObjectHistory = new List<GameObject>();

    void Start()
    {
        Debug.Log("VOID START");
        StartCoroutine(RequestController.getHistory(this, Storage.userProfile.id, _object, parent));
    }

    public void closeHistoryScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void loadHistories()
    {
        float k = -.0f;
        int i = 0;
        Debug.Log(Storage.histories.Count);
        listObjectHistory.Add(Instantiate(_object, parent));
        //listObjectHistory[0].transform;
        create(listObjectHistory[0], Storage.histories[0]);
        listObjectHistory[0].transform.Translate(.0f, k, .0f);
        for (i = 1; i < Storage.histories.Count; i++)
        {
            listObjectHistory.Add(Instantiate(_object, parent));
            create(listObjectHistory[i], Storage.histories[i]);
            k = k - 1.920f;
            listObjectHistory[i].transform.Translate(.0f, k, .0f);
        }

    }

    public void create(GameObject parent, History history)
    {
        parent.transform.GetChild(0).GetComponent<Text>().text = history.weather.date;
        parent.transform.GetChild(1).GetComponent<Text>().text += history.weather.city.title;
        parent.transform.GetChild(2).GetComponent<Text>().text += history.weather.temp;
        parent.transform.GetChild(3).GetComponent<Text>().text += history.weather.app_temp;
        parent.transform.GetChild(4).GetComponent<Text>().text += history.weather.wind_speed;
        parent.transform.GetChild(5).GetComponent<Text>().text = history.weather.description;
        parent.transform.GetChild(6).GetComponent<Button>().onClick.AddListener((() => deleteHistory(history.id, parent)));
    }

    public void deleteHistory(int id, GameObject parent)
    {
        Destroy(parent);
        StartCoroutine(RequestController.deleteHistory(id));
    }
}
