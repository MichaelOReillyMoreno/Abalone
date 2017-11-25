using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Un movimiento que puede realizar una canica
/// </summary>

public class MoveMarble {

    public Cell CurrentCell { get; set; }
    public Cell NextCell{ get; set; }
    public bool hasToDestroy { get; set; }

    public MoveMarble(Cell currentCell, Cell nextCell)
    {
        this.CurrentCell = currentCell;
        this.NextCell = nextCell;
    }

    public MoveMarble(Cell currentCell, bool hasToDestroy)
    {
        this.CurrentCell = currentCell;
        this.hasToDestroy = hasToDestroy;
    }

    public override string ToString()
    {
        return "Move from : " + CurrentCell.name + " To : " + NextCell.name;
    }

}
