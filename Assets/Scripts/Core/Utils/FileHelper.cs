using System.IO;
using System.Text;

public static class FileHelper
{
    public static bool FileExist(string fullPath)
    {
        return File.Exists(fullPath);
    }

    public static byte[] ReadAllBytes(string path)
    {
        string fullPath = Path.Combine(PathDefine.ASSETBUNDLES_DIR, path);
        if(!FileExist(fullPath))
        {
            Log.Error(LogLevel.Critical, "ReadAllBytes Error,File does not exit,fullPath:{0}", path);
            return null;
        }
        return File.ReadAllBytes(fullPath);
    }

    public static string ReadText(string path, Encoding encoding)
    {
        string fullPath = Path.Combine(PathDefine.ASSETBUNDLES_DIR, path);
        if (!FileExist(fullPath))
        {
            Log.Error(LogLevel.Critical, "ReadText Error,File does not exist,fullPath:{0}", path);
            return null;
        }

        byte[] data = File.ReadAllBytes(fullPath);
        return encoding.GetString(data);
    }

    public static void WriteAllBytes(string path,byte[] data)
    {
        string fullPath = Path.Combine(PathDefine.ASSETBUNDLES_DIR, path);
        if (FileExist(fullPath))
        {
            File.Delete(fullPath);
        }

        File.WriteAllBytes(fullPath, data);
    }

    public static void WriteStr(string path, string str, Encoding encoding)
    {
        string fullPath = Path.Combine(PathDefine.ASSETBUNDLES_DIR, path);
        if (FileExist(fullPath))
        {
            File.Delete(fullPath);
        }

        byte[] data = encoding.GetBytes(str);
        WriteAllBytes(fullPath, data);
    }
}
