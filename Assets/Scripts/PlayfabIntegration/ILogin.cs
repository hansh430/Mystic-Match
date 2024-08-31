using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILogin
{
    void Login(GetPlayerCombinedInfoRequestParams loginInfoParams, Action<LoginResult> loginSuccess, Action<PlayFabError> loginFailure, object loginParams);
}
