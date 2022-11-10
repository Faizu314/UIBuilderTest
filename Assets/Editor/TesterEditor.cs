using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tester))]
public class TesterEditor : Editor
{
    private Tester m_Tester;
    private Tester Target {
        get {
            if (m_Tester == null)
                m_Tester = (Tester)target;

            return m_Tester;
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Sign Up Via Email")) {
            Target.SignUpViaEmail();
        }
        else if (GUILayout.Button("Sign In Via Email")) {
            Target.SignInViaEmail();
        }
        else if (GUILayout.Button("Sign In Via Google")) {
            Target.SignInViaGoogle();
        }
        else if (GUILayout.Button("Sign Out")) {
            Target.SignOut();
        }
    }
}
