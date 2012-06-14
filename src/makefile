include config.inc

BINDIR=../bin
ASSEMBLY=$(BINDIR)/LuaInterface.dll
RUNNER=$(BINDIR)/luai.exe
STUB=$(BINDIR)/luanet.so

all: $(ASSEMBLY) $(STUB) $(RUNNER)

ASSEMBLY_SRC = AssemblyInfo.cs CheckType.cs \
  GenerateEventAssembly.cs Lua.cs LuaDLL.cs LuaException.cs LuaNetRunner.cs \
  Metatables.cs MethodWrapper.cs ObjectTranslator.cs ProxyType.cs

$(ASSEMBLY): $(ASSEMBLY_SRC)
	$(CSC) -out:$(ASSEMBLY) -t:library $(ASSEMBLY_SRC)
	
$(RUNNER): LuaNetRunner.cs
	$(CSC) -out:$(RUNNER) -r:$(ASSEMBLY) LuaNetRunner.cs
	
$(STUB): luastdcall.c
	$(CC) $(SHARED) -o $(STUB) -I $(LUA_INCLUDE) luastdcall.c $(LUA_LIB)
    
clean:
	rm $(STUB) $(ASSEMBLY) $(RUNNER)