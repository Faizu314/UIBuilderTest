using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Phezu.Util;

public class FirebaseManager : Singleton<FirebaseManager> {
    private FirebaseApp m_App;
    private FirebaseAuth m_Auth;
    private FirebaseUser m_User;

    public bool IsReady { get; private set; }

    private void Awake() {
        InitializeFirebase();
    }

    private void InitializeFirebase() {
        IsReady = false;

        Debug.Log("Initializing App");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            try {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available) {
                    m_App = FirebaseApp.DefaultInstance;

                    InitializeFirebaseAuth();

                    IsReady = true;
                    Debug.Log("Firebase is ready");
                }
                else {
                    Debug.LogError(string.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            }
            catch (Exception e) {
                Debug.Log("Initialization threw an exception: " + e);
            }
        });
    }

    private void InitializeFirebaseAuth() {
        m_Auth = FirebaseAuth.DefaultInstance;
        m_Auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs) {
        if (m_Auth.CurrentUser == m_User)
            return;

        bool signedIn = m_Auth.CurrentUser != null;

        if (!signedIn && m_User != null) {
            Debug.Log("Signed out " + m_User.UserId);
        }
        m_User = m_Auth.CurrentUser;
        if (signedIn) {
            Debug.Log("Signed in " + m_User.UserId);
        }
    }

    public void SignUpToFirebaseViaEmail(string email, string password) {
        if (!IsReady) {
            Debug.Log("Accessing FirebaseManager when firebase was not ready");
            return;
        }

        m_Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            m_User = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                m_User.DisplayName, m_User.UserId);
        });
    }
    public void SignInToFirebaseViaEmail(string email, string password) {
        if (!IsReady) {
            Debug.Log("Accessing FirebaseManager when firebase was not ready");
            return;
        }

        m_Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            m_User = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                m_User.DisplayName, m_User.UserId);
        });
    }

    public void SignInToFirebaseViaGoogle(string googleIdToken) {
        if (!IsReady) {
            Debug.Log("Accessing FirebaseManager when firebase was not ready");
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);

        m_Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.Log("Firebase sign in with google credential canceled");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("Firebase sign in with google credential faulted: " + task.Exception);
                return;
            }

            m_User = m_Auth.CurrentUser;
            Debug.LogFormat("User signed in successfully via google: {0} ({1})",
                m_User.DisplayName, m_User.UserId);
        });
    }
    public void SignInToFirebaseViaFacebook(string facebookIdToken) {
        if (!IsReady) {
            Debug.Log("Accessing FirebaseManager when firebase was not ready");
            return;
        }

        Credential credential = FacebookAuthProvider.GetCredential(facebookIdToken);

        m_Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.Log("Firebase sign in with facebook credential canceled");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("Firebase sign in with facebook credential faulted: " + task.Exception);
                return;
            }

            m_User = m_Auth.CurrentUser;
            Debug.LogFormat("User signed in successfully via facebook: {0} ({1})",
                m_User.DisplayName, m_User.UserId);
        });
    }

    public void SignOutUser() {
        m_Auth.SignOut();
    }
}
