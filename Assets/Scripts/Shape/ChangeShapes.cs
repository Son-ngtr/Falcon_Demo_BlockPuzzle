using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShapes : MonoBehaviour
{

    public Button changebt;

    public void ChangeShape()
    {
        GameEvents.RequestNewShapes();
        changebt.enabled = false;  
    }
}
