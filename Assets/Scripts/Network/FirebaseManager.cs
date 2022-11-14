using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;

namespace Wallpaper.Network {

    public class FirebaseManager {

        private FirebaseApp m_App;
        private FirebaseAuth m_Auth;
        private FirebaseUser m_User;

        public bool IsReady { get; private set; }

        public FirebaseManager() {
            IsReady = false;
        }

        public void InitializeFirebase() {
            Debug.Log("Initializing App");
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                try {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == DependencyStatus.Available) {
                        m_App = FirebaseApp.DefaultInstance;

                        InitializeFirebaseAuth();
                        //Any other modules of firebase that we add will get initialized here.

                        //After initializing every module we set IsReady to true.
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
            //StateChanged event is fired everytime the user changes or gets logged out/in.
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

            m_Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(OnUserSignedIn);
        }
        public void SignInToFirebaseViaEmail(string email, string password) {
            if (!IsReady) {
                Debug.Log("Accessing FirebaseManager when firebase was not ready");
                return;
            }

            m_Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnUserSignedIn);
        }

        //As you can see it is exactly the same but instead of using email and password we use Google Token.
        public void SignInToFirebaseViaGoogleToken(string googleIdToken) {
            if (!IsReady) {
                Debug.Log("Accessing FirebaseManager when firebase was not ready");
                return;
            }

            Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);

            m_Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(OnUserSignedIn);
        }

        //As you can see it is exactly the same but instead of using email and password we use Facebook Token.
        public void SignInToFirebaseViaFacebookToken(string facebookIdToken) {
            if (!IsReady) {
                Debug.Log("Accessing FirebaseManager when firebase was not ready");
                return;
            }

            Credential credential = FacebookAuthProvider.GetCredential(facebookIdToken);

            m_Auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(OnUserSignedIn);
        }

        //The only point of adding this method is to reduce the lines on code. Instead of
        //duplicating this code in 4 places (which you can see through references) I now refer to this method.
        private void OnUserSignedIn(Task<FirebaseUser> task) {
            if (task.IsCanceled) {
                Debug.Log("Firebase sign in canceled");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("Firebase sign in faulted: " + task.Exception);
                return;
            }

            m_User = m_Auth.CurrentUser;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                m_User.DisplayName, m_User.UserId);
        }

        public void SignOutUser() {
            m_Auth.SignOut();
        }
    }
}