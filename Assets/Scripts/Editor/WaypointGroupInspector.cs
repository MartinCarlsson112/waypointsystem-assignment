using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointGroup))]
public class WaypointGroupInspector : Editor
{
    [SerializeField]
    bool edit = true;

    private const string WAYPOINT_PROPERTY_NAME = "waypoints";
    private const string OPTIONS_PROPERTY_NAME = "options";
    private const string LOOP_PROPERTY_NAME = "loopWaypoints";
    private const string EDIT_BUTTON_NAME = "Toggle Waypoint Handles";
    private const int DOTTED_LINE_LENGTH = 5;

    private GUILayoutOption[] buttonLayout;
    private GUILayoutOption[] waypointLabelLayout;

    private void OnEnable()
    {
        buttonLayout = new GUILayoutOption[]
        {
            GUILayout.Width(25),
            GUILayout.MaxWidth(25),
        };

        waypointLabelLayout = new GUILayoutOption[]
        {
            GUILayout.ExpandWidth(false),
            GUILayout.MaxWidth(25),
        };
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button(EDIT_BUTTON_NAME))
        {
            edit = !edit;
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.LabelField("Waypoints");
        var listPropertyChanged = false;
        var property = serializedObject.FindProperty(WAYPOINT_PROPERTY_NAME);
        if (property != null)
        {
            if (property.propertyType == SerializedPropertyType.Generic && property.isArray)
            {
                //add button if the array is empty
                if(property.arraySize < 1)
                {
                    if (GUILayout.Button("+", buttonLayout))
                    {
                        Add(0, property);
                    }
                }

                for (int i = 0; i < property.arraySize; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    var previousValue = element.vector3Value;

                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(i.ToString() + ":", waypointLabelLayout);
                    element.vector3Value = EditorGUILayout.Vector3Field("", element.vector3Value);

                    if (previousValue != element.vector3Value)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                    WaypointManagementButtons(i, property);
                    GUILayout.EndHorizontal();
                }
            }
        }

        if (listPropertyChanged)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void WaypointManagementButtons(int i, SerializedProperty property)
    {
        if (GUILayout.Button("+", buttonLayout))
        {
            Add(i, property);
        }
        if (GUILayout.Button("-", buttonLayout))
        {
            Remove(i, property);
        }
        if (GUILayout.Button("↑", buttonLayout))
        {
            MoveUp(i, property);
        }
        if (GUILayout.Button("↓", buttonLayout))
        {
            MoveDown(i, property);
        }
    }


    private void Add(int i, SerializedProperty property)
    {
        property.InsertArrayElementAtIndex(i);
        serializedObject.ApplyModifiedProperties();
    }

    private void Remove(int i, SerializedProperty property)
    {
        property.DeleteArrayElementAtIndex(i);
        serializedObject.ApplyModifiedProperties();
    }

    private void MoveUp(int i, SerializedProperty property)
    {
        if(i > 0)
        {
            Swap(i, i - 1, property);
        }
    }

    private void MoveDown(int i, SerializedProperty property)
    {
        if (i < property.arraySize - 1)
        {
            Swap(i, i + 1, property);
        }
    }
    private void Swap(int i, int j, SerializedProperty property)
    {
        var first = property.GetArrayElementAtIndex(i);
        var second = property.GetArrayElementAtIndex(j);
        var temp = first.vector3Value;
        first.vector3Value = second.vector3Value;
        second.vector3Value = temp;
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        if (!Selection.activeObject)
        {
            return;
        }

        var property = serializedObject.FindProperty(WAYPOINT_PROPERTY_NAME);
        var options = serializedObject.FindProperty(OPTIONS_PROPERTY_NAME);
        var loopOption = options.FindPropertyRelative(LOOP_PROPERTY_NAME);

        if (property != null)
        {
            if (property.propertyType == SerializedPropertyType.Generic && property.isArray)
            {
                Vector3 lastPosition = Vector3.zero;

                for (int i = 0; i < property.arraySize; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    var previousPos = element.vector3Value;
                    //position handle if we are in edit mode
                    if (edit)
                    {
                        element.vector3Value = Handles.PositionHandle(element.vector3Value, Quaternion.identity);
                    }
                    Handles.Label(element.vector3Value, i.ToString());
                    DrawLine(i, property, lastPosition, element, loopOption.boolValue);
                    TestPositionChanged(previousPos, element.vector3Value);
                    lastPosition = element.vector3Value;
                }
            }
        }
    }


    private void TestPositionChanged(Vector3 previousPos, Vector3 newPos)
    {
        //check if value changed
        var distance = Vector3.Distance(previousPos, newPos);
        if (distance > Mathf.Epsilon)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }


    private void DrawLine(int i, SerializedProperty property, Vector3 lastPosition, SerializedProperty element, bool looping)
    {
        //draw line between last and current waypoint, exclude first.
        if (i > 0)
        {
            Handles.DrawDottedLine(lastPosition, element.vector3Value, DOTTED_LINE_LENGTH);
        }

        //if looping option is enabled, draw line between last and first
        if (i >= property.arraySize - 1)
        {
            if (looping)
            {
                Handles.DrawDottedLine(element.vector3Value, property.GetArrayElementAtIndex(0).vector3Value, DOTTED_LINE_LENGTH);
            }
        }
    }

}
