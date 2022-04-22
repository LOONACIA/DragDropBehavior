using System.Windows.Controls;

namespace DragDropBehavior.Shapes;

/// <summary>
/// Rectangle.xaml에 대한 상호 작용 논리
/// </summary>
public partial class Rectangle : UserControl, IMyShape
{
	public Rectangle()
	{
		InitializeComponent();
	}
}
