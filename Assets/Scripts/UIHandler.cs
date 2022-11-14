using UnityEngine;
using TMPro;

namespace Wallpaper {

    public class UIHandler : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI emailInput;
        [SerializeField] private TextMeshProUGUI passwordInput;

        [SerializeField] private TextMeshProUGUI outputLog;

        private string UserEmail => emailInput.text;
        private string UserPass => passwordInput.text;

        public bool IsFormValid { get; private set; }

        private void Start() {
            emailInput.text = string.Empty;
            passwordInput.text = string.Empty;

            //The only point of doing this is so that we can see what is logging on our mobile device.
            //For further logging we will have to connect to android logcat.
            Application.logMessageReceived += OnDebugLog;
        }

        //This method will check using an external class whether the user input is valid and set the IsFormValid property.
        //In case of an invalid input we will set a hint text that will log the user.
        public void OnSubmit() {
            IsFormValid = !string.IsNullOrWhiteSpace(UserEmail) && !string.IsNullOrWhiteSpace(UserPass);
        }

        public string GetUserEmail() {
            //check if valid before returning
            return UserEmail;
        }

        public string GetUserPass() {
            //check if valid before returning
            return UserPass;
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
}