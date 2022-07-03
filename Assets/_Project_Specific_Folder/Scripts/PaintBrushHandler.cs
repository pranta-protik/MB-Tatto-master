using System.Collections.Generic;
using UnityEngine;

public class PaintBrushHandler : MonoBehaviour
{
    private DokoDemoPainterPen _pen;
    [SerializeField] private List<Color> _penColors;

    private void Start()
    {
        _pen = GetComponent<DokoDemoPainterPen>();
    }

    public void OnColorButtonClick(int colorIndex)
    {
        _pen.color = _penColors[colorIndex];
        _pen.radius = 10f;
        _pen.opacity = 1.0f;
        _pen.smoothTip = false;
        _pen.keepTarget = false;
        _pen.paintInvisible = false;
    }
}
