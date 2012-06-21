include config.inc

BINDIR=../bin
ASSEMBLY=$(BINDIR)/LuaInterface.dll
RUNNER=$(BINDIR)/luai.exe
STUB=$(BINDIR)/luanet.so

all: $(ASSEMBLY) $(STUB) $(RUNNER)

ASSEMBLY_SRC = \
AssemblyInfo.cs LuaException.cs LuaTable.cs \
CheckType.cs  LuaFunction.cs  LuaUserData.cs \
GenerateEventAssembly.cs  LuaGlobalAttribute.cs   Metatables.cs \
LuaBase.cs   LuaHideAttribute.cs  MethodWrapper.cs \
Lua.cs  LuaRegistrationHelper.cs  ObjectTranslator.cs \
LuaDLL.cs  LuaScriptException.cs  ProxyType.cs

$(ASSEMBLY): $(ASSEMBLY_SRC)
	$(CSC) -out:$(ASSEMBLY) $(DEBUG) -t:library $(ASSEMBLY_SRC)

$(RUNNER): LuaNetRunner.cs
	$(CSC) -out:$(RUNNER) $(DEBUG) -r:$(ASSEMBLY) LuaNetRunner.cs

$(STUB): luastdcall.c
	$(CC) $(SHARED) -o $(STUB) $(DEBUG)  -I $(LUA_INCLUDE) luastdcall.c $(LUA_LIB)

clean:
	rm $(STUB) $(ASSEMBLY) $(RUNNER)
