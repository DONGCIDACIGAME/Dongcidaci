using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class GameFXManager : ModuleManager<GameFXManager>
{
    public GameObject FXPoolNode => GameObject.Find("_FX_POOL");
    public GameObject FXDefaultCarryNode => GameObject.Find("_FX_DEFAULT");

    private List<GameObject> _fxObjs = new List<GameObject>();

    public void ShowAFXUnderDefaultNode(FXConfigData fxCfg, Vector3 anchorPos,Vector3 anchorRotate)
    {
        var popRet = PopAFXFromPoolWith(fxCfg.fxName);
        if (popRet == null)
        {
            popRet = PrefabUtil.LoadPrefab(GameFXDefine.FXPathPreffix + fxCfg.fxName,"Load a fx obj");
            popRet.transform.SetParent(FXDefaultCarryNode.transform,false);
        }
        else
        {
            
            popRet.transform.SetParent(FXDefaultCarryNode.transform, false);
            //popRet.SetActive(true);
        }
        popRet.name = fxCfg.fxName;

        var realAngle = anchorRotate + new Vector3(fxCfg.transInfo.angleX, fxCfg.transInfo.angleY, fxCfg.transInfo.angleZ);
        var initPosV3 = new Vector3(fxCfg.transInfo.posX, fxCfg.transInfo.posY, fxCfg.transInfo.posZ);
        var rotateX_V3 = Quaternion.AngleAxis(realAngle.x,Vector3.right) *initPosV3;
        var rotateY_V3 = Quaternion.AngleAxis(realAngle.y, Vector3.up) * rotateX_V3;
        var rotateZ_V3 = Quaternion.AngleAxis(realAngle.z, Vector3.forward) * rotateY_V3;

        popRet.transform.position = anchorPos + rotateZ_V3;
        popRet.transform.eulerAngles = realAngle;
        popRet.transform.localScale = new Vector3(fxCfg.transInfo.scaleX, fxCfg.transInfo.scaleY, fxCfg.transInfo.scaleZ);
        popRet.SetActive(true);
    }


    public void ShowAFX(FXConfigData fxCfg, Agent tgtAgent)
    {
        if (fxCfg == null || tgtAgent == null || fxCfg.fxName == string.Empty) return;
        var tgtCarryNode = tgtAgent.GetFXCarryNode(fxCfg.carryNodeName);
        if (tgtCarryNode == null) return;
        ShowAFX(fxCfg, tgtCarryNode);
    }


    public void ShowAFX(FXConfigData fxCfg,GameObject tgtCarryNode)
    {
        if(tgtCarryNode == null || fxCfg.fxName == string.Empty)
        {
            return;
        }

        var popRet = PopAFXFromPoolWith(fxCfg.fxName);
        if (popRet == null)
        {
            popRet = PrefabUtil.LoadPrefab(GameFXDefine.FXPathPreffix + fxCfg.fxName, "Load a fx obj");
            popRet.transform.SetParent(tgtCarryNode.transform, false);
        }
        else
        {

            popRet.transform.SetParent(tgtCarryNode.transform, false);
        }

        popRet.name = fxCfg.fxName;

        var realAngle = new Vector3(fxCfg.transInfo.angleX, fxCfg.transInfo.angleY, fxCfg.transInfo.angleZ);
        var initPosV3 = new Vector3(fxCfg.transInfo.posX, fxCfg.transInfo.posY, fxCfg.transInfo.posZ);

        popRet.transform.localPosition = initPosV3;
        popRet.transform.localEulerAngles = realAngle;
        popRet.transform.localScale = new Vector3(fxCfg.transInfo.scaleX, fxCfg.transInfo.scaleY, fxCfg.transInfo.scaleZ);
        popRet.SetActive(true);
    }




    private GameObject PopAFXFromPoolWith(string fxName)
    {
        if (_fxObjs == null || _fxObjs.Count == 0) return null;
        int pushIndex = -1;
        for (int i=0;i<_fxObjs.Count;i++)
        {
            if (_fxObjs[i].name.Equals(fxName))
            {
                Log.Logic(LogLevel.Normal, "PopAFXFromPoolWith find old one");
                pushIndex = i;
                break;
            }
        }

        if(pushIndex >= 0)
        {
            var ret = _fxObjs[pushIndex];
            _fxObjs.RemoveAt(pushIndex);
            return ret;
        }
        return null;
    }


    public void RecycleAFX(GameObject fxObj)
    {
        fxObj.SetActive(false);
        fxObj.transform.SetParent(FXPoolNode.transform,false);
        fxObj.transform.position = Vector3.zero;
        fxObj.transform.eulerAngles = Vector3.zero;
        fxObj.transform.localScale = Vector3.one;
        _fxObjs.Add(fxObj);
    }



    public override void Initialize()
    {
        _fxObjs = new List<GameObject>();

    }


    public override void Dispose()
    {
        if (_fxObjs != null && _fxObjs.Count>0)
        {
            for (int i = _fxObjs.Count-1; i>=0; i--)
            {
                GameObject.Destroy(_fxObjs[i]);
            }
        }
        
        _fxObjs = null;
    }

    
}
