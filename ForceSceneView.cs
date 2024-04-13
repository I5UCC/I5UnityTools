#if UNITY_EDITOR

using UnityEngine;

namespace I5Tools
{
    public class ForceSceneView : MonoBehaviour
    {
        private void Awake()
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
    }
}

#endif
