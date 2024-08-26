Class Application
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        ' Set SciChart Runtime Key here       
        SciChart.Charting.Visuals.SciChartSurface.SetRuntimeLicenseKey("SciChart_RuntimeKey_here")
    End Sub
End Class
