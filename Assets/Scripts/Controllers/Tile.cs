using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private Vector2Int _tileCoordinate;
    public TextMeshProUGUI TextMesh;

    private float _tileSizeX;
    private float _tileSizeZ;
    private float _yOffset;

    private readonly List<Transform> _containment = new();


    public void Initialize(Vector2Int tileCoordinate, float tileSizeX, float tileSizeZ, float yOffset)
    {
        _tileCoordinate = tileCoordinate;
        _tileSizeX = tileSizeX;
        _tileSizeZ = tileSizeZ;
        _yOffset = yOffset;

        UpdateWorldPosition();
    }



    public void UpdateTilePosition(Vector2Int playerTileCoord, int totalTilesX, int totalTilesZ)
    {
        int halfX = totalTilesX / 2;
        int halfZ = totalTilesZ / 2;

        Vector2Int newTileCoordinate = _tileCoordinate;

        int differenceX = _tileCoordinate.x - playerTileCoord.x;
        int differenceZ = _tileCoordinate.y - playerTileCoord.y;

        if (differenceX > halfX)
        {
            newTileCoordinate.x -= totalTilesX;
        }
        else if (differenceX < -halfX)
        {
            newTileCoordinate.x += totalTilesX;
        }


        if (differenceZ > halfZ)
        {
            newTileCoordinate.y -= totalTilesZ;
        }
        else if (differenceZ < -halfZ)
        {
            newTileCoordinate.y += totalTilesZ;

        }

        if (newTileCoordinate != _tileCoordinate)
        {
            Vector3 oldWorldPosition = transform.position;

            _tileCoordinate = newTileCoordinate;

            Vector3 newWorldPosition = GetWorldPositionFromTileCoord();
            Vector3 posDifference = newWorldPosition - oldWorldPosition;


            UpdateWorldPosition();

            //if (TextMesh != null)
            //    TextMesh.text = $"{_tileCoordinate.x}, {_tileCoordinate.y}";

            UpdateContainmentPosition(posDifference);
        }
    }

    private void UpdateContainmentPosition(Vector3 posDifference)
    {
        foreach (Transform objTransform in _containment)
        {
            TileOwnedObject owned = objTransform.GetComponent<TileOwnedObject>();

            if (owned != null && owned.OwningTile == this)
            {
                objTransform.position += posDifference;
            }
        }
    }

    public void EvaluateAuthority()
    {
        foreach (Transform objTransform in _containment)
        {
            if (objTransform == null) return;

            TileOwnedObject owned = objTransform.GetComponent<TileOwnedObject>();
            if (owned == null)
                continue;

            Vector3 delta = objTransform.position - transform.position;
            float sqrDistance = delta.sqrMagnitude;

            // Only claim the object if it's closer than its current tile
            if (owned.OwningTile == null || sqrDistance < owned.ClosestSqrDistance)
            {
                owned.ClosestSqrDistance = sqrDistance;
                owned.OwningTile = this;
            }
        }
    }

    //private bool CheckIfOwnObject(Transform objectTransform)
    //{
    //    float deltaX = objectTransform.position.x - transform.position.x;
    //    float deltaZ = objectTransform.position.z - transform.position.z;

    //    Debug.Log($"{deltaX}, {deltaX < _tileSizeX / 2}, {deltaZ}, {deltaZ < _tileSizeZ / 2}");

    //    return (deltaX < _tileSizeX / 2 && deltaZ < _tileSizeZ / 2);
    //}

    private void UpdateWorldPosition()
    {
        transform.position = GetWorldPositionFromTileCoord();

        if (TextMesh != null)
            TextMesh.text = $"{_tileCoordinate.x}, {_tileCoordinate.y}";
    }

    private Vector3 GetWorldPositionFromTileCoord()
    {
        return new Vector3(_tileCoordinate.x * _tileSizeX, _yOffset, _tileCoordinate.y * _tileSizeZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        //i dont want these to be added
        if (other.transform.tag == "Possession") return;
        if (other.transform.tag == "Player") return;
        if (other.transform.tag == "Enemy") return;

        if (!_containment.Contains(other.transform))
        {
            _containment.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _containment.Remove(other.transform);
    }

    private void DecideColor()
    {
        if (_tileCoordinate.x % 2 == 0)
        {
            if (_tileCoordinate.y % 2 == 0)
                Gizmos.color = new Color32(235, 235, 157, 255);
            else
                Gizmos.color = new Color32(244, 177, 131, 255);
        }
        else
        {
            if (_tileCoordinate.y % 2 == 0)
                Gizmos.color = new Color32(105, 166, 65, 255);
            else
                Gizmos.color = new Color32(230, 85, 99, 255);
        }
    }

    bool iscol;

    private void OnDrawGizmos()
    {
        if (!iscol)
            DecideColor();

        Gizmos.DrawWireCube(transform.position, new Vector3(_tileSizeX, 0, _tileSizeZ));

        foreach (Transform obj in _containment)
        {
            if (obj == null) return;

            TileOwnedObject owned = obj.GetComponent<TileOwnedObject>();

            if (owned != null && owned.OwningTile == this)
            {
                Gizmos.DrawLine(transform.position, obj.position);
            }
        }
    }

}
