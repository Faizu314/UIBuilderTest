namespace Wallpaper {

    public interface INetworkManager {
        void SignUpViaEmail(string email, string password);
        void SignInViaEmail(string email, string password);
        void SignInViaGoogle();
        void SignInViaFacebook();
    }
}
