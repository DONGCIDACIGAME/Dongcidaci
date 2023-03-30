using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class UwUrlData
{
    private List<string> _cdnUrl = new List<string>();
    private int _cdnIndex = 0;

    public void setCdnUrl(string url)
    {
        _cdnUrl.Clear();
        string[] urlList = url.Split(';');
        for (int i = 0; i < urlList.Length; i++)
        {
            _cdnUrl.Add(urlList[i]);
        }
    }

    public string pickCdnUrl
    {
        get
        {
            _cdnIndex = _cdnIndex % _cdnUrl.Count;
            string url = _cdnUrl[_cdnIndex];
            ++_cdnIndex;
            return url;
        }
    }
}
