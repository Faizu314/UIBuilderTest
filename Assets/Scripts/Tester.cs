using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private string email;
    [SerializeField] private string password;

    public void SignUpViaEmail() {
        FirebaseManager.Instance.SignUpToFirebaseViaEmail(email, password);
    }

    public void SignInViaEmail() {
        FirebaseManager.Instance.SignInToFirebaseViaEmail(email, password);
    }

    public void SignInViaGoogle() {
        GoogleManager.Instance.GoogleSignInClick();
    }

    public void SignOut() {
        FirebaseManager.Instance.SignOutUser();
    }
}
