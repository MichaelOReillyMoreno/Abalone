using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField]
    private LayerMask Cell;

    [SerializeField]
    private Board board;

    [SerializeField]
    private MovementHandler movementHandler;

    [SerializeField]
    private PossibleMovesCalculator posibleMovesCal;

    private List<PosibleMove> posibleMoves;

    private Cell [] cellsSelected;

    private int rowsDistanceSelections;
    private int columnsDistanceSelections;

    private int rowsDistSelectAbs;
    private int columnsDistSelectAbs;

    private Cell cellHit;
    private Ray ray;
    private RaycastHit hit;

    private bool isInMovement;
    private int indexMove;

    private void Start()
    {
        cellsSelected = new Cell[3];
        posibleMoves = new List<PosibleMove>();
    }

    void Update ()
    {
        if (!isInMovement && HasClicked_OnCell())
        {
            cellHit = hit.transform.GetComponent<Cell>();

            if (cellHit.Marble && cellHit.Marble.isWhite)
            {
                //si no hay ninguna canica seleccionada seleccionamos la primera
                if (!cellsSelected[0])
                {
                    if (!Try_One_MarblesSelection())
                        ResetSelected();
                }
                //Si no, intentamos seleccionar la segunda canica
                else if (!cellsSelected[1] && cellHit != cellsSelected[0])
                {
                    if (posibleMoves.Count > 0)
                        HidePosibleMoves();

                    cellsSelected[1] = cellHit;
                    AssignValuesDistance();

                    //si estan demasiado lejos las canicas
                    if (IsTooLongSelection())
                    {
                        if (!Try_Two_MarblesSelection())
                        {
                            if (!Try_Three_MarblesSelection())
                                ResetSelected();
                        }
                    }
                    else ResetSelected();
                }
                else ResetSelected();
            }
            else if (cellsSelected[0] && CheckHitCell_IsTarget())
            {
                StartCoroutine(MakeMove());
            }
        }
    }

    private IEnumerator MakeMove()
    {
        isInMovement = true;

        ResetSelected();
        yield return StartCoroutine(movementHandler.ExecuteMovement((posibleMoves[indexMove].Moves)));

        isInMovement = false;
    }

    private bool CheckHitCell_IsTarget()
    {
        indexMove = posibleMoves.FindIndex(x => x.TargetCell == cellHit);
        if (indexMove >= 0)
            return true;
        return false;
    }

    private bool Try_One_MarblesSelection()
    {
        cellsSelected[0] = cellHit;
        cellsSelected[0].Marble.lightSelected.enabled = true;

        posibleMoves = posibleMovesCal.Calculate(cellsSelected, 1, true);

        if (posibleMoves.Count > 0)
        {
            ShowPosibleMoves();
            return true;
        }
        return false;
    }

    private bool Try_Two_MarblesSelection()
    {
        cellsSelected[1].Marble.lightSelected.enabled = true;

        //para corregir que en la segunda mitad del tablero se cumpla la condicion de mas abajo se debe invertir el valor
        if (cellsSelected[0].Row > 4 || cellsSelected[1].Row > 4)
            rowsDistanceSelections = -rowsDistanceSelections;

        if (rowsDistSelectAbs < 2 && columnsDistSelectAbs < 2 && (rowsDistanceSelections + columnsDistanceSelections) != 0)
        {
            posibleMoves = posibleMovesCal.Calculate(cellsSelected, 2, true);

            if (posibleMoves.Count > 0)
            {
                ShowPosibleMoves();
                return true;
            }
        }
        return false;
    }

    private bool Try_Three_MarblesSelection()
    {
        if (HalfBoardConditions() ||  SecondHalfBoardConditions() || FirstHalfBoardConditions())
        {
            cellsSelected[2] = TakeCellBetween();

            if (cellsSelected[2].Marble && cellsSelected[2].Marble.isWhite)
            {
                cellsSelected[2].Marble.lightSelected.enabled = true;

                posibleMoves = posibleMovesCal.Calculate(cellsSelected, 3, true);

                if (posibleMoves.Count > 0)
                {
                    ShowPosibleMoves();
                    return true;
                }
            }
        }
        cellsSelected[2] = null;
        return false;
    }

    public bool FirstHalfBoardConditions()
    {
       return (cellsSelected[0].Row <= 5 && cellsSelected[1].Row <= 5) && (rowsDistSelectAbs != 1 && columnsDistSelectAbs != 1);
    }

    public bool HalfBoardConditions()
    {
       return ((cellsSelected[0].Row == 5 && cellsSelected[1].Row == 3) || (cellsSelected[0].Row == 3 && cellsSelected[1].Row == 5)) && (rowsDistSelectAbs == 2 && columnsDistSelectAbs == 1);

    }

    public bool SecondHalfBoardConditions()
    {
       return (cellsSelected[0].Row > 5 || cellsSelected[1].Row > 5) && ((rowsDistSelectAbs == 0 && columnsDistSelectAbs == 2) || (rowsDistSelectAbs == 2 && columnsDistSelectAbs == 0) || (rowsDistSelectAbs + columnsDistSelectAbs == 4));
    }

    private Cell TakeCellBetween()
    {
        //condiciones de hacer linea horizontal
        if (rowsDistanceSelections == 0 && columnsDistanceSelections == -2)
            return board.Get_NextCellLeft(cellsSelected[0]);

        else if (rowsDistanceSelections == 0 && columnsDistanceSelections == 2)
            return board.Get_NextCellRight(cellsSelected[0]);
        
        //condiciones primera mitad incluida linea central
        else if (cellsSelected[0].Row < 5 && cellsSelected[1].Row < 5)
        {
            if (rowsDistanceSelections == 2 && columnsDistanceSelections == 0)
                return board.Get_NextCellFrontLeft(cellsSelected[0]);

            else if (rowsDistanceSelections == 2 && columnsDistanceSelections == 2)
                return board.Get_NextCellFrontRight(cellsSelected[0]);

            else if (rowsDistanceSelections == -2 && columnsDistanceSelections == -2)
                return board.Get_NextCellBackLeft(cellsSelected[0]);

            else if (rowsDistanceSelections == -2 && columnsDistanceSelections == 0)
                return board.Get_NextCellBackRight(cellsSelected[0]);
        }
        //condiciones segunda mitad excluida linea central
        else
        {
            if ((rowsDistanceSelections == 2 && columnsDistanceSelections == 1) || (rowsDistanceSelections == 2 && columnsDistanceSelections == 2))
                return board.Get_NextCellBackRight(cellsSelected[0]);

            else if ((rowsDistanceSelections == 2 && columnsDistanceSelections == -1) || (rowsDistanceSelections == 2 && columnsDistanceSelections == 0))
                return board.Get_NextCellBackLeft(cellsSelected[0]);

            else if ((rowsDistanceSelections == -2 && columnsDistanceSelections == -1) || (rowsDistanceSelections == -2 && columnsDistanceSelections == -2))
                return board.Get_NextCellFrontLeft(cellsSelected[0]);

            else if ((rowsDistanceSelections == -2 && columnsDistanceSelections == 1) || (rowsDistanceSelections == -2 && columnsDistanceSelections == 0))
                return board.Get_NextCellFrontRight(cellsSelected[0]);
        }
        return null;
    }

    private void ResetSelected()
    {
        if (cellsSelected[0])
            cellsSelected[0].Marble.lightSelected.enabled = false;

        if (cellsSelected[1])
            cellsSelected[1].Marble.lightSelected.enabled = false;

        if (cellsSelected[2])
            cellsSelected[2].Marble.lightSelected.enabled = false;

        if (posibleMoves.Count > 0)
            HidePosibleMoves();

        cellsSelected[0] = cellsSelected[1] = cellsSelected[2] = null;
    }

    private void ShowPosibleMoves()
    {
        for (int i = 0; i < posibleMoves.Count; i++)
        {
            posibleMoves[i].TargetCell.ShowDirection(posibleMoves[i].direction);
        }
    }

    private void HidePosibleMoves()
    {
        for (int i = 0; i < posibleMoves.Count; i++)
        {
            posibleMoves[i].TargetCell.HideDirection();
        }
    }

    private void AssignValuesDistance()
    {
        rowsDistanceSelections = cellsSelected[1].Row - cellsSelected[0].Row;
        columnsDistanceSelections = cellsSelected[1].Column - cellsSelected[0].Column;

        rowsDistSelectAbs = Mathf.Abs(rowsDistanceSelections);
        columnsDistSelectAbs = Mathf.Abs(columnsDistanceSelections);
    }

    private bool IsTooLongSelection()
    {
        return (rowsDistSelectAbs > 2 || columnsDistSelectAbs > 2) ? false : true;
    }

    private bool HasClicked_OnCell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, Cell))
            {
                return true;
            }
            else ResetSelected();
        }

        return false;
    }

}
