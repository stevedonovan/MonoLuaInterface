LuaInterface is a library for integration between the Lua language and Microsoft
.NET platform's Common Language Runtime (CLR).  Lua scripts can use it to
instantiate CLR objects, access properties, call methods, and even handle
events with Lua functions.

Originally written by Fabio Mascarenhas, and currently maintained by Craig
Presti at
[here](http://code.google.com/p/luainterface)

This is version 1.5.3, which was the last version to use C/Invoke to link
dynamically to a native Lua shared library/DLL.

This port provides a working version of LuaInterface, buildable on Mono.

On Debian/Ubuntu, you will need the `liblua5.1-dev` and `mono-devel` packages.

To build, go into the src directory, and:

    $ ./configure

This requires a Lua installation to run, but no other dependencies. It will look
for the Lua headers in the usual places, `/usr/include` and `/usr/include/lua51`,
if your Lua directory is somewhere else altogether set LUA_INCLUDE:

    $ ./configure LUA_INCLUDE=/home/you/lua-5.1.5/src DEFINES=lua

`DEFINES` here is overriding the default on Linux, which is to assume
the Lua shared library looks like `liblua5.1.so` rather than `lua51.so`.

(Currently, this project builds against Lua 5.1 or LuaJIT.)

    $ make
    $ ,.install

Last step assumes you have a `~/bin` directory, but you can install globally with

    $ sudo ./install /usr/local/bin

(You can also install globally with e.g `sudo ./install /usr/local/bin`)

It will generate a wrapper script called `luai` looking like this:

    #!/bin/sh
    LUAI=/home/azisa/lua/MonoLuaInterface/bin
    export LD_LIBRARY_PATH=$LUAI
    export LUA_PATH=";;$LUAI/lua/?.lua"
    /usr/bin/mono $LUAI/luai.exe $*

We have to locally mess with `LD_LIBRARY_PATH` (or `DYLD_LIBRARY_PATH` on
OS X) because LuaInterface will need to find both the Lua shared library and the
stub library `luanet.so`.

The samples directory contains the original samples, plus some extended ones
from the Lua for Windows project.

Here is the proverbial 'Hello World':

```lua
-- hello.lua
luanet.load_assembly "System"
local Console = luanet.import_type "System.Console"
local Math = luanet.import_type "System.Math"

Console.WriteLine("sqrt(2) is {0}",Math.Sqrt(2))
```

Using the `CLRPackage` utilities, it is even simpler, since individual classes will be
loaded as needed:

```lua
-- hello2.lua
require 'CLRPackage'
import "System"
Console.WriteLine("sqrt(2) is {0}",Math.Sqrt(2))
```

If you want an interactive prompt, then there is a Lua interpreter in Lua, called
`lua.lua`, in the samples directory:

```
samples$ luai lua.lua
Lua 5.1.4  Copyright (C) 1994-2008 Lua.org, PUC-Rio
lua.lua (c) David Manura, 2008-08
> require 'CLRPackage'
> Console.WriteLine("hello from Mono")  -- 'System' is already loaded...
hello from Mono
>
```

It is straightforward to write GTK# applications in Lua - note that here the
`import` call is passed the package name and the namespace, in cases where
they are not the same:

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

This automatically creates globals, which is the only way that Lua can mimick the
usual C# scope resolution rules. For larger programs, this is probably not a
good idea, and so there's a way of explicitly creating namespaces:


```Lua
-- hello-gtk2.lua
require 'CLRPackage'
luanet.load_assembly 'gtk-sharp'
local gtk = luanet.namespace 'Gtk'

gtk.Application.Init()

local win = gtk.Window("Hello from GTK#")

win.DeleteEvent:Add(function()
    gtk.Application.Quit()
end)

win:Resize(300,300)

local label = gtk.Label()
label.Text = "Hello World!"
win:Add(label)

win:ShowAll()

gtk.Application.Run()

```

## LuaInterface from C#

LuaInterface is usually used by CLR applications which need a scripting language.
An example of the high-level interface with Lua is `tests/CallLua.cs`; this directory
also has the original C# tests. These all pass, except for passing a managed function
to `string.gsub`, which is a Lua limitation.

## Lua API

These are contained in the global table `luanet`:

''luanet.load_assembly''

Loads a CLR assembly; will throw an error if not found.

''luanet.import_type''

Bring in a class using the fully qualified name, e.g.
`C = luanet.import_type 'System.Console'`.  The assembly must have been previously
loaded.

''luanet.get_object_member''

This is given an object, and an index (string or integer). It can be used to look
up a property value, and will return nil + error message if the property does
not exist.

