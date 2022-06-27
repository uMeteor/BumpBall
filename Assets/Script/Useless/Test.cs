//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using XLua;

//public class Test : MonoBehaviour
//{
//    private LuaEnv luaEnv;

//    private void Start()
//    {
//        luaEnv = new LuaEnv();//创建lua 环境 最好全局唯一
//        luaEnv.DoString("print('这里是xlua')");//执行lua 只能在Resourse文件下读取
//        luaEnv.DoString("CS.UnityEngine.Debug.Log('这里是C#')");//通过xlua执行C#程序
//        // luaEnv.DoString("require 'Lua'");

//        //自定义路径
//        luaEnv.AddLoader((ref string luaName) => {

//            string path = Application.dataPath + "/StreamingAssets/TestLuaFile/" + luaName + ".lua.txt";
//            if (File.Exists(path))
//            {
//                return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));//读取lua文件 将其文本转换为二进制流return
//            }
//            else
//            {
//                Debug.Log("路径不存在：" + path);
//                return null;
//            }

//        });
//        luaEnv.DoString("require 'Lua'");
//        //从Lua文件中访问变量
//        int a = luaEnv.Global.Get<int>("z");
//        Debug.Log(a);
//        //string b = luaEnv.Global.Get<string>("b");
//        //bool c = luaEnv.Global.Get<bool>("c");

//        //读取lua里面的table文件
//       // LuaCapy luaCapy = luaEnv.Global.Get<LuaCapy>("s");//用class或struct接受只是拷贝，两者并无依赖关系
//        //Debug.Log(luaCapy.a + "|" + luaCapy.b);
//       // ILuaCapy iLuaCapy = luaEnv.Global.Get<ILuaCapy>("s");//用class或struct接受只是拷贝，两者并无依赖关系
//      //  Debug.Log(iLuaCapy.a + "|" + iLuaCapy.b);
//        //iLuaCapy.a = 13;
//        //iLuaCapy.b = "miss";
//        //Debug.Log(luaEnv.Global.Get<int>("Sorce.c"));

//    }



//    private void OnDestroy()
//    {
//        luaEnv.Dispose();
//    }

//}
//public class LuaCapy
//{
//   public int a;
//   public string b;


//}

//[CSharpCallLua]
//public interface ILuaCapy
//{
//    public int a{ get; set; }
//    public string b { get; set; }
//}
