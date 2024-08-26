Imports SciChart.Charting.Model.DataSeries
Imports SciChart.Examples.ExternalDependencies.Data

Class MainWindow
    Private Sub MainWindow_OnLoaded(sender As Object, e As RoutedEventArgs)
        ' Create a DataSeries of type X=double, Y=double
        Dim dataSeries = New UniformXyDataSeries(Of Double)(0D, 0.002)

        lineRenderSeries.DataSeries = dataSeries

        ' Append data to series. SciChart automatically redraws
        dataSeries.Append(DataManager.Instance.GetFourierYData(1.0, 0.1))

        ' Zoom to extents
        SciChartSurface.ZoomExtents()
    End Sub
End Class
