using System.Threading.Tasks;
using Firebase.Extensions;
using Google;
using System;

namespace Wallpaper.Network {

    public class GoogleManager {

        private const string GOOGLE_CLIENT_ID = "546643239907-gsat7uqqah78ng3egk0pukjc09c6vbrt.apps.googleusercontent.com";

        private GoogleSignInConfiguration m_GoogleConfig;
        private Action<string> m_OnTokenReceived;
        private Action<string> m_OnAuthError;

        public GoogleManager() {
            m_GoogleConfig = new GoogleSignInConfiguration {
                WebClientId = GOOGLE_CLIENT_ID,
                RequestIdToken = true
            };
        }

        //We save 2 callbacks because we will need to notify the caller that google has signed in.
        public void RequestGoogleToken(Action<string> onTokenReceived, Action<string> onAuthError) {
            GoogleSignIn.Configuration = m_GoogleConfig;
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            GoogleSignIn.Configuration.RequestEmail = true;

            m_OnTokenReceived = onTokenReceived;
            m_OnAuthError = onAuthError;
            GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnGoogleSignIn);
        }

        //This function is called automatically by google sdk on sign in. At this point we can use the callbacks
        //to notify the caller that google has signed in.
        private void OnGoogleSignIn(Task<GoogleSignInUser> task) {
            if (task.IsCanceled) {
                m_OnAuthError.Invoke("Google sign in canceled");
                return;
            }
            if (task.IsFaulted) {
                m_OnAuthError.Invoke(task.Exception.ToString());
                return;
            }

            m_OnTokenReceived.Invoke(task.Result.IdToken);
        }
    }
}