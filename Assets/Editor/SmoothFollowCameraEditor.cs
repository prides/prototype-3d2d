using UnityEditor;
 
[CustomEditor(typeof(SmoothFollowCamera))]
public class SmoothFollowCameraEditor : Editor
{
    SmoothFollowCamera m_Instance;
    PropertyField[] m_fields;

    public void OnEnable()
    {
        m_Instance = target as SmoothFollowCamera;
        m_fields = ExposeProperties.GetProperties(m_Instance);
    }

    public override void OnInspectorGUI()
    {
        if (m_Instance == null)
        {
            return;
        }
        this.DrawDefaultInspector();
        ExposeProperties.Expose(m_fields);
    }
}