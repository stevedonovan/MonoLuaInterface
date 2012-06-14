LuaInterface is a library for integration between the Lua language and Microsoft .NET platform's Common Language Runtime (CLR).  Lua scripts can use it to instantiate CLR objects, access properties, call methods, and even handle events with Lua functions.

Originally written by Fabio Mascarenhas, and currently maintained by Craig Presti at 
[here](http://code.google.com/p/luainterface)

This is version 1.5.3, which was the last version to use C/Invoke to link dynamically to a native Lua shared library/DLL.

This port provides a working version of LuaInterface, buildable on Mono.

To build, go into the src directory, and edit `makefile` so that it knows where your Lua headers are, and what the name of your C# compiler is. Then just do `make`.

To install, go back to the root and run `./install` which is a Lua script.

It will generate a wrapper script called `luai` looking like this:

    #!/bin/sh
    LUAI=/home/azisa/lua/MonoLuaInterface/bin
    export LD_LIBRARY_PATH=$LUAI
    export LUA_PATH=";;$LUAI/lua/?.lua"
    /usr/bin/mono $LUAI/luai.exe $*

The samples directory contains the original samples, plus some extended ones from the Lua for Windows project.

