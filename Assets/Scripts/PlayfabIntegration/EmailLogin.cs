using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailLogin : ILogin
{
    public class EmailLoginParams
    {
        public string Username;
        public string Password;
        public EmailLoginParams(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
    public void Login(GetPlayerCombinedInfoRequestParams loginInfoParams, Action<LoginResult> loginSuccess, Action<PlayFabError> loginFailure, object loginParams)
    {
        if (loginParams is not EmailLoginParams emailLoginParams)
        {
            loginFailure.Invoke(obj: new PlayFabError());
            Debug.LogError(message: "Login Parameter is null");
            return;
        }
        var request = new LoginWithPlayFabRequest
        {
            TitleId = PlayFabConstants.TitleID,
            Username = emailLoginParams.Username,
            Password = emailLoginParams.Password,
            InfoRequestParameters = loginInfoParams,
        };
        PlayFabClientAPI.LoginWithPlayFab(request, loginSuccess, loginFailure);
    }
}
