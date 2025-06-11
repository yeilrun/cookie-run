using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReadmeSTO))]
public class ReadmeEditor: Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        GUIStyle m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 18;
        m_BodyStyle.richText = true;

        GUIStyle m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 32;

        GUIStyle m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontStyle = FontStyle.Bold;
        m_HeadingStyle.fontSize = 24;
        
        GUILayout.Label("README", m_TitleStyle);
        
        GUILayout.Space(16);
        GUILayout.Label("변수명 지을때", m_HeadingStyle);
        GUILayout.Label("ex) strDailyUserTable", m_BodyStyle);
        
        GUILayout.Space(16);
        GUILayout.Label("스크립트 파일 네임스페이스", m_HeadingStyle);
        GUILayout.Label("namespace 이니셜 {}", m_BodyStyle);
        
        GUILayout.Space(16);
        GUILayout.Label("시작로딩 카메라 시야", m_HeadingStyle);
        SerializedProperty gameStartCamera = serializedObject.FindProperty("gameStartCamera");
        gameStartCamera.stringValue = EditorGUILayout.TextArea(gameStartCamera.stringValue, EditorStyles.textArea);
        
        GUILayout.Space(16);
        GUILayout.Label("메인1 카메라 시야", m_HeadingStyle);
        SerializedProperty gameMain1Camera = serializedObject.FindProperty("gameMain1Camera");
        gameMain1Camera.stringValue = EditorGUILayout.TextArea(gameMain1Camera.stringValue, EditorStyles.textArea);

        GUILayout.Space(16);
        GUILayout.Label("메인2 카메라 시야", m_HeadingStyle);
        SerializedProperty gameMain2Camera = serializedObject.FindProperty("gameMain2Camera");
        gameMain2Camera.stringValue = EditorGUILayout.TextArea(gameMain2Camera.stringValue, EditorStyles.textArea);

        GUILayout.Space(16);
        GUILayout.Label("게임플레이 카메라 시야", m_HeadingStyle);
        SerializedProperty gamePlayCamera = serializedObject.FindProperty("gamePlayCamera");
        gamePlayCamera.stringValue = EditorGUILayout.TextArea(gamePlayCamera.stringValue, EditorStyles.textArea);
        
        GUILayout.Space(16);
        GUILayout.Label("파일 이름", m_HeadingStyle);
        GUILayout.Label("ex) 머테리얼을 만든다 성이니셜M자유", m_BodyStyle);
        
        GUILayout.Space(16);
        GUILayout.Label("게임 시나리오", m_HeadingStyle);
        SerializedProperty scenario = serializedObject.FindProperty("scenario");
        scenario.stringValue = EditorGUILayout.TextArea(scenario.stringValue, EditorStyles.textArea);
        
        serializedObject.ApplyModifiedProperties();
    }
}
