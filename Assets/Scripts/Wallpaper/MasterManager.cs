using UnityEngine;
using Phezu.Util;

namespace Wallpaper {

    public class MasterManager : Singleton<MasterManager> {
        [SerializeField] [RequireInterface(typeof(INetworkManager))]
        private Object m_NetworkManager;

        [SerializeField] private UIHandler m_UIHandler;

        private INetworkManager NetworkManager => (INetworkManager)m_NetworkManager;

        public void SignUp() {
            m_UIHandler.OnSubmit();
            //if the user input was valid we sign user up.
            if (m_UIHandler.IsFormValid)
                SignUpUser();
        }

        public void SignIn() {
            m_UIHandler.OnSubmit();
            //if the user input was valid we sign user in.
            if (m_UIHandler.IsFormValid)
                SignInUser();
        }

        public void GoogleSignIn() {
            NetworkManager.SignInViaGoogle();
            //Load the home page
        }

        public void FacebookSignIn() {
            NetworkManager.SignInViaFacebook();
            //Load the home page
        }

        private void SignUpUser() {
            var email = m_UIHandler.GetUserEmail();
            var pass = m_UIHandler.GetUserPass();

            NetworkManager.SignUpViaEmail(email, pass);
            //Load the home page
        }

        private void SignInUser() {
            var email = m_UIHandler.GetUserEmail();
            var pass = m_UIHandler.GetUserPass();

            NetworkManager.SignInViaEmail(email, pass);
            //Load the home page
        }
    }
}