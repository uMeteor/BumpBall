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
//        luaEnv = new LuaEnv();//����lua ���� ���ȫ��Ψһ
//        luaEnv.DoString("print('������xlua')");//ִ��lua ֻ����Resourse�ļ��¶�ȡ
//        luaEnv.DoString("CS.UnityEngine.Debug.Log('������C#')");//ͨ��xluaִ��C#����
//        // luaEnv.DoString("require 'Lua'");

//        //�Զ���·��
//        luaEnv.AddLoader((ref string luaName) => {

//            string path = Application.dataPath + "/StreamingAssets/TestLuaFile/" + luaName + ".lua.txt";
//            if (File.Exists(path))
//            {
//                return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(path));//��ȡlua�ļ� �����ı�ת��Ϊ��������return
//            }
//            else
//            {
//                Debug.Log("·�������ڣ�" + path);
//                return null;
//            }

//        });
//        luaEnv.DoString("require 'Lua'");
//        //��Lua�ļ��з��ʱ���
//        int a = luaEnv.Global.Get<int>("z");
//        Debug.Log(a);
//        //string b = luaEnv.Global.Get<string>("b");
//        //bool c = luaEnv.Global.Get<bool>("c");

//        //��ȡlua�����table�ļ�
//       // LuaCapy luaCapy = luaEnv.Global.Get<LuaCapy>("s");//��class��struct����ֻ�ǿ��������߲���������ϵ
//        //Debug.Log(luaCapy.a + "|" + luaCapy.b);
//       // ILuaCapy iLuaCapy = luaEnv.Global.Get<ILuaCapy>("s");//��class��struct����ֻ�ǿ��������߲���������ϵ
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
