using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BoardGenerator : EditorWindow {

    private  float distance = 0.82f;

    private Transform board;
    private Transform AI;
    private Transform player;

    private Object prefab_Cell;
    private Object prefab_BlackMarble;
    private Object prefab_WhiteMarble;

    private List<GameObject> Cells;

    private float distanceX;
    private float distanceZ;
    private float offsetX;

    private int numTilesInLine;
    private int numMarble;

    [MenuItem("Window/BoardGenerator")]
    static void OpenWindow()
    {
        BoardGenerator window = (BoardGenerator)GetWindow(typeof(BoardGenerator));
        window.minSize = new Vector2(50, 50);
        window.maxSize = new Vector2(600, 100);
        window.Show();
    }
    void OnEnable()
    {
        prefab_Cell = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Cell.prefab", typeof(GameObject));
        prefab_BlackMarble = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Marble_Black.prefab", typeof(GameObject));
        prefab_WhiteMarble = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Marble_white.prefab", typeof(GameObject));

        board = GameObject.FindGameObjectWithTag("Board").transform;
        AI = GameObject.FindGameObjectWithTag("AI").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void OnGUI()
    {
        distance = EditorGUILayout.FloatField("Distance between cells:", distance);
        if (GUILayout.Button("Generate"))
        {
            BuilGrid();
            BuildMarbles();
        }
    }
    public void BuilGrid()
    {
        DeletePreviousBoard();

        //Inicializa valores
        distanceX = 0;
        distanceZ = 0;
        offsetX = 0;

        CreateFirstHalfBoard();
        CreateSecondHalfBoard();

        Cells = new List<GameObject>();
        foreach (Transform child in board) Cells.Add(child.gameObject);
        NameBoard();
    }

    private void BuildMarbles()
    {
        numMarble = 0;

        for (int i = 0; i < 11; i++) Instantiate_WhiteMarble(i);
        for (int i = 13; i < 16; i++) Instantiate_WhiteMarble(i);

        numMarble = 0;

        for (int i = 50; i < 61; i++) Instantiate_BlackMarble(i);
        for (int i = 45; i < 48; i++) Instantiate_BlackMarble(i);

    }

    private void Instantiate_WhiteMarble(int pos)
    {
        GameObject whiteMarble_Clone = null;
        whiteMarble_Clone = Instantiate(prefab_WhiteMarble, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

        whiteMarble_Clone.transform.parent = player;
        whiteMarble_Clone.transform.localPosition = Cells[pos].GetComponent<Cell>().Placeholder.position;
        whiteMarble_Clone.name = "WhiteMarble_" + numMarble;

        Cells[pos].GetComponent<Cell>().Marble = whiteMarble_Clone.GetComponent<Marble>();
        whiteMarble_Clone.GetComponent<Marble>().isWhite = true;
        whiteMarble_Clone.GetComponent<Marble>().lightSelected = whiteMarble_Clone.GetComponent<Marble>().GetComponent<Light>();

        ++numMarble;
    }

    private void Instantiate_BlackMarble(int pos)
    {
        GameObject blackMarble_Clone = null;
        blackMarble_Clone = Instantiate(prefab_BlackMarble, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

        blackMarble_Clone.transform.parent = AI;
        blackMarble_Clone.transform.localPosition = Cells[pos].GetComponent<Cell>().Placeholder.position;
        blackMarble_Clone.name = "BlackMarble_" + numMarble;

        Cells[pos].GetComponent<Cell>().Marble = blackMarble_Clone.GetComponent<Marble>();

        AI.GetComponent<AIManager>().Cells_Marbles.Add(Cells[pos].GetComponent<Cell>());

        ++numMarble;
    }

    private void InstantiateCell()
    {
        GameObject cellClone = null;
        cellClone = Instantiate(prefab_Cell, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;

        cellClone.transform.parent = board;
        cellClone.transform.localPosition = new Vector3(distanceX, 0, distanceZ);
        distanceX += distance;
    }

    private void DeletePreviousBoard()
    {
        Cells = new List<GameObject>();
        foreach (Transform child in board) Cells.Add(child.gameObject);
        Cells.ForEach(child => DestroyImmediate(child));

        List <GameObject> marbles = new List<GameObject>();

        foreach (Transform child in AI) marbles.Add(child.gameObject);
        foreach (Transform child in player) marbles.Add(child.gameObject);

        marbles.ForEach(child => DestroyImmediate(child));
    }

    private void CreateFirstHalfBoard()
    {
        numTilesInLine = 5;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < numTilesInLine; j++)
            {
                InstantiateCell();
            }
            offsetX += distance / 2;
            distanceX = -offsetX;
            distanceZ += distance * 0.9f;
            ++numTilesInLine;
        }
    }

    private void CreateSecondHalfBoard()
    {
        numTilesInLine = 8;
        offsetX = -offsetX;
        distanceX += distance;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < numTilesInLine; j++)
            {
                InstantiateCell();
            }
            offsetX += distance / 2;
            distanceX = offsetX + distance;
            distanceZ += distance * 0.9f;
            --numTilesInLine;
        }
    }

    public void NameBoard()
    {
        Cell[][] cells;

        cells = new Cell[9][];
        cells[0] = new Cell[5];
        cells[1] = new Cell[6];
        cells[2] = new Cell[7];
        cells[3] = new Cell[8];
        cells[4] = new Cell[9];
        cells[5] = new Cell[8];
        cells[6] = new Cell[7];
        cells[7] = new Cell[6];
        cells[8] = new Cell[5];

        int k = 0;

        for (int i = 0; i < cells.Length; i++)
        {
            for (int j = 0; j < cells[i].Length; j++)
            {
                Cells[k].name = "Cell_" + i + "_" + j;
                ++k;
            }
        }
    }
}

