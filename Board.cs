using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Cell[][] cells { get; set; }

    void Awake ()
    {
        InitializeBoard();
    }

    //------------GETS----------------

    public Cell Get_NextCellLeft (Cell currentCell)
    {
        return cells[currentCell.Row][currentCell.Column - 1];
    }

    public Cell Get_NextCellRight (Cell currentCell)
    {
        return cells[currentCell.Row][currentCell.Column + 1];
    }

/*
  A partir de la fila de en medio (5) las filas estan corridas hacia dentro, en las filas
  superiores o hacia fuera, en las inferiores por lo que su posicion en el array varia.
*/

    public Cell Get_NextCellFrontLeft(Cell currentCell)
    {
        if (currentCell.Row < 4)
        {
            return cells[currentCell.Row + 1][currentCell.Column];
        }
        else
        {
            return cells[currentCell.Row + 1][currentCell.Column - 1];
        }
    }

    public Cell Get_NextCellFrontRight(Cell currentCell)
    {
        if (currentCell.Row < 4)
        {
            return cells[currentCell.Row + 1][currentCell.Column + 1];
        }
        else
        {
            return cells[currentCell.Row + 1][currentCell.Column];
        }
    }

    public Cell Get_NextCellBackLeft(Cell currentCell)
    {
        if (currentCell.Row < 5)
        {
            return cells[currentCell.Row - 1][currentCell.Column - 1];
        }
        else
        {
            return cells[currentCell.Row - 1][currentCell.Column];
        }
    }

    public Cell Get_NextCellBackRight(Cell currentCell)
    {
        if (currentCell.Row < 5)
        {
            return cells[currentCell.Row - 1][currentCell.Column];
        }
        else
        {
            return cells[currentCell.Row - 1][currentCell.Column + 1];
        }
    }

    //------------CHECKS----------------

    public bool Check_NextCellLeft(Cell currentCell)
    {
        if (currentCell.Column > 0)
            return true;
        else
            return false;
    }

    public bool Check_NextCellRight(Cell currentCell)
    {
        if (currentCell.Column < (cells[currentCell.Row].Length - 1))
            return true;
        else
            return false;
    }

    public bool Check_NextCellFrontLeft(Cell currentCell)
    {
        if (currentCell.Row < 4)
        {
            return true;
        }
        else if (currentCell.Column > 0 && currentCell.Row < 8)
        {
            return true;
        }

        return false;
    }

    public bool Check_NextCellFrontRight(Cell currentCell)
    {
        if (currentCell.Row < 4)
        {
            return true;
        }
        else if (currentCell.Column < (cells[currentCell.Row].Length - 1) && currentCell.Row < 8)
        {
            return true;
        }
        return false;
    }

    public bool Check_NextCellBackLeft(Cell currentCell)
    {
        if (currentCell.Row > 4)
        {
            return true;
        }
        else if (currentCell.Column > 0 && currentCell.Row > 0)
        {
            return true;
        }
        return false;
    }

    public bool Check_NextCellBackRight(Cell currentCell)
    {
        if (currentCell.Row > 4)
        {
            return true;
        }
        else if (currentCell.Column < (cells[currentCell.Row].Length - 1) && currentCell.Row > 0)
        {
            return true;
        }
        return false;
    }

    public void InitializeBoard()
    {
        Cell[] cellsAux = new Cell[61];

        for (int i = 0; i < transform.childCount; i++)
        {
            cellsAux[i] = transform.GetChild(i).GetComponent<Cell>();
        }

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
                cells[i][j] = cellsAux[k];
                ++k;

                cells[i][j].Row = i;
                cells[i][j].Column = j;
            }
        }
    }
}
