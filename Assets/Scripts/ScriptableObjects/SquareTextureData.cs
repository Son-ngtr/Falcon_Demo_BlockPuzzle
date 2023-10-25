using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[CreateAssetMenu]
[System.Serializable]*/

public class SquareTextureData : ScriptableObject
{
    [System.Serializable]

    // Structure + texture hiển thị ảnh + màu sắc 
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }

    // Các thuộc tính: giá trị ngưỡng, giá trị ngưỡng cập nhật màu, kết cấu màu
    public int tresholdVal = 0;
    private const int StartTresholdVal = 30;
    public List<TextureData> activeSquareTextures;

    public Config.SquareColor currentColor;
    private Config.SquareColor _nextColor;

    // Trả về màu sắc hiện tại của ô vuông đang hoạt động
    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;

        for (int index = 0; index < activeSquareTextures.Count; index++)
        {
            if (activeSquareTextures[index].squareColor == currentColor)
            {
                currentIndex = index;
            }
        }

        return currentIndex;
    }

    // Cập nhật màu sắc ô vuông dựa trên số điểm hiện tại
    public void UpdateColors(int current_score)
    {
        currentColor = _nextColor;
        var currentColorIndex = GetCurrentColorIndex();

        // Trường hợp màu sắc là màu cuối -> cập nhật màu về đầu
        if (currentColorIndex == activeSquareTextures.Count - 1)
        {
            _nextColor = activeSquareTextures[0].squareColor;
        }
        // Tăng chỉ số màu thêm 1 đơn vị
        else
        {
            _nextColor = activeSquareTextures[currentColorIndex + 1].squareColor;
        }
        tresholdVal = StartTresholdVal + current_score;
    }

    // Hàm cập nhật màu khi bắt đầu
    public void SetStartColor()
    {
        tresholdVal = StartTresholdVal;
        currentColor = activeSquareTextures[0].squareColor;
        _nextColor = activeSquareTextures[1].squareColor;
    }

    private void Awake()
    {
        SetStartColor();
    }

    private void OnEnable()
    {
        SetStartColor();
    }
}
