using StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ArrayLayout))]
    public class ArrayLayoutEditor : PropertyDrawer
    {
        private const float CellPadding = 24f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);
            var reposition = position;
            reposition.y += CellPadding;
            var data = property.FindPropertyRelative("rows");

            if (data.arraySize != Config.BoardHeight)
                data.arraySize = Config.BoardHeight;
            for (var j = 0; j < Config.BoardHeight; j++)
            {
                var row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
                reposition.height = CellPadding;
                if (row.arraySize != Config.BoardWidth)
                    row.arraySize = Config.BoardWidth;
                reposition.width = position.width / Config.BoardWidth;
                for (var i = 0; i < Config.BoardWidth; i++)
                {
                    EditorGUI.PropertyField(reposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                    reposition.x += reposition.width;
                }
                reposition.x = position.x;
                reposition.y += CellPadding;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return CellPadding * (Config.BoardHeight + 1);
        }
    }
}
