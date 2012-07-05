/*
 Demonstrates using the higher-level LuaInterface API
*/

using System;
using LuaInterface;

// adding new Lua functions is particularly easy...
public class MyClass {

    [LuaGlobal]
    public static double Sqr(double x) {
        return x*x;
    }

    [LuaGlobal]
    public static double Sum(double x, double y) {
        return x + y;
    }

    // can put into a particular table with a new name
    [LuaGlobal(Name="utils.sum",Description="sum a table of numbers")]
    public static double SumX(params double[] values) {
        double sum = 0.0;
        for (int i = 0; i < values.Length; i++)
            sum += values[i];
        return sum;
    }

    public static void Register(Lua L) {
        L.NewTable("utils");
        LuaRegistrationHelper.TaggedStaticMethods(L,typeof(MyClass));
    }

}

public class CSharp {
	public virtual string MyMethod(string s) {
		return s.ToUpper();
	}	
	
	public static string UseMe (CSharp obj, string val) {
		return obj.MyMethod(val);	
	}
}

public class CallLua {

    public static void Main(string[] args) {
        Lua L = new Lua();

        object[] res = L.DoString("return 20,'hello'","tmp");
        Console.WriteLine("returned {0} {1}",res[0],res[1]);

        L.DoString("answer = 42");
        Console.WriteLine("answer was {0}",L["answer"]);

        MyClass.Register(L);

        L.DoString(@"
        print(Sqr(4))
        print(Sum(1.2,10))
        -- please note that this isn't true varargs syntax!
        print(utils.sum {1,5,4.2})
        ");

        L.DoString("X = {1,2,3}; Y = {fred='dog',alice='cat'}");

        LuaTable X = (LuaTable)L["X"];
        Console.WriteLine("1st {0} 2nd {1}",X[1],X[2]);
        // (but no Length defined: an oversight?)
        Console.WriteLine("X[4] was nil {0}",X[4] == null);

        // only do this if the table only has string keys
        LuaTable Y = (LuaTable)L["Y"];
        foreach (string s in Y.Keys)
            Console.WriteLine("{0}={1}",s,Y[s]);

        // getting and calling functions by name
        LuaFunction f = L.GetFunction("string.gsub");
        object[] ans = f.Call("here is the dog's dog","dog","cat");
        Console.WriteLine("results '{0}' {1}",ans[0],ans[1]);

        // and it's entirely possible to do these things from Lua...
        L["L"] = L;
        L.DoString(@"
            L:DoString 'print(1,2,3)'
        ");
		
		// it is also possible to override a CLR class in Lua using luanet.make_object.
		// This defines a proxy object which will successfully fool any C# code 
		// receiving it.
		 object[] R = L.DoString(@"
			luanet.load_assembly 'CallLua'  -- load this program
			local CSharp = luanet.import_type 'CSharp'
			local T = {}
			function T:MyMethod(s) 
				return s:lower()
            end
			luanet.make_object(T,'CSharp')
			print(CSharp.UseMe(T,'CoOl'))
			return T
		");
		// but it's still a table, and there's no way to cast it to CSharp from here...
		Console.WriteLine("type of returned value {0}",R[0].GetType());


    }

}

