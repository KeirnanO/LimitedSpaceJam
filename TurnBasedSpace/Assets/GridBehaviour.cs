using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GridBehaviour : MonoBehaviour
{
    public bool findDistance = false;

    public int rows = 10;
    public int columns = 10;
    public int scale = 2;

    [Header("Hex")]
    public GameObject gridPrefab;
    public Vector3 origin = new Vector3(0, 0, 0);

    public GameObject[,] gridArray;

    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;

    public List<GameObject> path = new List<GameObject>();
    public GameObject dummy;

    int counter;

    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab) GenerateGrid();
    }

    private void Start()
    {
        dummy = GameObject.Find("Dummy");
    }

    // Update is called once per frame
    void Update()
    {
        if (findDistance)
        {
            if (!dummy) return;


            if (path.Count > 1)
            {
                counter = path.Count;
                StartCoroutine(MoveDummy());
            }

            findDistance = false;
        }
    }

    IEnumerator MoveDummy()
    {
        while (counter > -1)
        {
            if (Vector3.Distance(dummy.transform.position, path[counter - 1].transform.position) < .5f)
            {
                counter--;

                if (counter == 1)
                {
                    yield return null;
                }

                Debug.Log(counter);
            }

            dummy.gameObject.transform.position = Vector3.MoveTowards(dummy.gameObject.transform.position, path[counter - 1].transform.position, 2f * Time.deltaTime);
            yield return null;
        }
    }
    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(origin.x + scale * i, origin.y, origin.z + scale * j), Quaternion.identity);
                obj.transform.SetParent(this.gameObject.transform);


                gridArray[i, j] = obj;
            }
        }
    }

    /*
    void SetDistance()
    {
        InitialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];

        // go through max steps as possible
        for (int step = 1; step < rows * columns; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if (obj && obj.GetComponent<GridStat>().visited == step - 1)
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;

        List<GameObject> tempList = new List<GameObject>();
        path.Clear();

        // if exists and can be visited
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            path.Add(gridArray[x, y]);
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
        }
        else
        {
            // cant reach location
            Debug.Log("cant reach");
            return;
        }

        for (int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))// up 
                tempList.Add(gridArray[x, y + 1]);
            if (TestDirection(x, y, step, 2)) // right
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3)) // down
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4)) // left
                tempList.Add(gridArray[x - 1, y]);

            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNum = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNum = i;

            }
        }
        return list[indexNum];
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1)) // up
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2)) // right
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3)) // down
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4)) // left
            SetVisited(x - 1, y, step);
    }

    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            if (obj)
                obj.GetComponent<GridStat>().visited = -1;
        }

        // 0 is current loc
        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction) // 1 = up 2 = right 3 = down 4 = left
    {
        switch (direction)
        {
            case 1: // check above
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step) return true;
                break;

            case 2: // check right
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step) return true;
                break;

            case 3: // check down
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step) return true;
                break;

            case 4: // check left
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step) return true;
                break;
        }
        return false;
    }

    void SetVisited(int x, int y, int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStat>().visited = step;
    }
    #endregion
    */
}
