using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2 (0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;
    public SquareTextureData squareTextureData;
    public AudioSource[] popupSound;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private System.Collections.Generic.List<GameObject> _gridSquares = new System.Collections.Generic.List<GameObject>();

    private LineUndicator _lineIndicator;

    private Config.SquareColor currentActiveSquareColor_ = Config.SquareColor.NotSet;

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;
    }

    void Start()
    {
        popupSound = GameObject.FindGameObjectWithTag("PopupSound").GetComponentsInChildren<AudioSource>();

        _lineIndicator = GetComponent<LineUndicator>();
        CreateGrid();
        currentActiveSquareColor_ = squareTextureData.activeSquareTextures[0].squareColor;
    }

    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor_ = color;
    }

    // Tạo bảng
        // Tạo các ô vuông
        // Thiết lập vị trí
    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    // Tạo các ô vuông trên bảng
    private void SpawnGridSquares()
    {
        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count -1].transform.SetParent (this.transform);
                _gridSquares[_gridSquares.Count -1].transform.localScale = new Vector3 (squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }

    // Thiết lập vị trí các ô vuông trên bảng
    private void SetGridSquaresPositions()
    {
        // Biến lưu STT cột, hàng; số ô vuông trống giữa các ô trên bảng;
        // Biến lưu trạng thái ô di chuyển hay chưa 
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2 (0.0f, 0.0f);
        bool row_moved = false;

        // Hình chữ nhật đầu tiên trong bảng -> tính toán kich thước của ô vuông và kc giữa các ô vuông
        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if(column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

            if(column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }
            if(row_number > 0 &&  row_number % 3 == 0 && row_moved == false)
            {
                //still in the same row
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);
            column_number++;
        }
    }

    // Biến và hàm lưu trạng thái chuyển đổi
    public bool change = false;
    public void CanChange()
    {
        change = true;
    }

    // Kiểm tra hình dạng hiện tại có thể đặt trên bảng 9x9 hay không
    private void CheckIfShapeCanBePlaced()
    {
        // Danh sách lưu chỉ số các ô vuông
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            // Ô vuông được chọn và ko bị chiếm dụng
            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; 

        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {

            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor_);
            }

            var shapeLeft = 0;

            foreach (var shape in shapeStorage.shapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else
            {
                GameEvents.SetShapeInActive();
            }

            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }

    // Kiểm tra có đường nào hoàn thiện không
    void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        foreach (var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        for (var row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for (var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }
            lines.Add(data.ToArray());
        }

        // Lấy số đường hoàn thiện dựa trên danh sách lines
        var completedLines = CheckIfSquareIsCompleted(lines);
        
        if (completedLines >= 1)
        {
            GameEvents.ShowCongratulationWritings();
            popupSound[0].Play();
        }

        // Điểm số
        var totalScores = 10 * completedLines;
        GameEvents.AddScores(totalScores);
        CheckIfPlayerLost();
    }

    // Trả về số dòng hoàn thiện dựa trên số ô vuông hoàn thiện
    private int CheckIfSquareIsCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;

        // looping all the data we stack -> check if occipied -> false -> line not completed -> at the end, check the line
        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }

            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            var completed = false;

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }

            // Gỡ trạng thái chiếm đóng
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if (completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;
    }

    // Kiểm tra kết thúc trò chơi
    private void CheckIfPlayerLost()
    {
        var validShapes = 0;

        for (var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if (CheckIfShapeCanBePlaceOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                // ?. prevent from crash by null index
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }

        if (validShapes == 0)
        {
            GameEvents.GameOver(false);
        }
    }

    // Kiểm tra nếu hình có thể đặt lên bảng hay ko
    private bool CheckIfShapeCanBePlaceOnGrid(Shape currentShape)
    {
        // Dữ liệu hình hiện tại, số cột, số hàng
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        // Tạo danh sách lưu các chỉ số ô vuông đã được lấp đầy
        List<int> origincalShapeFillUpSquares = new List<int>();
        var squareIndex = 0;

        // Duyệt các ô vuông của hình hiện tại và thêm các chỉ số ô vuông đã được ấp đầu vào danh sách
        // Ví dụ: hình vuông 2x2 có ô 0,0 và 1,1 được lấp đầu -> trả về danh sách 0,3
        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    origincalShapeFillUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        if (currentShape.TotalSquareNumber != origincalShapeFillUpSquares.Count)
        {
            Debug.LogError("Number of filled up squares are not the same as the original shape have.");
        }

        // Lấy danh sách các ô vuông trên bảng có cùng kích thước hình hiện tại
        // nếu shape là hình vuông 2x2 và bảng là 3x3 -> squareList: [0, 1, 2], [3, 4, 5], [6, 7, 8]
        var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);

        bool canBePlaced = false;

        foreach (var number in squareList)
        {
            // Kiểm tra hình dạng hiện tại có thể được đặt trên ô vuông tại chỉ số number[squareIndexToCheck] hay ko
            bool shapeCanBePlacedOnTheBoard = true;
            foreach (var squareIndexToCheck in origincalShapeFillUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }
 
    // Tạo danh sách các ô vuông
    // Ví dụ nếu input = (3,3) -> tạo danh sách [[0,1,2], [4,5,6], ..., [24,25,26]]
    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (rows - 1) < 9)
        {
            var rowData = new List<int>();

            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if (lastColumnIndex + (columns - 1) >= 9)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }

            safeIndex++;
            if (safeIndex > 100)
            {
                break;
            }
        }

        return squareList;
    }

}
