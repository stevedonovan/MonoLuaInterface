using Gtk;
using Gdk;
using System;

public interface ExTextViewFilter {
    bool Filter(Gdk.Key key);
}

public class ExTextView : TextView {
    
    public ExTextViewFilter Filter;
    
    protected override bool OnKeyPressEvent(EventKey evnt) {
        if (! Filter.Filter(evnt.Key))
            return base.OnKeyPressEvent(evnt);
        else
            return true;
    }
    
}