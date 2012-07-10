LuaInterface is a library for integration between the Lua language and Microsoft
.NET platform's Common Language Runtime (CLR).  Lua scripts can use it to
instantiate CLR objects, access properties, call methods, and even handle
events with Lua functions.

Originally written by Fabio Mascarenhas, and currently maintained by Craig
Presti at
[here](http://code.google.com/p/luainterface)

This corresponds to the latest version 2.0.3, backported to use P/Invoke against
standard Lua 5.1 shared libraries. It provides a working version of LuaInterface
buildable on Mono.

On Debian/Ubuntu, you will need the `liblua5.1-dev` and `mono-devel` packages.

To build, go into the src directory, and:

    $ ./configure

This requires a Lua installation to run, but no other dependencies. It will look
for the Lua headers in the usual places, `/usr/include` and `/usr/include/lua51`,
if your Lua directory is somewhere else altogether set LUA_INCLUDE:

    $ ./configure LUA_INCLUDE=/home/you/lua-5.1.5/src DEFINES=lua

`DEFINES` here is overriding the default on Linux, which is to assume
the Lua shared library looks like `liblua5.1.so` rather than `lua51.so`.

Configuration of LuaInterface is controlled by two C# preprocessor defines,
`__Windows__` and `__liblua__`.  The first makes the shared library extension '.dll',
and the second makes the Lua shared library name 'liblua5.1' rather than 'lua51'.
The latter is the default for Linux, at least for Debian/Ubuntu. Look at
`src/LuaDLL.cs` to see how these are used, and how to modify for your
configuration.

(Currently, this project builds against Lua 5.1 or LuaJIT.)

    $ make
    $ ./

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
also has the original C# tests. (These all pass, except for passing a managed function
to `string.gsub`, which is a Lua limitation.)

A basic C# program is here; it evaluates Lua expressions:

```C#
using System;
using LuaInterface;

public class TestLua {

  public static void Main(string[] args) {
    if (args.Length == 0) {
       Console.WriteLine("provide a Lua expression!");
    } else {
       Lua L = new Lua();  // will open all the standard Lua libraries
       try {
         object[] results = L.DoString("return "+args[0]);
         Console.WriteLine("answer is {0}",results[0]);
       } catch(Exception e) {
         Console.WriteLine("error: {0}",e.Message);
       }
       L.Close();
    }
  }

}
```

It must be compiled with a reference to LuaInterface.dll, and both luanet.so and liblua5.1.so must
be accessible on the library path.

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
not exist. (Looking up fields and indices directly will fail with an error).

''luanet.make_object''

This takes a table and a CLR class name and allows you to override any virtual methods
of that class.

For instance, given this C# class:

```C#
public class CSharp {
	public virtual string MyMethod(string s) {
		return s.ToUpper();
	}

	public static string UseMe (CSharp obj, string val) {
		return obj.MyMethod(val);
	}
}
```

then the following Lua code creates a proxy object where `MyMethod` is overriden:

```Lua
luanet.load_assembly 'CallLua'  -- load the assembly containing CSharp
local CSharp = luanet.import_type 'CSharp'
local T = {}
function T:MyMethod(s)
    return s:lower()
end
luanet.make_object(T,'CSharp')
print(CSharp.UseMe(T,'CoOl'))
```

There is a corresponding ``luanet.free_object`` for explicit disposal.

(See tests/CallLua.cs)

In addition, this version of LuaInterface defines two extra functions

''luanet.ctype''

This is the equivalent of `typeof` in C#; given a class proxy object, return the
actual CLR type.

```Lua
samples $ luai lua.lua
Lua 5.1.4  Copyright (C) 1994-2008 Lua.org, PUC-Rio
lua.lua (c) David Manura, 2008-08
> = String
ProxyType(System.String): 54267293
> ctype = luanet.ctype
> = ctype(String)
System.String: 2033388324

```

''luanet.enum''

This has two forms. The first casts an integer into an enum type:

```Lua
> enum = luanet.enum
> import 'System.Reflection'
> = BindingFlags.Static
Static: 8
> = enum(BindingFlags,8)
Static: 8
```

The second form parses a string representation for an enumeration type.
This is useful for enum flags:

```Lua
> = enum(BindingFlags,'Static,Public')
Static, Public: 24
```

It's now possible to use CLR reflection in a non-clumsy way; see `tests/ctype.lua`
for an example of using the Lua API directly from Lua itself, by importing all
static methods of the `LuaDLL` class.

## CLRPackage

This Lua module provides some very useful shortcuts. We have already seen `import`,
which brings classes into the global Lua table. This is not appropriate for larger
applications, so there is `luanet.namespace`. Note that its argument may be a table:

```Lua
local gtk,gdk = luanet.namespace {'Gtk','Gdk'}
```

(You do have to explicitly load the assemblies before using this)

''luanet.make_array''

This is a convenience function for creating CLR arrays; it is passed a class (the proxy,
not the type) and a table of values.

Note that the Lua expression `Class[10]` already makes us a `Class[]` array!

Bear in mind that CLR arrays index from zero, and throw a range error if the index
is out of bounds.

''luanet.each''

This constructs a Lua iterator from an IEnumerable interface:

```Lua
> dd = make_array(Double,{1,2,10})
> for x in each(dd) do print(x) end
1
2
10
> import 'System.Collections'
> al = ArrayList()
> al:Add(10)
> al:Add('hello')
> for o in each(al) do print(o) end
10
hello
> ht = Hashtable()
> ht.one = 1
> ht.two = 2
> for p in each(ht) do print(p.Key,p.value) end
one   1
two   2

```






