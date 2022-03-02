using UnityEngine;
using UnityEditor;

public class SlimUIWindow : EditorWindow {
    [MenuItem("Window/SlimUI Online Documentation")]
    public static void ShowWindow() {
        Application.OpenURL("https://www.slimui.com/documentation");
    }

    [InitializeOnLoad]
    public class InitOnLoad {
        static InitOnLoad() {
            if (!EditorPrefs.HasKey("UsingSlimUIFirstTime")) {
                EditorPrefs.SetInt("UsingSlimUIFirstTime", 1);
                EditorUtility.DisplayDialog("Thank you for choosing SlimUI!",
                                            "Make your project AAA in minutes! If you have any questions, comments, or feedback, feel free to reach out to us at www.slimui.com. Thank you!",
                                            "GOT IT!");
            }
        }
    }
}
