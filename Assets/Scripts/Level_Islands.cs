using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Level_Islands : MonoBehaviour
{
    public Game_Events game_Events;  
    public bool spawnIslandBool;      
    public GameObject islandLVL1;
    public GameObject islandLVL2;
    public GameObject islandLVL3;
    public GameObject islandLVL4;
    public GameObject game_island;
    public Transform[] islandsOfLevel;
    public Transform currentIsland;
    public List<Transform> currentIslandsOnLevel;
    
    [Tooltip("Pega de Game_Events")]
    [SerializeField]
    private float spawnIslandTime;
    public float spawnIslandTimeAux;
    public bool checkpointSetted;
    public Tile grassCheckpointTile;
    public Tile desertCheckpointTile;

    void Start(){
        // setGameLevelIsland();
        // setIslandTime();
        // resetSpawnIslandTimer();
        // spawnIsland(); // Spawn the first Island
    }

    public void setGameLevelIsland(){

        if (game_Events.getGameLevel() == 1){
            game_island = islandLVL1;
        }else
        if (game_Events.getGameLevel() == 2){
            game_island = islandLVL2;
        }else
        if (game_Events.getGameLevel() == 3){
            game_island = islandLVL3;
        }else
        if (game_Events.getGameLevel() == 4){
            game_island = islandLVL4;
        }

        setIslandsOfLevel();
    }

    public void setIslandTime(){
        spawnIslandTime = game_Events.getSpawnIslandTime();
    }

    public void resetSpawnIslandTimer(){
        spawnIslandTimeAux = spawnIslandTime;
    }

    public void spawnIsland() {
        GameObject spawnGO = game_Events.getSpawnGroundEnemysGO();
        GameObject backgroundGO = game_Events.getBackgorundGO();
        Transform luckyIsland = islandsOfLevel[ Random.Range(0, islandsOfLevel.Length )];

        float respawnSize = spawnGO.transform.localScale.x / 2;

        // Spawna a ilha em um lugar aleatorio do SpawnGO
        currentIsland = Instantiate(luckyIsland, new Vector2(Random.Range( -respawnSize, respawnSize ), spawnGO.transform.position.y), Quaternion.identity, backgroundGO.transform);
        currentIslandsOnLevel.Add(currentIsland);

        spawnGroundEnemyOnIsland();
        // if( game_Events.getEnemysDefeated() >= game_Events.getEnemysToSpawnCheckpoint() ){
        //     spawnCheckpoint();
        //     game_Events.resetEnemysDefeated();
        //     checkpointSetted = false;
        // }else{
        //     checkpointSetted = false;
        //     spawnGroundEnemyOnIsland();
            
        // }
    }

    void Update(){
        if (spawnIslandBool){
            checkSpawnIslandTime();
        }
    }


    private void setIslandsOfLevel()
    {
        islandsOfLevel = new Transform[game_island.transform.childCount];

        for (int i = 0; i < game_island.transform.childCount; i++)
        {
            islandsOfLevel[i] = game_island.transform.GetChild(i);
        }
       
    }

    private void checkSpawnIslandTime() {
        spawnIslandTimeAux -= Time.deltaTime;

        if(spawnIslandTimeAux <= 0){
            spawnIsland();
            resetSpawnIslandTimer();
        }
    }
    

    private void spawnCheckpoint(){
        
        Tilemap tilemap = currentIsland.GetComponent<Tilemap>();
        // List<Vector3> tileWorldLocations;

        BoundsInt bounds = tilemap.cellBounds;                
        // tileWorldLocations = new List<Vector3>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            if(!checkpointSetted){
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = tilemap.CellToWorld(localPlace);

                if (tilemap.HasTile(localPlace)) {
                    var tile = tilemap.GetTile(localPlace);


                    if (tile.name == "Grass_01"){
                        // grassCheckpointGO
                        tilemap.SetTile(localPlace, grassCheckpointTile);
                        checkpointSetted = true;
                    }else if( tile.name == "Desert_01"){
                        // desertCheckpointGO
                        tilemap.SetTile(localPlace, desertCheckpointTile);
                        checkpointSetted = true;
                    }
                }
            }
        }
    }

    private void spawnGroundEnemyOnIsland()
    {   
        int groundEnemysSpawned = 0;
        int maxGroundEnemysinIsland = game_Events.getMaxGroundEnemysInIsland();

        Tilemap tilemap = currentIsland.GetComponent<Tilemap>();
        // List<Vector3> tileWorldLocations;

        BoundsInt bounds = tilemap.cellBounds;                
        // tileWorldLocations = new List<Vector3>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);

            if (tilemap.HasTile(localPlace)) 
            {
                var tile = tilemap.GetTile(localPlace);
                
                if (tile.name == "Grass_01" || tile.name == "Desert_01") 
                {
                    // Debug.Log ("Tile: " + tile.name + " Place: " + place);
                    // tileWorldLocations.Add(place);

                    int _spawn = Random.Range(1, 100);
                    Sprite[] groundEnemys = game_Events.getCurrentGroundEnemys();

                    if (  (_spawn <= game_Events.getSpawnGroundEnemyChance() ) && (groundEnemysSpawned < maxGroundEnemysinIsland) ){
                        GameObject enemyGround = Instantiate(game_Events.getEnemyGroundGO(), place , Quaternion.identity ) as GameObject;
                        enemyGround.GetComponent<EnemyGround>().setSprite( groundEnemys[ Random.Range(0, groundEnemys.Length)] );
                        enemyGround.transform.parent = currentIsland.transform;
                        // Destroy(enemyGround, 25);
                        groundEnemysSpawned += 1;
                    }
                }
            }
        }
    }

    public void setSpawnIslandBool(bool b)
    {
        this.spawnIslandBool = b;
    }

    public void destroyAllIslands(){
        foreach (Transform island in currentIslandsOnLevel)
        {
            //Seta a uma posição abaixo do islandLimit para ser destruida automaticamente pelo script Island Behaviour
            island.transform.position = new Vector2(0, -20);
        }
    }
}