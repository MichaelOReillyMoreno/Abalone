using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Left = 0, FrontLeft = 1, FrontRight = 2, Right = 3, BackRight = 4, BackLeft = 5 };

/// <summary>
/// Calcula los movimentos posibles de una seleccion de canicas y devuelve una lista de los mismos con una casilla sobre la que hacer click para ejecutar cada una de las posiblilades
/// </summary>

public class PossibleMovesCalculator : MonoBehaviour {

    [SerializeField]
    private Board board;
     
    Direction direction;

    private List<PosibleMove> posibleMoves;

    private PosibleMove posibleMove;
    private List<MoveMarble> moves;

    private Cell[] cellsSelected;
    private Cell[] cellsOrdered;
    private int numMarbles;

    private Cell nextCell;

    private bool isWhite;

    private int pushForce;
    private int pusshForceEnemy;

    public List<PosibleMove> Calculate(Cell[] _cellsSelected, int _numMarbles, bool _isWhite)
    {
        posibleMoves = new List<PosibleMove>();
        direction = Direction.Left;

        numMarbles = _numMarbles;
        cellsSelected = _cellsSelected;
        isWhite = _isWhite;

        Check_PosibleMoves();

        return posibleMoves;
    }

    private void Check_PosibleMoves()
    {
        for (int i = 0; i < 6; i++)
        {
            bool isValidMove = false;

            cellsOrdered = cellsSelected;

            moves = new List<MoveMarble>();
            direction = (Direction)i;

            Cell nextCell_0 = (Check_NextCell(cellsOrdered[0], direction) == true) ? Get_NextCell(cellsOrdered[0], direction) : null;
            cellsOrdered = SortCellsSelected(cellsOrdered, nextCell_0);

            for (int indexMove = 0; indexMove < numMarbles; indexMove++)
            {
                isValidMove = SimulateMoveConsequences(indexMove);

                if (!isValidMove)
                    break;
            }

            if (isValidMove)
            {
                posibleMoves.Add(new PosibleMove(ChooseTargetCell(), moves, direction));
            }
        }
    }

    private bool SimulateMoveConsequences(int indexCell)
    {
        if (Check_NextCell(cellsOrdered[indexCell], direction))
        {
            nextCell = Get_NextCell(cellsOrdered[indexCell], direction);

            if (!nextCell.Marble || Check_IsMarbleSelection(nextCell))
            {
                moves.Add(new MoveMarble(cellsOrdered[indexCell], nextCell));
                return true;
            }
            else if (!nextCell.Marble.IsSameColor(isWhite))
            {
                pushForce = Get_PushForce();

                //En el posible movimiento chocaria con una canica enemiga,
                //Si solo tiene una canica de fuerza en esa direccion, no puede moverse
                if (pushForce > 0 && HasEnoughMarbles_ToMoveEnemy())
                {
                    moves.Add(new MoveMarble(cellsOrdered[indexCell], nextCell));
                    SetToMove_EnemyMarbles();

                    return true;
                }
                return false;
            }
        }
        return false;
    }

    private void SetToMove_EnemyMarbles()
    {
        Cell currentCell = cellsOrdered[0];

        for (int i = 0; i < pusshForceEnemy + 1; i++)
        {
            currentCell = Get_NextCell(currentCell, direction);

            if (Check_NextCell(nextCell, direction))
            {
                nextCell = Get_NextCell(currentCell, direction);
                moves.Insert(0, new MoveMarble(currentCell, nextCell));
            }
            else
            {
                moves.Insert(0, new MoveMarble(currentCell, true));
                break;
            }
        }
    }

    private bool HasEnoughMarbles_ToMoveEnemy()
    {
        pusshForceEnemy = 0;
        Cell enemyCell = Get_NextCell(cellsOrdered[0], direction);

        for (int i = 0; i < pushForce; i++)
        {
            if (Check_NextCell(enemyCell, direction))
            {
                enemyCell = Get_NextCell(enemyCell, direction);

                if (enemyCell.Marble)
                {
                    if(!enemyCell.Marble.IsSameColor(isWhite))
                        ++pusshForceEnemy;
                    else
                        return false;
                }   
            }
        }
        if (pushForce > pusshForceEnemy)
            return true;

        return false;
    }

    private int Get_PushForce()
    {
        int pushForce = 0;
        Cell lastCell = cellsOrdered[cellsOrdered.Length - 1];

        for (int i = 0; i < 2; i++)
        {
            if(Check_NextCell(lastCell, direction))
                lastCell = Get_NextCell(lastCell, direction);

            if (lastCell.Marble && lastCell.Marble.IsSameColor(isWhite))
            {
                ++pushForce;
            }
            else break;
        }
      
        return pushForce;
    }

    private Cell ChooseTargetCell()
    {
        Cell targetCell = cellsOrdered[0];

        while (targetCell.Marble && targetCell.Marble.IsSameColor(isWhite))
        {
            targetCell = Get_NextCell(targetCell, direction);
        }

        return targetCell;
    }

    private bool Check_IsMarbleSelection(Cell cell)
    {
        for (int i = 0; i < numMarbles; i++)
        {
            if (cell == cellsOrdered[i])
                return true;
        }
        return false;
    }

    private Cell[] SortCellsSelected(Cell[] cellsSelected, Cell nextCell_0)
    {
        Cell[] cellsOrdered = new Cell[] { };

        if (numMarbles == 1)
        {
            cellsOrdered = new Cell[] { cellsSelected[0] };
        }
        else if (numMarbles == 2)
        {
            if (!nextCell_0 || !nextCell_0.Marble || !nextCell_0.Marble.IsSameColor(isWhite))
            {
                cellsOrdered = new Cell[] { cellsSelected[0], cellsSelected[1] };
            }
            else
            {
                cellsOrdered = new Cell[] { cellsSelected[1], cellsSelected[0] };
            }
        }
        else if (numMarbles == 3)
        {
            if (!nextCell_0 || !nextCell_0.Marble || !nextCell_0.Marble.IsSameColor(isWhite))
            {
                cellsOrdered = new Cell[] { cellsSelected[0], cellsSelected[2], cellsSelected[1] };
            }
            else
            {
                cellsOrdered = new Cell[] { cellsSelected[1], cellsSelected[2], cellsSelected[0] };
            }
        }

        return cellsOrdered;
    }

    private Cell Get_NextCell(Cell cell, Direction direction)
    {
        Cell nextCell = cell;

        switch (direction)
        {
            case Direction.Left:
                nextCell = board.Get_NextCellLeft(cell);
                break;

            case Direction.FrontLeft:
                nextCell = board.Get_NextCellFrontLeft(cell);
                break;

            case Direction.FrontRight:
                nextCell = board.Get_NextCellFrontRight(cell);
                break;
            case Direction.Right:
                nextCell = board.Get_NextCellRight(cell);
                break;

            case Direction.BackRight:
                nextCell = board.Get_NextCellBackRight(cell);
                break;

            case Direction.BackLeft:
                nextCell = board.Get_NextCellBackLeft(cell);
                break;
        }

        return nextCell;
    }

    private bool Check_NextCell(Cell cell, Direction direction)
    {
        bool isValid = false;

        switch (direction)
        {
            case Direction.Left:
                isValid = board.Check_NextCellLeft(cell);
                break;

            case Direction.FrontLeft:
                isValid = board.Check_NextCellFrontLeft(cell);
                break;

            case Direction.FrontRight:
                isValid = board.Check_NextCellFrontRight(cell);
                break;
            case Direction.Right:
                isValid = board.Check_NextCellRight(cell);
                break;

            case Direction.BackRight:
                isValid = board.Check_NextCellBackRight(cell);
                break;

            case Direction.BackLeft:
                isValid = board.Check_NextCellBackLeft(cell);
                break;
        }

        return isValid;
    }
}
