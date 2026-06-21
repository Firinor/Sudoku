using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DifficultyShuffle
{
    public static List<SudokuTileView> ShuffleEasy(List<SudokuTileView> listTiles)
    {
        List<SudokuTileView> result = new();
        
        List<Sprite> tilesShuffled = GetShuffeledDatas(listTiles);
        
        List<SudokuTileView> tilesToCheck = new(listTiles);
        
        //Decomposition
        //int layer = 1; //Debug
        while (result.Count < listTiles.Count)
        {
            List<SudokuTileView> tilesLayer = new();
            
            foreach (var checkTile in tilesLayer)
            {
                checkTile.IsPlayable = false;
                checkTile.gameObject.SetActive(false);
                tilesToCheck.Remove(checkTile);
            }
            tilesLayer.Shuffle();
            
            //Debug.Log("Layer" + layer++ + " Count" + tilesLayer.Count);
            result.AddRange(tilesLayer);
        }
        
        //Initialization
        int currentIndex = 0;
        foreach (var tile in result)
        {
            tile.IsPlayable = true;
            currentIndex++;
        }
        
        return result;
    }
    public static List<SudokuTileView> ShuffleNormal(List<SudokuTileView> listTiles)
    {
        List<SudokuTileView> tilesToSpawn = new();
        
        List<Sprite> tilesShuffled = GetShuffeledDatas(listTiles);
        
        //Decomposition
        int Count = listTiles.Count;
        while (tilesToSpawn.Count < Count)
        {
            List<SudokuTileView> tilesToCheck = new(listTiles);
            SudokuTileView randomTile1 = null;
            while(randomTile1 == null)
            {
                if (tilesToCheck.Count > 0)
                {
                    SudokuTileView randomTile = tilesToCheck.PullRandom();
                    randomTile1 = randomTile;
                    listTiles.Remove(randomTile1);
                }
                else
                {
                    SudokuTileView randomTile = listTiles.PullRandom();
                    randomTile1 = randomTile;
                }
                tilesToSpawn.Add(randomTile1);
            }
            SudokuTileView randomTile2 = null;
            while(randomTile2 == null)
            {
                if (tilesToCheck.Count > 0)
                {
                    SudokuTileView randomTile = tilesToCheck.PullRandom();
                    randomTile2 = randomTile;
                    listTiles.Remove(randomTile2);
                }
                else
                {
                    SudokuTileView randomTile = listTiles.PullRandom();
                    randomTile2 = randomTile;
                }
                tilesToSpawn.Add(randomTile2);
            }

            randomTile1.gameObject.SetActive(false);
            randomTile1.IsPlayable = false;
            randomTile2.gameObject.SetActive(false);
            randomTile2.IsPlayable = false;
        }
        
        //Initialization
        int currentIndex = 0;
        foreach (var tile in tilesToSpawn)
        {
            tile.IsPlayable = true;
            currentIndex++;
        }

        return tilesToSpawn;
    }
    public static List<SudokuTileView> ShuffleHard(List<SudokuTileView> listTiles)
    {
        List<Sprite> tilesShuffled = GetShuffeledDatas(listTiles);
        
        tilesShuffled.Shuffle();
        
        //Initialization
        int currentIndex = 0;
        foreach (var tile in listTiles)
        {
            currentIndex++;
        }

        return listTiles;
    }
    
    private static List<Sprite> GetShuffeledDatas(List<SudokuTileView> listTiles)
    {
        List<Sprite> result = new();

        var sorted = result
            .OrderBy(t => t.GetHashCode())
            .ToList();
        
        var pairs = new List<List<Sprite>>();
        for (int i = 0; i < sorted.Count; i += 2)
        {
            pairs.Add(new(){sorted[i], sorted[i+1]});
        }
        
        pairs.Shuffle();
        
        return pairs.SelectMany(p => p).ToList();
    }
}