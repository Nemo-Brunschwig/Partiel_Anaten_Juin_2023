using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public int x, y, z;

    public bool placed;

    public List<TileScriptableObject> validTiles;

    //public bool updated;
    public int entropy { get { return validTiles.Count; } }

    private TileScriptableObject tilePlaced;

    /// <summary>
    /// Init Tile
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    /// <param name="z">Z position</param>
    /// <param name="validTiles">A new TileScriptableObject list</param>
    public TileData(int x, int y, int z, List<TileScriptableObject> validTiles)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.validTiles = validTiles;
        placed = false;
    }

    /// <summary>
    /// Place the chosen tile
    /// </summary>
    public void PlaceTile()
    {
        // mark this tile as placed
        placed = true;
        if (validTiles.Count != 0)
        {
            // chose a shap randomely in the valid tiles list
            int randTileId = UnityEngine.Random.Range(0, validTiles.Count);
            // and place it in a variable
            tilePlaced = validTiles[randTileId];
            // instantiate the tile
            GameObject.Instantiate(tilePlaced.prefab, new Vector3(x, y, z), tilePlaced.prefab.transform.rotation, IslandGenerator.instance.transform);
            // collapse neighbours
            CollapseNeighbours();
        }
    }

    /// <summary>
    /// Collapse all neighbours
    /// </summary>
    public void CollapseNeighbours()
    {
        // init direction variable
        Dir dir;
        TileData neighbourTile;
        // start with left direction
        if (x - 1 >= 0)
        {
            // define left in direction variable
            dir = Dir.LEFT;
            // get neighbour in a variable
            neighbourTile = IslandGenerator.instance.tiles[y][x - 1][z];
            // check if it is not already placed
            if (!neighbourTile.placed)
                // collapse it
                neighbourTile.Collapse(tilePlaced.validNeighbours[(int)dir], dir);
        }
        // etc...
        if (x + 1 < IslandGenerator.instance.XZsize)
        {
            dir = Dir.RIGHT;
            neighbourTile = IslandGenerator.instance.tiles[y][x + 1][z];
            if (!neighbourTile.placed)
                neighbourTile.Collapse(tilePlaced.validNeighbours[(int)dir], dir);
        }
        if (z - 1 >= 0)
        {
            dir = Dir.BACK;
            neighbourTile = IslandGenerator.instance.tiles[y][x][z - 1];
            if (!neighbourTile.placed)
                neighbourTile.Collapse(tilePlaced.validNeighbours[(int)dir], dir);
        }
        if (z + 1 < IslandGenerator.instance.XZsize)
        {
            dir = Dir.FORWARD;
            neighbourTile = IslandGenerator.instance.tiles[y][x][z + 1];
            if (!neighbourTile.placed)
                neighbourTile.Collapse(tilePlaced.validNeighbours[(int)dir], dir);
        }
    }

    /// <summary>
    /// Delete none valid tiles
    /// </summary>
    /// <param name="neighbourID">The ID of the neighbour</param>
    /// <param name="neighbourDir">The direction of the neighbour</param>
    public void Collapse(int neighbourID, Dir neighbourDir)
    {
        // Get the valid symmetrical ID of the opposit direction
        int id = symmetricalNeighbour[neighbourID];
        Dir dir = GetOppositeDir(neighbourDir);

        // Remove all none valid tile
        for(int i = 0; i < validTiles.Count; i++)
        {
            if (validTiles[i].validNeighbours[(int)dir] != id)
            {
                validTiles.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Get opposit dir
    /// </summary>
    /// <param name="dir">First dir</param>
    /// <returns>Opposit dir</returns>
    /// <exception cref="ArgumentException">WTF dude</exception>
    public static Dir GetOppositeDir(Dir dir)
    {
        switch (dir)
        {
            case Dir.LEFT:
                return Dir.RIGHT;

            case Dir.RIGHT:
                return Dir.LEFT;

            case Dir.FORWARD:
                return Dir.BACK;

            case Dir.BACK:
                return Dir.FORWARD;

            case Dir.UP:
                return Dir.DOWN;

            case Dir.DOWN:
                return Dir.UP;
            default:
                throw new ArgumentException("Dir not implemented");
        }
    }

    /// <summary>
    /// Get symmetrical neighbour
    /// </summary>
    public static Dictionary<int, int> symmetricalNeighbour = new Dictionary<int, int>
    {
        {0,0},
        {1,1},
        {2,3},
        {3,2},
        {-1,-1}
    };

    /// <summary>
    /// Good string to debug
    /// </summary>
    /// <returns>Thanks Remi</returns>
    public override string ToString()
    {
        return "("+ x + "," + y + "," + z + ")" + " placed :" + placed.ToString(); 
    }

}
