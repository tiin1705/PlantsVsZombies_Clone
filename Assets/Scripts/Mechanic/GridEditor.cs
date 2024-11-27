#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Hiển thị các thuộc tính mặc định

        GridManager gridManager = (GridManager)target;

        if (GUILayout.Button("Recalculate Grid"))
        {
            // Cập nhật và tái vẽ grid nếu cần
            SceneView.RepaintAll();
        }
    }
}
#endif

