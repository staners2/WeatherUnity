using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserProfile
{
    public Int32 id;
    public String login;
    public String password;
    public Language language;

    public UserProfile(Int32 id, String login, String password, Language language)
    {
        this.id = id;
        this.login = login;
        this.password = password;
        this.language = language;
    }
}
