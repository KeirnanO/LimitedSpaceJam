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
    public int mana;
    public int startingMana;

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

            StartPlayerTurn();

            while (playerTurn)
                yield return null;

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

            print("Enemy Turn");

            yield return null;
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

    public void EnemyTurn()
    {
        //Move all Enemies -- This will be where the AI is called
        foreach (var enemy in enemiesSpawned)
        {
            MoveEntityOnGrid(enemy, enemy.position, enemy.position + Vector2Int.left);
        }


        //Spawn New Enemy
        int x = Grid.instance.columns - 1;
        int y = Random.Range(0, Grid.instance.rows);
        
        Entity newEnemy = Grid.instance.SpawnEntity(x, y, EnemyPrefab);

        if (newEnemy != null)
            enemiesSpawned.Add(newEnemy);
    }

    public void MoveEntityOnGrid(Entity entity, Vector2Int oldPos, Vector2Int newPos)
    {
        Tile[,] gridArray = Grid.instance.gridArray;

        if (newPos.x < 0)
        {
            entity.transform.position = houseSpot.position;
            StopAllCoroutines();
            Application.Quit();
            return;
        }

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
        if (selectedPlant == null)
            return;

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
            Grid.instance.ShowTarget(x, y);
    }

    public void UnShowTarget()
    {
        Grid.instance.UnshowTarget();
    }

    public void SelectPlant(PlaceableScriptableObject plant)
    {
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
            enemiesSpawned.Remove(entity);

        if (playersEntities.Contains(entity))
            playersEntities.Remove(entity);

        Grid.instance.gridArray[entity.position.x, entity.position.y].entity = null;
        Destroy(entity.gameObject);
    }
}
