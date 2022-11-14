using System;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

namespace Wallpaper.Network {

    public class FacebookManager {
        public bool IsReady { get; private set; }

        private Action<string> m_OnTokenReceived;
        private Action<string> m_OnAuthError;

        public FacebookManager() {
            IsReady = false;
        }

        public void InitializeFacebook() {
            //FB Init method takes two callbacks, one is called after initialization and the other
            //is called whenever the unity app is hidden meaning when the player presses home button.
            FB.Init(OnFacebookInitialized, OnHideUnity);
        }

        private void OnFacebookInitialized() {
            if (!FB.IsInitialized) {
                Debug.LogError("Failed to initialized facebook");
                return;
            }

            FB.ActivateApp();

            //On Successful initialization we set IsReady to true so that we can safely call other API methods.
            IsReady = true;
            Debug.Log("Facebook initialized");
        }

        private void OnHideUnity(bool isUnityShown) {
            if (isUnityShown)
                FB.ActivateApp();
        }

        //We save the onTokenReceive and onError callbacks because we will need them later.
        public void RequestFacebookToken(Action<string> onTokenReceived, Action<string> onAuthError) {
            if (!IsReady) {
                Debug.Log("Trying to log in through facebook when facebook manager was not ready");
                return;
            }

            m_OnTokenReceived = onTokenReceived;
            m_OnAuthError = onAuthError;
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, OnFacebookSignIn);
        }

        //This function is called by FB on sign in. At this point we can call the saved callbacks to let
        //the caller know that facebook has signed in.
        private void OnFacebookSignIn(ILoginResult result) {
            if (!FB.IsLoggedIn) {
                m_OnAuthError.Invoke(result.Error);
                return;
            }

            m_OnTokenReceived.Invoke(AccessToken.CurrentAccessToken.TokenString);
        }
    }
}