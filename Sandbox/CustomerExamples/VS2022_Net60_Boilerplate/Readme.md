#VS2022 .NET 6.0 boilerplate 

A very simple example demonstrating SciChart WPF working in .NET 6.0 with VS2022 

This example adds an XAxis, YAxis and single annotation to the chart. 

```
        <!-- Simple example to demonstrate SciChart WPF 6.5 working in VS2022 with .NET 6.0 -->
        <s:SciChartSurface>
            <s:SciChartSurface.XAxis>
                <s:NumericAxis/>
            </s:SciChartSurface.XAxis>
            <s:SciChartSurface.YAxis>
                <s:NumericAxis/>
            </s:SciChartSurface.YAxis>
            <s:SciChartSurface.Annotations>
                <s:TextAnnotation Text="Hello .NET 6.0 World!"
                                  FontFamily="22"
                                  Foreground="Orange"
                                  X1="0.5"
                                  Y1="0.5"
                                  CoordinateMode="Relative"
                                  HorizontalAnchorPoint="Center"
                                  VerticalAnchorPoint="Center"/>
            </s:SciChartSurface.Annotations>
        </s:SciChartSurface>
```

If you experience any problems with .NET 6.0 then modify this example and send 
it back to our tech support at https://support.scichart.com and they will be 
happy to help

