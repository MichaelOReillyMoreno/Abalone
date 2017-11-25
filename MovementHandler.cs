using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eljecuta la sucesion de movimientos de una lista de canicas
/// </summary>

public class MovementHandler : MonoBehaviour {

    [SerializeField]
    private float timeToMoveMarble;

    private Cell CurrentCell_Marble;
    private Cell NextCell_Marble;
    private Marble marbleAux;

    public IEnumerator ExecuteMovement( List<MoveMarble> moves )
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if (!moves[i].hasToDestroy)
            {
                yield return StartCoroutine(MoveMarble(moves[i]));
            }
            else
            {
                yield return StartCoroutine(DestroyMarble(moves[i].CurrentCell));
            }

        }
    }

    private IEnumerator MoveMarble(MoveMarble move)
    {
        CurrentCell_Marble = move.CurrentCell;
        NextCell_Marble = move.NextCell;

        yield return StartCoroutine(CurrentCell_Marble.Marble.MoveTo(CurrentCell_Marble.Position_Placeholder, NextCell_Marble.Position_Placeholder, timeToMoveMarble));

        marbleAux = CurrentCell_Marble.Marble;

        CurrentCell_Marble.Marble = null;
        NextCell_Marble.Marble = marbleAux;
    }

    private IEnumerator DestroyMarble(Cell cellMarble)
    {
        yield return StartCoroutine(cellMarble.Marble.MoveTo(cellMarble.Position_Placeholder, 
            new Vector3(cellMarble.Position_Placeholder.x, cellMarble.Position_Placeholder.y + 2, cellMarble.Position_Placeholder.z), timeToMoveMarble * 2));

        cellMarble.Marble.gameObject.SetActive(false);
        cellMarble.Marble = null;
    }

}
