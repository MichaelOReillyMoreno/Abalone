using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Una posible sucession movimientos que puede realizar la seleccion de canicas, incluidas las canicas del rival afectadas
/// </summary>

public class PosibleMove {

    //Celda sobre la cual, al hacer click inicia dicho movimiento
    public Cell TargetCell { get; set; }
    public List<MoveMarble> Moves { get; set; }
    public Direction direction { get; set; }

    public PosibleMove(Cell targetCell, List<MoveMarble> moves, Direction direction)
    {
        this.TargetCell = targetCell;
        this.Moves = moves;
        this.direction = direction;
    }

    public PosibleMove(List<MoveMarble> moves)
    {
        this.Moves = moves;
    }
}
