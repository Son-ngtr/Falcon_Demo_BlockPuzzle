using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUndicator : MonoBehaviour
{
    // Mảng 2 chiều lưu chỉ số các dòng
    public int[,] line_data = new int[9, 9]
    {
        { 0,  1,  2,    3,  4,  5,    6,  7,  8 },
        { 9, 10, 11,   12, 13, 14,   15, 16, 17 },
        {18, 19, 20,   21, 22, 23,   24, 25, 26 },

        {27, 28, 29,   30, 31, 32,   33, 34, 35 },
        {36, 37, 38,   39, 40, 41,   42, 43, 44 },
        {45, 46, 47,   48, 49, 50,   51, 52, 53 },

        {54, 55, 56,   57, 58, 59,   60, 61, 62 },
        {63, 64, 65,   66, 67, 68,   69, 70, 71 },
        {72, 73, 74,   75, 76, 77,   78, 79, 80 }
    };

    // Mảng 2 chiều lưu chỉ số các ô vuông
    public int[,] square_data = new int[9, 9]
    {
        { 0,  1,  2,    9, 10, 11,   18, 19, 20 },
        { 3,  4,  5,   12, 13, 14,   21, 22, 23 },
        { 6,  7,  8,   15, 16, 17,   24, 25, 26 },

        {27, 28, 29,   36, 37, 38,   45, 46, 47 },
        {30, 31, 32,   39, 40, 41,   48, 49, 50 },
        {33, 34, 35,   42, 43, 44,   51, 52, 53 },

        {54, 55, 56,   63, 64, 65,   72, 73, 74 },
        {57, 58, 59,   66, 67, 68,   75, 76, 77 },
        {60, 61, 62,   69, 70, 71,   78, 79, 80 }
    };

    // Ẩn khỏi giao diện chỉnh sửa của Unity
    [HideInInspector]
    // Mảng 1 chiều lưu chỉ số các cột
    public int[] columnIndexes = new int[9]
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8
    };

    // Trả về vị trí ô vuông (row, col) có chỉ số square_index
    private (int, int) GetSquarePosition(int squareIndex)
    {
        int pos_row = -1;
        int pos_col = -1;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (line_data[row, col] == squareIndex)
                {
                    pos_row = row;
                    pos_col = col;
                }
            }
        }
        return (pos_row, pos_col);
    }

    // Trả về dòng dọc đi qua ô vuông có chỉ số squareIndex
    // Ví dụ: nếu index = 30 thì trả về dòng 1, nếu 41 thì dòng 6
    public int[] GetVerticalLine(int squareIndex)
    {
        int[] line = new int[9];

        var square_position_col = GetSquarePosition(squareIndex).Item2;

        for (int index = 0; index < 9; index++)
        {
            line[index] = line_data[index, square_position_col];
        }

        return line;
    }

    // Trả về chỉ số của ô vuông có chỉ số square trong lưới vuông 3x3
    public int GetGridSquareIndex(int square)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (square_data[row, col] == square)
                {
                    return row;
                }
            }
        }
        return -1;
    }
}
