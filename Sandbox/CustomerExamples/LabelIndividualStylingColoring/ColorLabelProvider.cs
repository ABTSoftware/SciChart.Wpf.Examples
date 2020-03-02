using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace LabelIndividualStylingColoring
{
    /*
     * To override label colours in SciChart
     *
     * Use the LabelProvider feature, inherit NumericLabelProvider for NumericAxis, or appropriate label provider for your axis type.
     * Next, use the default FormatLabel functions, or, create your own if you wish to change label formatting.
     *
     * Finally, override CreateDataContext. We need to create a class which inherits DefaultTickLabelViewModel.
     * In this case we use the build in NumericTickLabelViewModel and add some properties to the derived class.
     *
     * The function UpdateDataContext performs base class operations to set properties such as Text and style on the tick label view model.
     * Finally, in the view, we have a TickLabel template that binds to our new ColorNumericLabelViewModel.Foreground property.
     */
    public class ColorNumericLabelViewModel : NumericTickLabelViewModel
    {
        public SolidColorBrush Foreground { get; }
        public ColorGenerator ColorGenerator { get; }

        public ColorNumericLabelViewModel(ColorGenerator colorGenerator)
        {
            ColorGenerator = colorGenerator;
            Foreground = new SolidColorBrush(ColorGenerator.DefaultColor);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName.Equals(nameof(Text)))
            {
                Foreground.Color = double.TryParse(Text, out double value)
                    ? ColorGenerator.GetColor(value)
                    : ColorGenerator.DefaultColor;
            }
        }
    }

    public class ColorLabelProvider : NumericLabelProvider
    {
        private readonly ColorGenerator _colorGenerator;

        public ColorLabelProvider(ColorGenerator colorGenerator)
        {
            _colorGenerator = colorGenerator;
        }

        public override ITickLabelViewModel CreateDataContext(IComparable dataValue)
        {
            // Create our ViewModel which implements ITickLabelViewModel and set additional properties 
            var label = new ColorNumericLabelViewModel(_colorGenerator);

            // use base class method to update the labels default properties 
            base.UpdateDataContext(label, dataValue);

            return label;
        }
    }
}