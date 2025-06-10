using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReadmeSTO))]
public class ReadmeEditor: Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle m_BodyStyle = new GUIStyle(EditorStyles.label);
        m_BodyStyle.wordWrap = true;
        m_BodyStyle.fontSize = 18;
        m_BodyStyle.richText = true;

        GUIStyle m_TitleStyle = new GUIStyle(m_BodyStyle);
        m_TitleStyle.fontSize = 32;

        GUIStyle m_HeadingStyle = new GUIStyle(m_BodyStyle);
        m_HeadingStyle.fontStyle = FontStyle.Bold;
        m_HeadingStyle.fontSize = 24;
        
        GUILayout.Label("Readme", m_TitleStyle);
        GUILayout.Space(16);
        GUILayout.Label("변수", m_HeadingStyle);
        GUILayout.Label("ex) ", m_BodyStyle);
    }
}
