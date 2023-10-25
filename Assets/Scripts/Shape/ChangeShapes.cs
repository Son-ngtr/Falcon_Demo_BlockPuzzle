using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShapes : MonoBehaviour
{
    // Nút và số lần thay đổi
    public Button changebt;
    public Text count;

    // Thay đổi hình
    public void ChangeShape()
    {
        GameEvents.RequestNewShapes();
        changebt.enabled = false;
        count.text = "0";
    }
}
