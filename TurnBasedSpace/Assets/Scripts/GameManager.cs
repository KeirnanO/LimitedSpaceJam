using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Entity EnemyPrefab;
    public Entity FlowerPrefab;
    public List<Entity> enemiesSpawned = new List<Entity>();

    public List<Entity> playersEntities = new List<Entity>();
    public Transform houseSpot;
    public Transform epd;
    public int mana;
    public int startingMana;

    public int enemiesKilled = 0;

    PlaceableScriptableObject selectedPlant = null;
    bool playerTurn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }
    IEnumerator GameLoop()
    {
        IncreaseMana(startingMana);

        while (true)
        {
            playerTurn = true;
            //Start of Player Turn

            IncreaseMana(10);
            UIManager.instance.CreateEnergyText(epd.position, 10);

            StartPlayerTurn();

            while (playerTurn)
                yield return new WaitForSeconds(1f);

            int x = 0;

            foreach (var entity in playersEntities)
            {
                
                if (entity.GetType() == typeof(RangedUnit))
                {
                    x++;

                    RangedUnit unit = entity as RangedUnit;
                    unit.StartFire();
                }
            }

            if(x > 0)
                yield return new WaitForSeconds(3f);

            //Once Player Turn has ended start the enemyTurn;
            EnemyTurn();

            yield return null;
        }
    }

    IEnumerator TrySwapTiles(Tile tile1, Tile tile2)
    {
        //Cancel all Inputs 

        //Swap Tiles
        Transform entity1 = null;
        Transform entity2 = null;
        
        if(tile1.entity != null)
            entity1 = tile1.entity.transform;
        if (tile2.entity != null)
            entity2 = tile2.entity.transform;

        float distance = 0;
        while (distance < 1)
        {
            if (entity1 == null && entity2 == null)
            {
                distance = 1;
                break;
            }
            
            if(entity1 != null)
                entity1.position = Vector3.Lerp(tile1.transform.position, tile2.transform.position, distance);
            if(entity2 != null)
                entity2.position = Vector3.Lerp(tile2.transform.position, tile1.transform.position, distance);

            distance += Time.deltaTime * 5;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        GridMerge.SwapTiles(tile1.gridPos.x, tile1.gridPos.y, Grid.instance.gridArray);

        List<Tile> chain1;
        List<Tile> chain2;
        

        if(GridMerge.TrySwapTile(tile1.gridPos.x, tile1.gridPos.y, out chain1, out chain2))
        {
            if(chain1 != null)
                if(chain1.Count > 2)
                    yield return MergeTiles(chain1);
            if(chain2 != null)
                if (chain2.Count > 2)
                    yield return MergeTiles(chain2);
        }
        else
        {
            distance = 0;

            while (distance < 1)
            {
                if (entity1 == null && entity2 == null)
                {
                    distance = 1;
                    break;
                }

                if (entity1 != null)
                    entity1.position = Vector3.Lerp(tile2.transform.position, tile1.transform.position, distance);
                if (entity2 != null)
                    entity2.position = Vector3.Lerp(tile1.transform.position, tile2.transform.position, distance);

                distance += Time.deltaTime * 10;
                yield return null;
            }

            GridMerge.SwapTiles(tile1.gridPos.x, tile1.gridPos.y, Grid.instance.gridArray);
        }
    }

    IEnumerator MergeTiles(List<Tile> chain)
    {
        float distance = 0;

        while (distance < 1)
        {
            for(int i = 1; i < chain.Count; i++)
            {
                chain[i].entity.transform.position = Vector3.Lerp(chain[i].transform.position, chain[0].transform.position, distance);
            }

            distance += Time.deltaTime * 8;
            yield return null;
        }

        PlaceableScriptableObject mergedUnit = ObjectDatabase.instance.GetObjectID(chain[0].entity.objectID.upgradedUnit);

        for (int i = 0; i < chain.Count; i++)
        {
            DestroyEntity(chain[i].entity);
        }

        if (chain[0].entity != null)
            chain[0].entity = null;

        Grid.instance.SpawnEntity(chain[0], mergedUnit.EntityPrefab);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (Grid.instance.unitPreview == true)
            {
                Grid.instance.RotateTarget();
            }
        }

    }

    public void IncreaseMana(int amount)
    {
        mana += amount;

        //BAD
        UIManager.instance.playerEnergyText.SetText("Energy: " + mana.ToString());
    }

    public void EndPlayerTurn()
    {
        playerTurn = false;
    }

    public void StartPlayerTurn()
    {
        foreach (var entity in playersEntities)
        {
            entity.OnRoundStart();
        }
    }

    //This currently holds only AI Logic that should be called from another class
    public void EnemyTurn()
    {
        //Move all Enemies -- This will be where the AI is called
        foreach (var enemy in enemiesSpawned)
        {
            // MoveEntityOnGrid(enemy, enemy.position, enemy.position + Vector2Int.left);
            MoveEnemy(enemy, enemy.position, enemy.position);
        }

        if(enemiesKilled > 30)
        {

            if(enemiesSpawned.Count == 0)
            {
                //YOU WIN
                StopAllCoroutines();
                Application.Quit();
            }


            return;
        }

        SpawnEnemy();

       // FourDirSpawn(Random.Range(0, 4));
        //FourDirSpawn(Random.Range(0, 4));
    }

    public void SpawnEnemy()
    {
        //Spawn New Enemy
        int x = Grid.instance.columns - 1;
        int y = Random.Range(0, Grid.instance.rows);

        Entity newEnemy = Grid.instance.SpawnEntity(x, y, EnemyPrefab);

        if (newEnemy != null)
            enemiesSpawned.Add(newEnemy);
    }

    public void FourDirSpawn(int random)
    {
        Entity newEnemy;
        
        if (random == 0)
        {
            newEnemy = Grid.instance.SpawnEntity(Random.Range(6, 9), 15, EnemyPrefab);
            newEnemy.SetDir(DirSpawned.NORTH);
            if (newEnemy != null)
                enemiesSpawned.Add(newEnemy);
        }

        if (random == 1)
        {
            newEnemy = Grid.instance.SpawnEntity(Random.Range(6, 9), 0, EnemyPrefab);
            newEnemy.SetDir(DirSpawned.SOUTH);
            if (newEnemy != null)
                enemiesSpawned.Add(newEnemy);
        }

        if (random == 2)
        {
            newEnemy = Grid.instance.SpawnEntity(0, Random.Range(6, 9), EnemyPrefab);
            newEnemy.SetDir(DirSpawned.WEST);
            if (newEnemy != null)
                enemiesSpawned.Add(newEnemy);
        }

        if (random == 3)
        {
            newEnemy = Grid.instance.SpawnEntity(15, Random.Range(6, 9), EnemyPrefab);
            newEnemy.SetDir(DirSpawned.EAST);
            if (newEnemy != null)
                enemiesSpawned.Add(newEnemy);
        }

        
    }

    public void MoveEnemy(Entity entity, Vector2Int oldPos, Vector2Int newPos)
    {

        switch (entity.dirSpawned)
        {
            default: newPos = Vector2Int.zero; Debug.Log("Zero Executed");  break;

            case DirSpawned.NORTH:
                newPos += Vector2Int.down; break;

            case DirSpawned.SOUTH:
                newPos += Vector2Int.up; break;

            case DirSpawned.EAST:
                newPos += Vector2Int.left; break;

            case DirSpawned.WEST:
                newPos += Vector2Int.right; break;

        }

        Tile[,] gridArray = Grid.instance.gridArray;

        if (gridArray[newPos.x, newPos.y].entity == null)
        {
            entity.GetComponent<Animator>().SetBool("Eat", false);
            gridArray[newPos.x, newPos.y].entity = entity;
            Debug.Log(newPos.x);
            Debug.Log(newPos.y);
            Debug.Log(oldPos.x);
            Debug.Log(oldPos.y);
            gridArray[oldPos.x, oldPos.y].entity = null;
            entity.position = newPos;
            entity.transform.position = gridArray[newPos.x, newPos.y].transform.position;
        }

        if (gridArray[newPos.x, newPos.y].entity.type.Equals(EntityType.Plant))
        {
            entity.GetComponent<Animator>().SetBool("Eat", true);
            gridArray[newPos.x, newPos.y].entity.TakeDamage(1);

            if (gridArray[newPos.x, newPos.y].entity.health <= 0)
            {
                entity.GetComponent<Animator>().SetBool("Eat", false);
                DestroyEntity(gridArray[newPos.x, newPos.y].entity);

                gridArray[newPos.x, newPos.y].entity = null;
            }
        }
    }

    //This is a Temp AI Function
    public void MoveEntityOnGrid(Entity entity, Vector2Int oldPos, Vector2Int newPos)
    {
        Tile[,] gridArray = Grid.instance.gridArray;

        //if (newPos.x < 0)
        //{
        //    //YOU LOSE
        //    entity.transform.position = houseSpot.position;
        //    StopAllCoroutines();
        //    Application.Quit();
        //    return;
        //}

        if (gridArray[newPos.x, newPos.y].entity == null)
        {
            entity.GetComponent<Animator>().SetBool("Eat", false);
            gridArray[newPos.x, newPos.y].entity = entity;
            gridArray[oldPos.x, oldPos.y].entity = null;
            entity.position = newPos;
            entity.transform.position = gridArray[newPos.x, newPos.y].transform.position;
        }
        
        if(gridArray[newPos.x, newPos.y].entity.type.Equals(EntityType.Plant))
        {
            entity.GetComponent<Animator>().SetBool("Eat", true);
            gridArray[newPos.x, newPos.y].entity.TakeDamage(1);

            if(gridArray[newPos.x, newPos.y].entity.health <= 0)
            {
                entity.GetComponent<Animator>().SetBool("Eat", false);
                DestroyEntity(gridArray[newPos.x, newPos.y].entity);

                gridArray[newPos.x, newPos.y].entity = null;
            }
        }
    }

    public void ClickTile(Tile tile)
    {
        if (!playerTurn)
            return;

        if (selectedPlant == null)
        {
            TrySwap(tile, Grid.instance.gridArray[tile.gridPos.x + 1, tile.gridPos.y]);
            return;
        }

        Entity newEntity = Grid.instance.SpawnEntity(tile, selectedPlant.EntityPrefab);

        if (newEntity == null)
            return;

        playersEntities.Add(newEntity);

        IncreaseMana(-selectedPlant.cost);
        selectedPlant = null;
        UnShowTarget();
    }

    public void ShowTarget(int x, int y)
    {
        if(selectedPlant != null)
            Grid.instance.ShowTarget(x, y, selectedPlant.EntityPrefab);
    }

    public void UnShowTarget()
    {
        Grid.instance.UnshowTarget();
    }

    public void SelectPlant(PlaceableScriptableObject plant)
    {
        if (!playerTurn)
            return;

        if (mana < plant.cost)
        {
            selectedPlant = null;
            return;
        }

        if (selectedPlant == plant)
        {
            selectedPlant = null;
            return;
        }

        selectedPlant = plant;
    }
    

    public void DestroyEntity(Entity entity)
    {
        if (enemiesSpawned.Contains(entity))
        {
            enemiesKilled++;
            enemiesSpawned.Remove(entity);
        }

        if (playersEntities.Contains(entity))
            playersEntities.Remove(entity);

        Grid.instance.gridArray[entity.position.x, entity.position.y].entity = null;
        Destroy(entity.gameObject);
    }

    public void TrySwap(Tile tile1, Tile tile2)
    {
        StartCoroutine(TrySwapTiles(tile1, tile2));
    }
}
