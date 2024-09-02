using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabLogin : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private TMP_InputField loginUsername;
    [SerializeField] private TMP_InputField loginPassword;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Button playButtton;
    [SerializeField] private Button startButtton;

    [Header("Register UI")]
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField verifyPassword;
    [SerializeField] private TMP_InputField email;

    [Header("Reset Password UI")]
    [SerializeField] private TMP_InputField emailInput;

    [Header("Extras")]
    [SerializeField] private GameObject loadInProgress;
    [SerializeField] private TMP_Text vallidationMessage;
    [SerializeField] private List<Button> allButtons = new List<Button>();

    private LevelManager levelManager;
    public void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        loginUsername.text = PlayerPrefs.GetString(PlayFabConstants.SavedUsername, "");
        CheckPlayerAlreadyLoggedIn();
    }
    private void CheckPlayerAlreadyLoggedIn()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            ValidationMessage("User is already logged in", Color.green);
            loginPanel.SetActive(false);
            playButtton.gameObject.SetActive(true);
            startButtton.gameObject.SetActive(false);
        }
        else
        {
            playButtton.gameObject.SetActive(false);
            startButtton.gameObject.SetActive(true);
        }
    }
    public void OnPlayButtonClicked()
    {
        StartCoroutine(LoadGameScene());
    }
    private void Login(ILogin loginMethod, object loginParams)
    {
        loginMethod.Login(_loginInfoParams, OnLoginSuccess, OnLoginFailure, loginParams);
        loadInProgress.SetActive(true);
    }

    #region Email Login

    public void LoginWithEmail()
    {
        if (ValidateLoginData())
        {
            Login(new EmailLogin(), new EmailLogin.EmailLoginParams(loginUsername.text, loginPassword.text));
        }
    }

    private bool ValidateLoginData()
    {
        //Validating data
        string errorMessage = "";

        if (loginUsername.text.Length < 5)
        {
            errorMessage = "Username must be at least 5 characters";
        }
        else if (loginPassword.text.Length < 8)
        {
            errorMessage = "Password must be at least 8 characters";
        }

        if (errorMessage.Length > 0)
        {
            ValidationMessage(errorMessage, Color.red);
            return false;
        }

        return true;
    }

    #endregion

    #region Email Register

    public void Register()
    {
        if (!ValidateRegisterData()) return;

        var request = new RegisterPlayFabUserRequest
        {
            TitleId = PlayFabConstants.TitleID,
            Email = email.text,
            Password = password.text,
            Username = username.text,
            InfoRequestParameters = _loginInfoParams,
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnLoginFailure);
        loadInProgress.SetActive(true);
    }

    bool ValidateRegisterData()
    {
        string errorMessage = "";

        if (!email.text.Contains("@"))
        {
            errorMessage = "E-mail is not valid";
        }
        else if (email.text.Length < 5)
        {
            errorMessage = "E-mail is not valid";
        }
        else if (username.text.Length < 5)
        {
            errorMessage = "Username must be at least 5 characters";
        }
        else if (password.text.Length < 8)
        {
            errorMessage = "Password must be at least 8 characters";
        }
        else if (password.text != verifyPassword.text)
        {
            errorMessage = "Password doesn't match Repeat password";
        }

        if (errorMessage.Length > 0)
        {
            ValidationMessage(errorMessage, Color.red);
            return false;
        }

        return true;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        ValidationMessage("Register Success!", Color.green);
        PlayerPrefs.SetString("USERNAME", username.text);
        PlayerPrefs.SetString("PW", password.text);
        loadInProgress.SetActive(false);
        loginPanel.SetActive(true);
    }

    #endregion


    private void OnLoginSuccess(LoginResult result)
    {
        ValidationMessage("Login Success!", Color.green);
        PlayerPrefs.SetString(PlayFabConstants.SavedUsername, username.text);
       // levelManager.GetUserData();
        loadInProgress.SetActive(false);
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (name == null)
        {
            namePanel.SetActive(true);
            loginPanel.SetActive(false);
        }
        else
        {
            loginPanel.SetActive(false);
            playButtton.gameObject.SetActive(true);
            startButtton.gameObject.SetActive(false);
        }

    }
    #region Reset Password
    public void OnClickResetPassword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = PlayFabConstants.TitleID
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
        loadInProgress?.SetActive(true);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        loadInProgress?.SetActive(false);
        ValidationMessage("Password reset mail sent!",Color.green);
    }
    #endregion
    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        loadInProgress.SetActive(true);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        loadInProgress.SetActive(false);
        ValidationMessage("User name set!", Color.green);
        StartCoroutine(LoadGameScene());
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error);
    }

    private readonly GetPlayerCombinedInfoRequestParams _loginInfoParams =
        new GetPlayerCombinedInfoRequestParams
        {
            GetUserAccountInfo = true,
            GetUserData = true,
            GetUserInventory = true,
            GetUserVirtualCurrency = true,
            GetUserReadOnlyData = true,
            GetPlayerProfile = true,
        };

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login failure: " + error.Error + "  " + error.ErrorDetails + error + "  " +
                       error.ApiEndpoint + "  " + error.ErrorMessage);
        loadInProgress.SetActive(false);
        ValidationMessage(error.ErrorMessage, Color.red);
    }
    private void ValidationMessage(string errorMessage, Color color)
    {
        vallidationMessage.text = errorMessage;
        vallidationMessage.color = color;
        vallidationMessage.gameObject.SetActive(true);
    }
    private IEnumerator LoadGameScene()
    {
        SetAllButtonsInteractables(false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
        SetAllButtonsInteractables(true);
    }
    private void SetAllButtonsInteractables(bool status)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].interactable = status;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
