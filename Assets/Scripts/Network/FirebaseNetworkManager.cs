using UnityEngine;

namespace Wallpaper.Network {

    public class FirebaseNetworkManager : MonoBehaviour, INetworkManager {
        private FirebaseManager m_FirebaseManager;
        private GoogleManager m_GoogleManager;
        private FacebookManager m_FacebookManager;

        private void Start() {
            m_FirebaseManager = new();
            m_GoogleManager = new();
            m_FacebookManager = new();

            m_FirebaseManager.InitializeFirebase();
            m_FacebookManager.InitializeFacebook();
        }

        public void SignUpViaEmail(string email, string password) {
           m_FirebaseManager.SignUpToFirebaseViaEmail(email, password);
        }

        public void SignInViaEmail(string email, string password) {
            m_FirebaseManager.SignInToFirebaseViaEmail(email, password);
        }

        //For these two methods we will first use fb/google managers to get the token and on token received
        //we will call the firebase sign in method using that token.
        //The second parameter passed is the onError callback in which case we will log to the user.

        public void SignInViaFacebook() {
            m_FacebookManager.RequestFacebookToken((token) => {
                m_FirebaseManager.SignInToFirebaseViaFacebookToken(token);
            },
            (error) => {
                //throw exception
            });
        }

        public void SignInViaGoogle() {
            m_GoogleManager.RequestGoogleToken((token) => {
                m_FirebaseManager.SignInToFirebaseViaGoogleToken(token);
            },
            (error) => {
                //throw exception
            });
        }
    }
}