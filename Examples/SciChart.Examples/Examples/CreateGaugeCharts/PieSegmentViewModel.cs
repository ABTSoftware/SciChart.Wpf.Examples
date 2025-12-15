using System.Windows.Media;
using SciChart.Charting.Visuals;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateGaugeCharts;

public class BrushViewModel
{
    public Brush Brush { get; set; }
    public string BrushName { get; set; }
}

public class PieSegmentViewModel : BaseViewModel, IPieSegmentViewModel
{
    private double _value;
    private double _percentage;
    private bool _isSelected;
    private string _name;
    private Brush _fill;
    private Brush _stroke;
    private double _strokeThickness;

    public double Value
    {
        get => _value;
        set
        {
            if (_value.Equals(value)) return;

            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public double Percentage
    {
        get => _percentage;
        set
        {
            if (_percentage.Equals(value)) return;

            _percentage = value;
            OnPropertyChanged(nameof(Percentage));
        }
    }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value.Equals(_isSelected)) return;

            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
    public string Name
    {
        get => _name;
        set
        {
            if (ReferenceEquals(_name, value)) return;

            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    public Brush Fill
    {
        get => _fill;
        set
        {
            if (ReferenceEquals(_fill, value)) return;

            _fill = value;
            OnPropertyChanged(nameof(Fill));
        }
    }
    public Brush Stroke
    {
        get => _stroke;
        set
        {
            if (ReferenceEquals(_stroke, value)) return;

            _stroke = value;
            OnPropertyChanged(nameof(Stroke));
        }
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            if (value.Equals(_strokeThickness)) return;

            _strokeThickness = value;
            OnPropertyChanged(nameof(StrokeThickness));
        }
    }
}