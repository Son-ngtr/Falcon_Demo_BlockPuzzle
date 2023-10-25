using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerDownHandler
{

    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2 (0f, 200f);

    [HideInInspector]
    public ShapeData CurrentShapeData;

    public int TotalSquareNumber {  get; set; }

    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    // private bool _shapeDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;

    // Khởi tạo các thuộc tính: kích thước, RectTransform, canvas, vị trí, trạng thái hoạt động
    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        // _shapeDraggable = true;
        _startPosition = transform.localPosition;
        _shapeActive = true;
    }

    // Hàm gọi khi đối tượng được bật/ tắt. Trong đó sẽ đăng ký/ hủy đăng ký sự kiện
    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInActive += SetShapeInActive;
    }
    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInActive -= SetShapeInActive;
    }

    // Kiểm tra hình ở vị trí bắt đầu với vị trí hiện tại
    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    // Kiểm tra nếu bất kỳ ô vuông trong hình đang hoạt động
    public bool IsAnyOfShapeSquareActive()
    {
        foreach (var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    // Hàm vô hiệu hóa hình nếu + hình ko ở vị trí bắt đầu + có ô vuông hoạt động
    private void SetShapeInActive()
    {
        if (IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
            foreach (var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        }
    }

    // Kích hoạt hình 
    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        _shapeActive = true;
        
    }

    // Vô hiệu hóa hình
    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
        }
        _shapeActive = false;
    }

    // Yêu cầu hình mới
    public void RequestNewShape(ShapeData shapeData)
    {
        // Đặt vị trí hình về vị trí ban đầu
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    // Tạo hình dạng mới dựa trên shapeData có sẵn
    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);

        // Kiểm tra danh sách hình có đủ để tạo hay không
        while (_currentShape.Count < TotalSquareNumber)
        {
            // Thêm hình vào danh sách, yêu cầu khởi tạo
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        // Vô hiệu hóa các ô vuông hiện tại, đặt mọi thứ về 0
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        // Đặt vị trí để tạo hình dạng cuối cùng
        for(var row = 0; row < shapeData.rows; row++)
        {
            for(var col = 0; col < shapeData.columns; col++)
            {
                // Kiêm tra hình hoạt động hay ko
                if (shapeData.board[row].column[col])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = 
                        new Vector2(GetXPositionForShapeSquare(shapeData, col, moveDistance),
                        GetYPositionForShapeSquare(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
            }
        }
    }

    // Tính toán vị trí Y của một ô vuông
    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;

        if(shapeData.rows > 1)
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;
                if (row < middleSquareIndex) // move it on minus
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex) // move it on plus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 1);
                var multiplier = shapeData.rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                    {
                        shiftOnY = moveDistance.y / 2 * -1;
                    }
                    if (row == middleSquareIndex1)
                    {
                        shiftOnY = (moveDistance.y / 2);
                    }
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2) // move it on negative
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2) // move it on plus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
        }

        return shiftOnY;
    }

    // Tính toán vị trí X
    // Kiểm tra hình có nhiều hơn 1 cột hay không
        // Có -> Tính toán chỉ số của ô vuông trong hình
    // Kiểm tra xem ô vuông hiện tại ở trước hay sau ô trung tâm
        // Trước -> di chuyển ô vuông về bên trái
        // Sau -> di chuyển ô vuông sang bên phải
    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.columns > 1) //Vertical posiotion calculation
        {
            if (shapeData.columns % 2 != 0) 
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if(column < middleSquareIndex) // move it on the negative
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column > middleSquareIndex) // move it on plus
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                var multiplier = shapeData.columns / 2;

                if(column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if(column == middleSquareIndex2)
                    {
                        shiftOnX = moveDistance.x / 2; 
                    }
                    if(column == middleSquareIndex1)
                    {
                        shiftOnX = (moveDistance.x / 2) * -1;
                    }
                }

                if(column < middleSquareIndex1 && column < middleSquareIndex2) // move it on negative
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column < middleSquareIndex1 && column > middleSquareIndex2) // move it on plus
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
                
            }
        }

        return shiftOnX;
    }

    // Trả về số ô vuông hoạt động
    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                {
                    number++;
                }
            }
        }
        return number;
    }


    // Xử lý sự kiện tương tác hình với bảng 9x9
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Thực thi khi chọn hình hoặc bỏ
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, 
            eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanBePlaced();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }

}
