using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [Range(2,32)]
    public int XZsize = 10;
    public int Ysize = 1;

    [SerializeField] List<TileScriptableObject> tilesSO;

    public TileData[][][] tiles;

    public static IslandGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Init tile grid
        InitTiles();
        //Place first random Tile;
        PlaceRandomTile();
        //Place tile with lowest entropy until all tiles are placed
        PlaceTileWithLowestEntropy();
    }

    /// <summary>
    /// Find lowest entropy and not placed tile
    /// </summary>
    /// <returns>The good tile</returns>
    public TileData FindTileWithLowestEntropy()
    {
        // init tile with null
        TileData lowestEntropy = null;
        // for each line
        for (int x = 0; x < XZsize; x++)
        {
            // for each column
            for (int z = 0; z < XZsize; z++)
            {
                // place the current tile in a variable
                TileData tile = tiles[0][x][z];
                // if tile is already placed, continue
                if (tile.placed)
                    continue;

                // if the final tile is null or if the current tile has less validTiles than final tile
                if (lowestEntropy == null || tile.validTiles.Count < lowestEntropy.validTiles.Count)
                {
                    // replace final tile by current tile
                    lowestEntropy = tile;
                }
                // if counts are equal
                else if (tile.validTiles.Count == lowestEntropy.validTiles.Count)
                {
                    // chose one randomely
                    int rdm = UnityEngine.Random.Range(0, 2);
                    lowestEntropy = rdm == 1 ? tile : lowestEntropy;
                }
            }
        }
        // return final tile
        return lowestEntropy;
    }

    /// <summary>
    /// Init tiles array
    /// </summary>
    public void InitTiles()
    {
        // first array is Y axe (stages)
        tiles = new TileData[Ysize][][];
        // for each stage
        for (int y = 0; y < tiles.Length; y++)
        {
            // second array is X axe
            tiles[y] = new TileData[XZsize][];
            for (int x = 0; x < tiles[y].Length; x++)
            {
                // third array is Z axe
                tiles[y][x] = new TileData[XZsize];

                // for each tile
                for (int z = 0; z < tiles[y][x].Length; z++)
                {
                    // init tile with a clone of tilesSO list
                    tiles[y][x][z] = new TileData(x, y, z, new List<TileScriptableObject>(tilesSO));
                }
            }
        }
    }

    /// <summary>
    /// Place the first random tile
    /// </summary>
    public void PlaceRandomTile()
    {
        // place first tile
        tiles[0][0][0].PlaceTile();
    }


    /// <summary>
    /// Place all other tiles
    /// </summary>
    public void PlaceTileWithLowestEntropy()
    {
        // get the tile with the lowest entropy
        TileData tile = FindTileWithLowestEntropy();
        // if found one
        if (tile != null)
        {
            // place it
            tile.PlaceTile();
            // replay this function
            PlaceTileWithLowestEntropy();
        }
    }
}
