LuaInterface is a library for integration between the Lua language and Microsoft .NET platform's Common Language Runtime (CLR).  Lua scripts can use it to instantiate CLR objects, access properties, call methods, and even handle events with Lua functions.

Originally written by Fabio Mascarenhas, and currently maintained by Craig Presti at 
[here](http://code.google.com/p/luainterface)

This is version 1.5.3, which was the last version to use C/Invoke to link dynamically to a native Lua shared library/DLL.

This port provides a working version of LuaInterface, buildable on Mono.

On Debian/Ubuntu, you will need the `liblua5.1-dev` and `mono-devel` packages.

To build, go into the src directory, and edit `makefile` so that it knows where your Lua headers are, and what the name of your C# compiler is. Then just do `make`.

To install, go back to the root and run `./install` which is a Lua script.

(You can also install globally with `sudo ./install /usr/local/bin`)

It will generate a wrapper script called `luai` looking like this:

    #!/bin/sh
    LUAI=/home/azisa/lua/MonoLuaInterface/bin
    export LD_LIBRARY_PATH=$LUAI
    export LUA_PATH=";;$LUAI/lua/?.lua"
    /usr/bin/mono $LUAI/luai.exe $*

The samples directory contains the original samples, plus some extended ones from the Lua for Windows project.

Here is the proverbial 'Hello World':

```lua
-- hello.lua
luanet.load_assembly "System"
local Console = luanet.import_type "System.Console"
local Math = luanet.import_type "System.Math"

Console.WriteLine("sqrt(2) is {0}",Math.Sqrt(2))
```

Using the `CLRPackage` utilities, it is even simpler, since individual classes will be loaded as needed:

```lua
-- hello2.lua
require 'CLRPackage'
import "System"
Console.WriteLine("sqrt(2) is {0}",Math.Sqrt(2))
```

If you want an interactive prompt, then there is a Lua interpreter in Lua, called `lua.lua`, in the samples directory:

```
samples$ luai lua.lua
Lua 5.1.4  Copyright (C) 1994-2008 Lua.org, PUC-Rio
lua.lua (c) David Manura, 2008-08
> require 'CLRPackage'
> Console.WriteLine("hello from Mono")  -- 'System' is already loaded...
hello from Mono
> 
```

It is straightforward to write GTK# applications in Lua - note that here the `import` call is passed the package name and the namespace, in cases where they are not the same:

```Lua
-- hello-gtk.lua
require 'CLRPackage'
import ('gtk-sharp','Gtk')

Application.Init()

local win = Window("Hello from GTK#")

win.DeleteEvent:Add(function()
    Application.Quit()
end)

win:Resize(300,300)

local label = Label()
label.Text = "Hello World!"
win:Add(label)

win:ShowAll()

Application.Run()

```

