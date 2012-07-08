include ../src/config.inc

BINDIR=../bin
LDPATH=LD_LIBRARY_PATH=$(BINDIR)
ASSEMBLY=LuaInterface.dll
TEST=TestLua.exe
EX=CallLua.exe

all:  $(ASSEMBLY) $(TEST) $(EX) run

$(TEST):  TestLua.cs Entity.cs
	$(CSC) -r:$(ASSEMBLY) $^

$(EX):  CallLua.cs
	$(CSC) -r:$(ASSEMBLY) $^

$(ASSEMBLY): $(BINDIR)/$(ASSEMBLY)
	cp $<  $@

# to run a CLR program with LuaInterface, then
# 1. LuaInterface.dll must be in same dir, or be in the GAC
# 2. luanet.so and (liblua5.1.so or lua51.so) must be on the shared library path.
run:	$(TEST)
	$(LDPATH) mono $(EX)
	$(LDPATH) mono $(TEST)



