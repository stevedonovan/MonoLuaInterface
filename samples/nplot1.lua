require "CLRPackage"
require "CLRForm"
import "System"
import "System.Windows.Forms"
import "System.Drawing"
import 'NPlot.dll'
NPW = CLRPackage('NPlot.dll','NPlot.Windows')

function doubles(t)
    return luanet.make_array(Double,t)
end

f = Form()

s = NPW.PlotSurface2D()
s.Dock = DockStyle.Fill
f.Controls:Add(s)

lp = LinePlot()
lp.AbscissaData = doubles{1,2,3,4}
lp.OrdinateData = doubles{10,25,30,27}

s:Add(lp)

f:ShowDialog()

