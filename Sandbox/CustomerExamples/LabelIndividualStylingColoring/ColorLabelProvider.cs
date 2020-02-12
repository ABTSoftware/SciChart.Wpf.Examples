using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace LabelIndividualStylingColoring
{
    /*
     * To override label colours in SciChart
     *
     * Use the LabelProvider feature, inherit NumericLabelProvider for NumericAxis, or appropriate label provider for your axis type
     *
     * Next, use the default FormatLabel functions, or, create your own if you wish to change label formatting
     *
     * Finally, override CreateDataContext. We need to create a class which inherits DefaultTickLabelViewModel. In this case we use the build in NumericTickLabelViewModel
     * and add some properties to the derived class
     *
     * The function UpdateDataContext performs base class operations to set properties such as Text and style on the tick label view model.
     *
     * Finally, in the view, we have a TickLabel template that binds to our new viewmodel.Foreground property 
     */
    public class ColorNumericLabelViewModel : NumericTickLabelViewModel
    {
        public SolidColorBrush Foreground { get; set; }
    }

    public class ColorLabelProvider : NumericLabelProvider
    {
        private readonly ColorGenerator _colorGenerator;

        public ColorLabelProvider(ColorGenerator colorGenerator)
        {
            _colorGenerator = colorGenerator;
        }
        public override string FormatLabel(IComparable dataValue)
        {
            return base.FormatLabel(dataValue);
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            return base.FormatCursorLabel(dataValue);
        }

        public override ITickLabelViewModel CreateDataContext(IComparable dataValue)
        {
            // Create our ViewModel which implements ITickLabelViewModel and set additional properties 
            var label = new ColorNumericLabelViewModel()
            {
                Foreground = new SolidColorBrush(_colorGenerator.GetColor(dataValue))
            };
            // use base class method to update the labels default properties 
            base.UpdateDataContext(label, dataValue);
            return label;
        }
    }
}