using System;
using UnityEngine;

// 版本号位
public static class VersionBit
{
    public const int BINARY = 0;
    public const int MAJOR = 1;
    public const int MIN = 2;
}

[Serializable]
public class VersionData
{
    //binary.major.min

    [SerializeField]
    public int[] versionData = new int[3] { 0, 0, 0 };

    public readonly static VersionData zero = new VersionData();

    public VersionData()
    {
    }

    public VersionData(int[] version)
    {
        if (version.Length == 3)
        {
            versionData = version;
        }
    }

    public VersionData(VersionData version)
    {
        SetValue(version);
    }

    public VersionData(string str)
    {
        Load(str);
    }

    public bool IsZero()
    {
        return versionData[0] == 0 && versionData[1] == 0 && versionData[2] == 0;
    }

    public override bool Equals(object obj)
    {
        VersionData target = obj as VersionData;
        if (null == target)
        {
            return false;
        }

        return (target.versionData[0] == versionData[0]
            && target.versionData[1] == versionData[1]
            && target.versionData[2] == versionData[2]);
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public int MajorId
    {
        get
        {
            return versionData[0];
        }
    }

    public void AddBinaryVersionAndNormal()
    {
        versionData[VersionBit.BINARY]++;
        versionData[VersionBit.MAJOR] = 0;
        versionData[VersionBit.MIN] = 0;
    }

    public int MajorVersion
    {
        get
        {
            return versionData[1];
        }
    }

    public int MinVersion
    {
        get
        {
            return versionData[2];
        }
    }

    public int this[int index]
    {
        get
        {
            return versionData[index];
        }

        set
        {
            versionData[index] = value;
        }
    }

    public bool Load(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        string[] v = str.Split('.');
        if (v.Length != 3)
        {
            return false;
        }

        for (int i = 0; i < 3; ++i)
        {
            int dt;
            if (int.TryParse(v[i], out dt))
            {
                versionData[i] = dt;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void SetValue(VersionData src)
    {
        for (int i = 0; i < 3; ++i)
        {
            versionData[i] = src.versionData[i];
        }
    }

    public override string ToString()
    {
        return string.Format("{0}.{1}.{2}", versionData[0], versionData[1], versionData[2]);
    }

    public bool Great(VersionData version)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (versionData[i] == version.versionData[i])
            {
                continue;
            }

            return versionData[i] > version.versionData[i];
        }

        return false;
    }

    // 规范化到 Major版本 (也就是把Min版本号置为0)
    public void NormalizeToMajorVersion()
    {
        versionData[VersionBit.MIN] = 0;
    }

    public void NormalizeToBinaryVersion()
    {
        versionData[VersionBit.MAJOR] = 0;
        versionData[VersionBit.MIN] = 0;
    }

    public bool IsMajorVersion()
    {
        return versionData[VersionBit.MIN] == 0;
    }

    public int CompareTo(VersionData d)
    {
        if (Equals(d))
        {
            return 0;
        }

        if (Great(d))
        {
            return 1;
        }

        return -1;
    }

    public VersionData Clone()
    {
        VersionData d = new VersionData();
        d.SetValue(this);
        return d;
    }
}
