using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared {
  public static class Extensions {
    /// <summary>
    /// Print any object using the system json
    /// NOTE: this might get caught in infinite loops (look how to fix...)
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pretty">print multi line json</param>
    /// <returns></returns>
    public static string ToJson(this object self, bool pretty = false) {
      return JsonSerializer.Serialize(self, new JsonSerializerOptions { WriteIndented = pretty, ReferenceHandler = ReferenceHandler.Preserve });
    }

    /// <summary>
    /// print file location and object pretty printed
    /// 
    /// https://www.hanselman.com/blog/GettingTheLineNumberAndFileNameFromC.aspx
    /// </summary>
    /// <param name="self">extended object</param>
    /// <param name="pretty">Multi line json</param>
    public static void Dbg(this object self, bool pretty = false) {
      var CallStack = new StackFrame(1, true);
      var fileName = CallStack.GetFileName();
      var lineNum = CallStack.GetFileLineNumber();
      var type = self.GetType();
      Console.WriteLine($"\nDBG: {fileName}:{lineNum} : {type} - {self.ToJson(pretty)}\n");
    }
  }
}
