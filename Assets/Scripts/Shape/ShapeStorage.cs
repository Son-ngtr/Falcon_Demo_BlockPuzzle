using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    // Thuộc tính chưas
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;

    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }

    // Trả về hình dạng đang được chọn
    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }

        Debug.LogError("There is no shape selected!");
        return null;
    }

    // Lấy một hình dạng trong danh sách
    private void RequestNewShapes()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }
}
