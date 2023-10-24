using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    // Các Image trong giao diện khi người chơi tương tác
    public Image hooverImage;
    public Image activeImage;
    public Image normalImage;
    public List<Sprite> normalImages;

    // Chưa set màu sắc cho ô vuông hiện tại
    private Config.SquareColor currentSquareColor_ = Config.SquareColor.NotSet;

    // Trả về màu sắc hiện tại của ô vuông
    public Config.SquareColor GetCurrentColor()
    {
        return currentSquareColor_;
    }

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    /*public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }*/

    // Xử lý khi người chơi đặt hình lên bảng 9x9
    public void PlaceShapeOnBoard(Config.SquareColor color)
    {
        currentSquareColor_ = color;
        ActivateSquare();
    }

    // Set màu cho ô vuông
    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }

    // Hủy kích hoạt ô vuông
    public void Deactivate()
    {
        currentSquareColor_ = Config.SquareColor.NotSet;
        activeImage.gameObject.SetActive(false);
    }

    // Xóa trạng thái hiện tại của ô vuông
    public void ClearOccupied()
    {
        currentSquareColor_ = Config.SquareColor.NotSet;
        Selected = false;
        SquareOccupied = false;
    }

    // Đặt hình ảnh cho ô vuông
    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    // Xử lý sự kiện đi vào vùng collider của ô vuông
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
        
    }

    // Xử lý khi đi ra ngoài vùng collider của ô vuông
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().Unsetoccupied();
        }
    }

    // Xử lý khi đặt đối tượng trong vùng collider của ô vuông
    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;

        if (SquareOccupied == false)
        {
            hooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }


}
