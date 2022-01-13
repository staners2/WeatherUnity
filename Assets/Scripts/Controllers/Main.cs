using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{
    public Text loginText;
    public InputField loginInputField;
    
    public InputField passwordInputField;

    public Dropdown languagesDropdown;

    void Start()
    {
        Debug.Log("START");
        languagesDropdown.ClearOptions();
        StartCoroutine(RequestController.getLanguages(languagesDropdown));
    }

    public void enterAccount(string sceneName)
    {
        Debug.Log("LOGIN START");
        if (!validationData())
            return;
        
        String login = loginText.text;
        String password = passwordInputField.text;
        Int32 languageId = Storage.languages
            .Find(x => x.title == languagesDropdown.options[languagesDropdown.value].text).id;
        
        StartCoroutine(corutineLogin(sceneName, login, password, languageId));
    }

    public void registrationAccount(string sceneName)
    {
        if (!validationData())
            return;
        
        String login = loginText.text;
        String password = passwordInputField.text;
        Int32 languageId = Storage.languages
            .Find(x => x.title == languagesDropdown.options[languagesDropdown.value].text).id;

        StartCoroutine(corutineRegistration(sceneName, login, password, languageId));
    }
    
    public Boolean validationData() {
        if (String.IsNullOrEmpty(loginInputField.text))
            return false;

        if (String.IsNullOrEmpty(passwordInputField.text))
            return false;

        return true;
    }
    
    public IEnumerator corutineLogin(string sceneName, String login, String password, Int32 languageId)
    {
        yield return RequestController.login(login, password, languageId);
        Debug.Log($"CORUTINE END");
        
        UserProfile userProfile = Storage.userProfile;
        Debug.Log($"PROFILE IS NULL = {userProfile == null}");
        if (userProfile != null)
            SceneManager.LoadScene(sceneName);
    }

    public IEnumerator corutineRegistration(string sceneName, String login, String password, Int32 languageId)
    {
        yield return RequestController.registration(login, password, languageId);
        Debug.Log($"CORUTINE END");
        
        UserProfile userProfile = Storage.userProfile;
        Debug.Log($"PROFILE IS NULL = {userProfile == null}");
        if (userProfile != null)
            SceneManager.LoadScene(sceneName);
    }
}
