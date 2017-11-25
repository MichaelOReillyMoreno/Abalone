using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La Canica no sabe nada de nada, no sabe ni si se puede mover, ni su celda, ni nada, solo es controlada por el jugador o la IA y son quienes deciden los movimientos y si son posibles
/// </summary>

public class Marble : MonoBehaviour {

    public bool isWhite;
    public Light lightSelected;

    public IEnumerator MoveTo(Vector3 currentPosition, Vector3 nextPosition, float time)
    {
        float t = 0f;
        float closeEnough = 0.01f;

        while (Vector3.Distance(transform.position, nextPosition) > closeEnough)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPosition, nextPosition, t);

            yield return 0;
        }

        transform.position = nextPosition;
    }

    public bool IsSameColor(bool b)
    {
        return (isWhite == b);
    }
}
