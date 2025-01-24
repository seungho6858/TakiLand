using UnityEditor;
 
// https://forum.unity.com/threads/shortcut-key-for-lock-inspector.95815/#post-5628082
public static class InspectorLockToggle
{
    private static EditorWindow _mouseOverWindow;
 
    [MenuItem("Mib/Toggle Lock &q")]
    private static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}