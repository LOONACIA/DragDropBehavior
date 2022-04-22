using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DragDropBehavior.Behavior;

public class DragDropBehavior : Behavior<Canvas>
{
	private DragDropAdorner? _adorner;

	public Type InnerType
	{
		get => (Type)GetValue(InnerTypeProperty);
		set => SetValue(InnerTypeProperty, value);
	}

	public static readonly DependencyProperty InnerTypeProperty =
			DependencyProperty.RegisterAttached(nameof(InnerType),
			typeof(Type), typeof(DragDropBehavior), new PropertyMetadata(default(Type)));

	#region Behavior Override
	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
		AssociatedObject.Drop += AssociatedObject_Drop;
		AssociatedObject.GiveFeedback += AssociatedObject_GiveFeedback;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
		AssociatedObject.Drop -= AssociatedObject_Drop;
		AssociatedObject.GiveFeedback -= AssociatedObject_GiveFeedback;
	}
	#endregion

	private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		var control = FindAncestor((DependencyObject)AssociatedObject.InputHitTest(e.GetPosition(AssociatedObject)));

		if (control is null)
			return;

		SetDropEffect(control);

		DataObject data = new DataObject(nameof(AssociatedObject), control, true);
		DragDrop.DoDragDrop(control, data, DragDropEffects.Move);

		UnsetDropEffect(control);
	}

	private void AssociatedObject_Drop(object sender, DragEventArgs e)
	{
		// Drop action //
	}

	private void SetDropEffect(Control control)
	{
		_adorner = new DragDropAdorner(control, GetMousePositionWindowsForms());
		AdornerLayer.GetAdornerLayer(control)?.Add(_adorner);
		Mouse.SetCursor(Cursors.Hand);
	}

	private void UnsetDropEffect(Control control)
	{
		AdornerLayer.GetAdornerLayer(control)?.Remove(_adorner);
		Mouse.SetCursor(null);
		_adorner = null;
	}

	private void AssociatedObject_GiveFeedback(object sender, GiveFeedbackEventArgs e)
	{
		e.UseDefaultCursors = e.Effects != DragDropEffects.Move;
		if (_adorner is not null)
		{
			_adorner.Update(GetMousePositionWindowsForms());
		}
		e.Handled = true;
	}

	private static Point GetMousePositionWindowsForms()
	{
		var point = System.Windows.Forms.Control.MousePosition;
		return new Point(point.X, point.Y);
	}

	private Control? FindAncestor(DependencyObject obj)
	{
		while (obj is not null)
		{
			if (InnerType.Equals(obj.GetType()) || InnerType.IsAssignableFrom(obj.GetType()))
			{
				return (Control)obj;
			}
			obj = VisualTreeHelper.GetParent(obj);
		}

		return null;
	}
}
