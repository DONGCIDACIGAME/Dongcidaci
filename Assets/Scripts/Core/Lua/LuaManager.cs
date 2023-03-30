//using XLua;

//namespace GameEngine
//{
//    public class LuaManager : ModuleManager<LuaManager>
//    {
//        LuaEnv mLuaEnv;

//        /// <summary>
//        /// lua 文件相对路径 -> 绝对路径
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <returns></returns>
//        private string GetLuaFileFullPath(string filePath)
//        {
//            return string.Format("{0}/{1}.lua", PathDefine.LUA_DIR_PATH, filePath);
//        }

//        /// <summary>
//        /// custom lua loader
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <returns></returns>
//        private byte[] CustomLuaLoader(ref string filePath)
//        {
//            if (filePath.Equals("LuaEntry"))
//            {
//                return null;
//            }

//            string fullPath = GetLuaFileFullPath(filePath);
//            if (!FileHelper.FileExist(fullPath))
//            {
//                Log.Error(LogLevel.Critical, "Custom Load Lua File Failed,filePath:{0}", fullPath);
//                return null;
//            }

//            Log.Logic(LogLevel.Info, "lua file full path:{0}", fullPath);

//            return FileHelper.ReadAllBytes(fullPath);
//        }

//        public void CreateLuaEnv()
//        {
//            mLuaEnv = new LuaEnv();

//            // 添加自定义loader
//            mLuaEnv.AddLoader(CustomLuaLoader);

//            // lua 文件入口
//            mLuaEnv.DoString("require('LuaEntry')");
//        }

//        /// <summary>
//        /// 加载单个lua脚本
//        /// </summary>
//        /// <param name="luaFile">lua文件在luaScripts文件夹下的相对路径</param>
//        private void LoadLuaScript(string luaFile)
//        {
//            mLuaEnv.DoString(string.Format("require('{0}')", luaFile));
//        }

//        public void DoString(string s)
//        {
//            if (mLuaEnv != null)
//            {
//                mLuaEnv.DoString(s);
//            }
//            else
//            {
//                Log.Error(LogLevel.Fatal, "LuaManager mLuaEnv is null!");
//            }
//        }

//        public override void OnUpdate(float deltaTime)
//        {

//        }

//        public override void OnLateUpdate(float deltaTime)
//        {

//        }

//        public void DisposeLuaEnv()
//        {
//            if (mLuaEnv != null)
//            {
//                mLuaEnv.Dispose();
//            }
//        }

//        public override void Initialize()
//        {

//        }

//        public override void Dispose()
//        {

//        }
//    }
//}
