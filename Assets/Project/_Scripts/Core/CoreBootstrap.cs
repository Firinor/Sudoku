using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FirAnimations;
using UnityEngine;

public class CoreBootstrap : MonoBehaviour
{
    public FirAnimation closeСurtain;  
    
    //[SerializeField, Min(9)] 
    //private int NumberOfUniqueTiles;
    [SerializeField] 
    private Settings settings;
    [SerializeField] 
    private TilePool pool;
    [SerializeField] 
    private SudokuRules rules;
    
    [SerializeField] 
    private Transform tileStartAnimationPoint;
    
    [SerializeField] 
    private Material[] floorMaterials;
    
    private SaveData player;
    
    [ContextMenu("DeckInitialize")]
    private IEnumerator Start()
    {
        closeСurtain.Initialize();
        
        yield return null;
        
        closeСurtain.Play();//OpenScene
        
        LoadPlayerData();
        
        settings.Initialize();
        pool.ClearAll(instant: true);
        //StartCoroutine(DeckInitialize(EmptyDesk()));
        rules.Initialize(player);
    }

    private List<SudokuTileView> EmptyDesk()
    {
        //Empty Desk
        List<SudokuTileView> tilesView = new();
        //Dictionary<SudokuTileView, DeckTile> dictionaryViewTile = new();
        Dictionary<Vector3, SudokuTileView> dictionaryTileView = new();
        int index = 0;
        
        /*foreach(var deckTile in desk.TilesPositions)
        {
            SudokuTileView tile = pool.Get();
            tile.DisableVisual();
            int floor = (int)(deckTile.position.z / -0.607f);
            tile.gameObject.name = "Tile z" + floor + "x" + (int)deckTile.position.x + "y" + (int)deckTile.position.y;
            tile.transform.position = deckTile.position;
            tile.SetData(listTiles[index]);
            index++;
            tile.SetDefaultMaterial(floorMaterials[floor]);
            tilesView.Add(tile);
            dictionaryViewTile.Add(tile, deckTile);
            dictionaryTileView.Add(deckTile.position, tile);
        }*/
        
        return tilesView;
    }
    private List<int> FillListWhisTiles(int lastTileIndex)
    {
        List<int> ints = new(lastTileIndex);
        for (int i = 0; i < lastTileIndex; i++)
        {
            int index = i;
            ints.Add(index);
        }

        return ints;
    }

    private void LoadPlayerData()
    {
#if IS_YANDEX
        player = new YGSaveData();
#else
        player = new PrefsSaveData();
#endif
        player.FirstLoad();
        //tileData = tiles.First(t => string.Equals(t.ID, player.TilesID));
    }

    public void Shuffle()
    {
        StartCoroutine(DeckInitialize(pool.GetAll()));
    }
    
    private IEnumerator DeckInitialize(List<SudokuTileView> listTiles)
    {
        rules.UnselectTile();

        yield return null;

        List<SudokuTileView> tilesToSpawn = (player.Difficulty) switch
        {
            0 => DifficultyShuffle.ShuffleEasy(listTiles),
            2 => DifficultyShuffle.ShuffleHard(listTiles),
            _ => DifficultyShuffle.ShuffleNormal(listTiles),
        };

        //Animations
        tilesToSpawn = tilesToSpawn
            .OrderByDescending(t => (int)(t.transform.position.z*10))
            .ThenByDescending(t => (int)(t.transform.position.y*10))
            .ThenBy(t => t.transform.position.x)
            .ToList();
        
        AnimationCurve curve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 1f, 2f, 2f)
        );
        AnimationCurve curveRotation = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(1f, 2f, 2f, 2f)
        );
        int index = 0;
        float tileOffset = 0.003f;
        foreach (var tile in tilesToSpawn)
        {
            Vector3 startPosition = tile.GetComponent<RectTransform>().anchoredPosition3D;
            var animation = tile.gameObject.AddComponent<FirPositionAnimation>();
            animation.OnComplete += () =>
            {
                animation.OnComplete = null;
                Destroy(animation);
            };
            animation.Curve = curve;
            animation.enabled = false;
            Vector3 _startAnimationPosition = tileStartAnimationPoint.position + Vector3.forward * tileOffset * index;
            index++;
            animation.StartPosition = _startAnimationPosition;
            animation.EndPosition = startPosition;
            tile.transform.position = _startAnimationPosition;
            tile.gameObject.SetActive(true);
        }
        
        float delta = 0.04f;
        int tilesCounter = 0;
        SudokuTileView lastTile = tilesToSpawn[^1];
        foreach (var tile in tilesToSpawn)
        {
            if(tile == lastTile)
                continue;
            
            tile.GetComponent<FirPositionAnimation>().Play();
            var animationRotation = tile.gameObject.AddComponent<FirRotationAnimation>();
            animationRotation.StartZoom = Vector3.zero;
            animationRotation.EndZoom = new Vector3(0,180,180);
            animationRotation.OnComplete += () =>
            {
                tile.GetComponent<FirZoomAnimation>().Play();
                animationRotation.OnComplete = null;
                Destroy(animationRotation);
                SoundManager.Instance.PlayTileSelect(transform.position, 0.4f);
                tilesCounter++;
            };
            animationRotation.Curve = curveRotation;
            animationRotation.Play();
            yield return new WaitForSeconds(delta);
            delta *= 0.996f;
        }
        yield return new WaitForSeconds(1);
        
        //lastTile
        lastTile.GetComponent<FirPositionAnimation>().Play();
        var lastAnimationRotation = lastTile.gameObject.AddComponent<FirRotationAnimation>();
        lastAnimationRotation.StartZoom = Vector3.zero;
        lastAnimationRotation.EndZoom = new Vector3(0,180,180);
        lastAnimationRotation.OnComplete += () =>
        {
            lastTile.GetComponent<FirZoomAnimation>().Play();
            //lastTile.EnableShadow();
            lastAnimationRotation.OnComplete = null;
            Destroy(lastAnimationRotation);
            SoundManager.Instance.PlayTileSelect(transform.position);
            tilesCounter++;
        };
        lastAnimationRotation.Curve = curveRotation;
        lastAnimationRotation.Play();
        
        yield return new WaitUntil(() => tilesCounter == tilesToSpawn.Count);

        rules.CheckWinCondition();
    }
}
