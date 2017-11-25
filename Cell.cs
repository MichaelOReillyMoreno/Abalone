using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    public Marble Marble;
    public Transform Placeholder;

    public int Row { get; set; }
    public int Column { get; set; }
    public Vector3 Position_Placeholder { get; set; }

    [SerializeField]
    private Image ArrowImg;

    [SerializeField]
    private RectTransform ArrowTr;

    void Awake ()
    {
        Position_Placeholder = Placeholder.position;
    }

    public void ShowDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:

                ArrowTr.rotation = new Quaternion(0.0f, -0.7f, 0.7f, 0.0f);
                break;

            case Direction.FrontLeft:

                ArrowTr.rotation = new Quaternion(0.3f, -0.7f, 0.7f, 0.3f);
                break;

            case Direction.FrontRight:

                ArrowTr.rotation = new Quaternion(0.7f, -0.3f, 0.3f, 0.7f);
                break;
            case Direction.Right:

                ArrowTr.rotation = new Quaternion(0.7f, 0.0f, 0.0f, 0.7f);
                break;

            case Direction.BackRight:

                ArrowTr.rotation = new Quaternion(0.7f, 0.3f, -0.3f, 0.7f);
                break;

            case Direction.BackLeft:

                ArrowTr.rotation = new Quaternion(-0.3f, -0.7f, 0.7f, -0.3f);
                break;
        }

        ArrowImg.enabled = true;
    }

    public void HideDirection()
    {
        ArrowImg.enabled = false;
    }

    public override string ToString()
    {
        return transform.name + " Col : " + Row + " Column : " + Column;
    }
}
