require "CLRPackage"
require "CLRForm"
import "System.Windows.Forms"
import "System.Drawing"
--import 'Nplot.Windows'
import 'NPlot.dll'
NPW = CLRPackage('NPlot.dll','NPlot.Windows')

f = Form()

s = NPW.PlotSurface2D()
s.Dock = DockStyle.Fill
f.Controls:Add(s)

lp = LinePlot()
lp.AbscissaData = make_array(double,{1,2,3})
lp.OrdinateData = make_array(double,{10,25,30})

s:Add(lp)

f:ShowDialog()

