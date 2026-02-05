using JetBrains.Annotations;
using Mono.Cecil;
using NUnit.Framework.Interfaces;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class ShipCreatorWindow : EditorWindow
{
    private Mesh _mesh;
    private Material _material;
    private int _columnCount = 31;
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

        //_mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", _mesh, typeof(Mesh), false);
        //_material = (Material)EditorGUILayout.ObjectField("Material", _material, typeof(Material), false);
        
        _textAsset = (TextAsset)EditorGUILayout.ObjectField("TextAsset", _textAsset, typeof(TextAsset), false);
        _shipID = EditorGUILayout.IntField("Ship ID", _shipID);
        _columnCount = EditorGUILayout.IntField("Amount of Data Columns", _columnCount);
        //_offset = EditorGUILayout.IntField("Offset", _offset);


        GUILayout.Space(20);
        GUILayout.Label("Create / Update Ships", EditorStyles.boldLabel);
        if (GUILayout.Button("Create Ship"))
        {
            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);
            string fileName = CreateFileName(textData, _shipID);

            if (FileAlreadyExists(fileName))
            {
                Debug.Log($"{fileName} already exists");
                return;
            }
            else
            {
                ShipData shipData = CreateInstance<ShipData>();
                UpdateShipData(shipData, textData, _shipID);
                AssetDatabase.CreateAsset(shipData, $"Assets/Data/{fileName}.asset");
            }
        }

        if (GUILayout.Button("Update Ship"))
        {
            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);

            string fileName = CreateFileName(textData, _shipID);

            if (FileAlreadyExists(fileName))
            {
                ShipData shipData = AssetDatabase.LoadAssetAtPath<ShipData>($"Assets/Data/{fileName}.asset");
                UpdateShipData(shipData, textData, _shipID);
                return;
            }
            else
            {
                Debug.Log($"{fileName} does not exist");
            }
        }


        GUILayout.Space(20);
        GUILayout.Label("Batch Create / Update Ships", EditorStyles.boldLabel);

        if (GUILayout.Button("Batch Create Ships"))
        {
            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);
            int idCount = textData.Length / _columnCount;

            for (int i = 1;  i < idCount; i++)
            {
                string fileName = CreateFileName(textData, i);

                if (FileAlreadyExists(fileName))
                {
                    continue;
                }
                else
                {
                    ShipData shipData = CreateInstance<ShipData>();
                    UpdateShipData(shipData, textData, i);
                    AssetDatabase.CreateAsset(shipData, $"Assets/Data/{fileName}.asset");
                }
            }
        }

        if (GUILayout.Button("Batch Update Ships"))
        {
            string[] textData = _textAsset.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);

            int idCount = textData.Length / _columnCount;

            for (int i = 1; i < idCount; i++)
            {
                string fileName = CreateFileName(textData, i);

                if (FileAlreadyExists(fileName))
                {
                    ShipData shipData = AssetDatabase.LoadAssetAtPath<ShipData>($"Assets/Data/{fileName}.asset");
                    UpdateShipData(shipData, textData, i);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    private void UpdateShipData(ShipData shipData, string[] textData, int ID)
    {
        //sets up general data
        shipData.ShipName = textData[_columnCount * ID + 1];
        shipData.ShipQuality = int.Parse(textData[_columnCount * ID + 2]);

        //sets up Hud data
        shipData.HUDWeaponName = textData[_columnCount * ID + 3];
        shipData.HUDWeaponID = int.Parse(textData[_columnCount * ID + 4]);
        shipData.CrossHairVisual = (CrossHairType)Enum.Parse(typeof(CrossHairType), textData[_columnCount * ID + 6]);

        //sets up health data
        shipData.Health = float.Parse(textData[_columnCount * ID + 7]);

        //sets up movement data
        shipData.MaxSpeed = float.Parse(textData[_columnCount * ID + 8]);
        shipData.Acceleration = float.Parse(textData[_columnCount * ID + 9]);
        shipData.Deceleration = float.Parse(textData[_columnCount * ID + 10]);
        shipData.RotationSpeed = float.Parse(textData[_columnCount * ID + 11]);

        //sets up attack data
        shipData.Damage = float.Parse(textData[_columnCount * ID + 12]);
        shipData.KnockBack = float.Parse(textData[_columnCount * ID + 13]);
        shipData.AttackSpeed = float.Parse(textData[_columnCount * ID + 14]);
        shipData.AttackLifeTime = float.Parse(textData[_columnCount * ID + 15]);
        shipData.AttackDelay = float.Parse(textData[_columnCount * ID + 16]);

        //sets up Screenshake data
        shipData.ScreenShakeIntensity = float.Parse(textData[_columnCount * ID + 17]);
        shipData.ScreenShakeDuration = float.Parse(textData[_columnCount * ID + 18]);
    }

    private string CreateFileName(string[] textData, int ID)
    {
        return $"0{ID}_{textData[_columnCount * ID + 0]}";
    }

    public bool FileAlreadyExists(string fileName)
    {
        return File.Exists($"Assets/Data/{fileName}.asset");
    }
}
