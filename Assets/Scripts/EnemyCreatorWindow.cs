using UnityEditor;
using UnityEngine;

public class EnemyCreatorWindow : EditorWindow
{
    private Mesh _mesh;
    private Material _material;
    private float _speed;
    private int _health;

    [MenuItem("Tools/Enemy Creator")]
    public static void Open()
    {
        GetWindow<EnemyCreatorWindow>("Enemy Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Enemy Setup", EditorStyles.boldLabel);

        _mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", _mesh, typeof(Mesh), false);
        _material = (Material)EditorGUILayout.ObjectField("Material", _material, typeof(Material), false);

        _speed = EditorGUILayout.FloatField("Speed", _speed);
        _health = EditorGUILayout.IntField("Health", _health);

        GUILayout.Space(10);

        if (GUILayout.Button("Create Enemy"))
        {
            //CreateEnemy();
        }
    }
}
