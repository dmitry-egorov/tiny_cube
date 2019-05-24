namespace Plugins.Lanski.Subjective.Editor
{
    public abstract class ScriptlessEditor : UnityEditor.Editor
    {
        private static readonly string[] _dontIncludeMe = {"m_Script"};
     
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
 
            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}