using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StoryModel))]
public class StoryModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 스크립터블 오브젝트 필드 표시
        base.OnInspectorGUI();

        StoryModel storyModel = (StoryModel)target;

        // 변경 사항이 있을 때 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
