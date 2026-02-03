using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Windows;
using JetBrains.Annotations;
using Mono.Cecil;

public class ShipCreatorWindow : EditorWindow
{
    private Mesh _mesh;
    private Material _material;
    private int _columnCount = 32;
    private int _offset;
    private int _shipID;
    private ShipData _shipData;

    private TextAsset _textAsset;

    [MenuItem("Galvasite Tools/Ship Data Creator")]
    public static void Open()
    {
        GetWindow<ShipCreatorWindow>("Ship Data Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Ship Setup", EditorStyles.boldLabel);

        _mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", _mesh, typeof(Mesh), false);
        _material = (Material)EditorGUILayout.ObjectField("Material", _material, typeof(Material), false);
        _textAsset = (TextAsset)EditorGUILayout.ObjectField("TextAsset", _textAsset, typeof(TextAsset), false);
        _shipID = EditorGUILayout.IntField("Ship ID", _shipID);
        _columnCount = EditorGUILayout.IntField("Amount of columns on the Excel Sheet", _columnCount);
        //_offset = EditorGUILayout.IntField("Offset", _offset);


        GUILayout.Space(10);

        if (GUILayout.Button("Create Ship"))
        {

            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);

            string fileName = CreateFileName(textData);

            //string fileName = $"0{_shipID}_{textData[_columnCount * _shipID + _offset]}";

            if (FileAlreadyExists(fileName))
            {
                Debug.Log($"{fileName} already exists");
                return;
            }
            else
            {
                ShipData shipData = CreateInstance<ShipData>();

                UpdateData(shipData, textData);

                AssetDatabase.CreateAsset(shipData, $"Assets/Data/{fileName}.asset");
            }

        }

        if (GUILayout.Button("Update Ship"))
        {
            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);

            string fileName = CreateFileName(textData);

            if (FileAlreadyExists(fileName))
            {
                ShipData shipData = AssetDatabase.LoadAssetAtPath<ShipData>($"Assets/Data/{fileName}.asset");
                UpdateData(shipData, textData);
                return;
            }
            else
            {
                Debug.Log($"{fileName} does not exist");
            }
        }
                
    }

    private void UpdateData(ShipData shipData, string[] textData)
    {
        shipData.ShipName = textData[_columnCount * _shipID + 1];
        shipData.ShipQuality = int.Parse(textData[_columnCount * _shipID + 2]);
        shipData.HUDWeaponName = textData[_columnCount * _shipID + 3];
        shipData.HUDWeaponID = int.Parse(textData[_columnCount * _shipID + 4]);
    }

    private string CreateFileName(string[] textData)
    {
        return $"0{_shipID}_{textData[_columnCount * _shipID + 0]}";
    }

    public bool FileAlreadyExists(string fileName)
    {
        return File.Exists($"Assets/Data/{fileName}.asset");
    }
}
