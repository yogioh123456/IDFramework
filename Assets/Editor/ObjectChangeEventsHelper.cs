using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public class ObjectChangeEventsHelper
{
    static ObjectChangeEventsHelper()
    {
        ObjectChangeEvents.changesPublished += ChangesPublished;
    }

    private static void ChangesPublished(ref ObjectChangeEventStream stream) {
        if (Application.isPlaying) {
            return;
        }
        for (int i = 0; i < stream.length; ++i) {
            switch (stream.GetEventType(i)) {
                case ObjectChangeKind.CreateGameObjectHierarchy:
                    //UI父节点自动挂上CanvasGroup
                    stream.GetCreateGameObjectHierarchyEvent(i, out var changeGameObjectParent);
                    var newGameObject = EditorUtility.InstanceIDToObject(changeGameObjectParent.instanceId) as GameObject;
                    if (newGameObject.transform.parent != null) {
                        if (newGameObject.transform.parent.name.Equals("Canvas") && newGameObject.GetComponent<Image>() != null) {
                            newGameObject.AddComponent<CanvasGroup>();
                        }
                    }
                    break;
            }
        }
    }
}
