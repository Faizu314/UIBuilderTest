using System.Collections.Generic;
using UnityEngine;
using Phezu.Util;
using Facebook.Unity;

public class FacebookManager : Singleton<FacebookManager>
{
    public bool IsReady { get; private set; }

    private void Start() {
        IsReady = false;
        FB.Init(OnFacebookInitialized, OnHideUnity);
    }

    private void OnFacebookInitialized() {
        if (!FB.IsInitialized) {
            Debug.LogError("Failed to initialized facebook");
            return;
        }

        FB.ActivateApp();
        IsReady = true;
        Debug.Log("Facebook initialized");
    }

    private void OnHideUnity(bool isUnityShown) {
        if (isUnityShown)
            FB.ActivateApp();
    }

    public void FacebookSignInClick() {
        if (!IsReady) {
            Debug.Log("Trying to log in through facebook when facebook manager was not ready");
            return;
        }

        FB.LogInWithReadPermissions(new List<string>(){ "public_profile", "email", "user_friends" }, OnFacebookSignIn);
    }

    private void OnFacebookSignIn(ILoginResult result) {
        if (!FB.IsLoggedIn) {
            Debug.Log("Facebook sign in unsuccessful: " + result.Error);
            return;
        }

        Debug.Log("Facebook token received");
        FirebaseManager.Instance.SignInToFirebaseViaFacebook(AccessToken.CurrentAccessToken.TokenString);
    }
}
