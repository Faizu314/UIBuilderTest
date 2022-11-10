using System.Threading.Tasks;
using UnityEngine;
using Firebase.Extensions;
using Google;
using Phezu.Util;


public class GoogleManager : Singleton<GoogleManager> {

    private const string GOOGLE_CLIENT_ID = "546643239907-gsat7uqqah78ng3egk0pukjc09c6vbrt.apps.googleusercontent.com";

    private GoogleSignInConfiguration googleConfig;

    private void Awake() {
        googleConfig = new GoogleSignInConfiguration {
            WebClientId = GOOGLE_CLIENT_ID,
            RequestIdToken = true
        };
    }

    public void GoogleSignInClick() {
        GoogleSignIn.Configuration = googleConfig;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnGoogleSignIn);
    }

    private void OnGoogleSignIn(Task<GoogleSignInUser> task) {
        if (task.IsCanceled) {
            Debug.Log("Google sign in canceled");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("Google sign in faulted: " + task.Exception);
            return;
        }

        Debug.Log("Google ID Token received");

        FirebaseManager.Instance.SignInToFirebaseViaGoogle(task.Result.IdToken);
    }
}
