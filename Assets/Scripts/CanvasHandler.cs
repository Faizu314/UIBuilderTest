using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI emailInput;
    [SerializeField] private TextMeshProUGUI passwordInput;

    [SerializeField] private TextMeshProUGUI outputLog;

    [SerializeField] private Button signUpButton;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button googleButton;
    [SerializeField] private Button facebookButton;

    private string UserEmail => emailInput.text.Trim();
    private string UserPass => passwordInput.text.Trim();

    private void Start() {
        signUpButton.onClick.AddListener(() => {
            FirebaseManager.Instance.SignUpToFirebaseViaEmail(UserEmail, UserPass);
        });

        signInButton.onClick.AddListener(() => {
            FirebaseManager.Instance.SignInToFirebaseViaEmail(UserEmail, UserPass);
        });

        googleButton.onClick.AddListener(() => {
            GoogleManager.Instance.GoogleSignInClick();
        });

        facebookButton.onClick.AddListener(() => {
            FacebookManager.Instance.FacebookSignInClick();
        });

        Application.logMessageReceived += OnDebugLog;
    }

    private void OnDebugLog(string condition, string stackTrace, LogType type) {
        switch (type) {
            case LogType.Log:
                outputLog.SetText("Log: " + condition);
                break;
            case LogType.Error:
                outputLog.SetText("Error: " + condition);
                break;
            default:
                outputLog.SetText("Other: " + condition);
                break;
        }
    }
}
