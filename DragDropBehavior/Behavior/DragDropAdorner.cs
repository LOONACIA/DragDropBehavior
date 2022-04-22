using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DragDropBehavior.Behavior;

public class DragDropAdorner : Adorner
{
    // Shape를 화면에 시각화할 Brush
	private Brush _visualBrush;

    // Shape의 화면 좌표
	private Point _currentLocation;

    // 최초 클릭 좌표
	private Point _startLocation;

    public DragDropAdorner(UIElement adornedElement, Point startLocation) : base(adornedElement)
	{
		_startLocation = startLocation;
		_visualBrush = new VisualBrush(AdornedElement)
		{
			Opacity = 0.75
		};
		IsHitTestVisible = false;
	}

	public void Update(Point location)
	{
		_currentLocation = location;

        (Parent as AdornerLayer)?.Update(AdornedElement);
	}

	protected override void OnRender(DrawingContext dc)
	{
		var p = _currentLocation;
		p.Offset(-_startLocation.X, -_startLocation.Y);

		dc.DrawRectangle(_visualBrush, null, new Rect(p, RenderSize));
	}
}
