include ../src/config.inc

BINDIR=../bin
ASSEMBLY=LuaInterface.dll
PROG=TestLua.exe

all:  $(ASSEMBLY) $(PROG) run

$(PROG):  TestLua.cs Entity.cs
	$(CSC) -r:$(ASSEMBLY) $^

$(ASSEMBLY): $(BINDIR)/$(ASSEMBLY)
	cp $<  $@
	
run:	$(PROG)
	mono $(PROG)
	


