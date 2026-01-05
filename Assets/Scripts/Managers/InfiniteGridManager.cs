using System;
using System.Collections;
using UnityEngine;

public class InfiniteGridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private Transform _playerTransform;

    [Header("Grid Settings")]
    [SerializeField]
    private int _tilesX = 17;
    [SerializeField]
    private int _tilesZ = 17;
    [SerializeField]
    private float _yOffset = -1f;

    [SerializeField]
    private float _tileWorldSizeX = 10f;
    [SerializeField]
    private float _tileWorldSizeZ = 10f;

    private Tile[] allTiles;
    private Vector2Int lastPlayerTileCoord;

    private TileOwnedObject[] _ownedObjects;

    private void Start()
    {
        StartCoroutine(FindObjects());

        GenerateGrid();
        lastPlayerTileCoord = GetPlayerTileCoord();
    }

    private void Update()
    {
        Vector2Int currentPlayerTile = GetPlayerTileCoord();

        if (currentPlayerTile != lastPlayerTileCoord)
        {
            if (currentPlayerTile == null) return;

            UpdateTilePositions(currentPlayerTile);
            lastPlayerTileCoord = currentPlayerTile;
        }
    }
    private void LateUpdate()
    {



        foreach (Tile tile in allTiles)
        {
            tile.EvaluateAuthority();
        }
    }

    private void GenerateGrid()
    {
        allTiles = new Tile[_tilesX * _tilesZ];
        int index = 0;

        for (int x = 0; x < _tilesX; x++)
        {
            for (int z = 0; z < _tilesZ; z++)
            {
                float xPos = (x - _tilesX / 2) * _tileWorldSizeX;

                float yPos = (z - _tilesZ / 2) * _tileWorldSizeZ;

                Vector3 spawnPos = new Vector3(xPos, 0, yPos);

                GameObject tileObject = Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform);

                Tile tileComponent = tileObject.GetComponent<Tile>();

                Vector2Int tileCoordinate = new Vector2Int(x - _tilesX / 2, z - _tilesZ / 2);

                tileComponent.Initialize(tileCoordinate, _tileWorldSizeX, _tileWorldSizeZ, _yOffset);

                allTiles[index++] = tileComponent;
            }
        }
    }

    private void UpdateTilePositions(Vector2Int playerTileCoord)
    {
        foreach (Tile tile in allTiles)
        {
            tile.UpdateTilePosition(playerTileCoord, _tilesX, _tilesZ);
        }
    }


    private Vector2Int GetPlayerTileCoord()
    {
        return new Vector2Int(
            Mathf.FloorToInt(_playerTransform.position.x / _tileWorldSizeX),
            Mathf.FloorToInt(_playerTransform.position.z / _tileWorldSizeZ)
        );
    }

     public IEnumerator FindObjects()
    {
        yield return new WaitForSeconds(.1f);
        _ownedObjects = FindObjectsByType<TileOwnedObject>(FindObjectsSortMode.None);


        foreach (TileOwnedObject obj in _ownedObjects)
        {
            obj.OwningTile = null;
            obj.ClosestSqrDistance = float.MaxValue;
        }
    }
}
