using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Slider _widthSlider;
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private bool _updateSizeDelta = true;

    private void Awake()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        _widthSlider.minValue = 0;
        _widthSlider.maxValue = screenWidth;
        _widthSlider.value = screenWidth * 0.25f;

        _heightSlider.minValue = 0;
        _heightSlider.maxValue = screenHeight;
        _heightSlider.value = screenHeight * 0.25f;
    }

    private void Update()
    {
        if (_updateSizeDelta)
            _rect.sizeDelta = new Vector2(_widthSlider.value, _heightSlider.value);
    }

    public Vector2Int[] GetArea(int resolution = 100)
    {
        if (resolution > 100)
            resolution = 100;
        if (resolution < 1) 
            resolution = 1;
        
        var sizeDelta = _rect.sizeDelta;
        var bottomLeftPosition = new Vector2(Mathf.Abs(_rect.transform.position.x - sizeDelta.x),
            Mathf.Abs(_rect.transform.position.y - sizeDelta.y));
        
        var upperRightPosition = new Vector2(Mathf.Abs(_rect.transform.position.x + sizeDelta.x),
            Mathf.Abs(_rect.transform.position.y + sizeDelta.y));

        var resultList = new List<Vector2Int>();
        var xCount = (int) Mathf.Abs(bottomLeftPosition.x - upperRightPosition.x);
        var yCount = (int) Mathf.Abs(bottomLeftPosition.y - upperRightPosition.y);
        
        for (int ii = 0; ii < xCount / (100 / resolution); ii++)
            for (int jj = 0; jj < yCount / (100 / resolution); jj++)
                resultList.Add(new Vector2Int(ii * 100 / resolution, jj * 100 / resolution));

        return resultList.ToArray();
    }
 
}