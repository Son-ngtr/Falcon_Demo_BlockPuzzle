using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShapes : MonoBehaviour
{

    public Button changebt;
    public Text count;

    public void ChangeShape()
    {
        GameEvents.RequestNewShapes();
        changebt.enabled = false;
        count.text = "0";
    }
}
