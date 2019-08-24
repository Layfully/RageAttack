using System;

namespace UnityEditor.PostProcessing
{
    public class PostProcessingModelEditorAttribute : Attribute
    {
        public readonly Type Type;
        public readonly bool AlwaysEnabled;

        public PostProcessingModelEditorAttribute(Type type, bool alwaysEnabled = false)
        {
            Type = type;
            AlwaysEnabled = alwaysEnabled;
        }
    }
}
