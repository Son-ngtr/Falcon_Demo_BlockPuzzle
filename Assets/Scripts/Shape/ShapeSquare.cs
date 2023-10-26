using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Quản lý các ô hình vuông: kích hoạt, hủy bỏ, cập nhật xung đột, xóa cập nhật xung đột
public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }
    
    public void DeactivateShape()
    {
        // Tắt vùng va chạm của một BoxCollider
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void ActivateShape()
    {
        // Bật vùng va chạm của một BoxCollider
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        occupiedImage.gameObject.SetActive(true);
    }

    public void Unsetoccupied()
    {
        occupiedImage.gameObject.SetActive(false);
    }
}
