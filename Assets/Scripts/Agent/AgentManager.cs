using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class AgentManager : ModuleManager<AgentManager>
{
    private GameObject mHeroNode;
    private GameObject mMonsterNode;

    private Hero mHero;
    private List<Monster> mMonsters;



    public override void Dispose()
    {
        
    }

    public override void Initialize()
    {
        mHero = null;
        mMonsters = new List<Monster>();
        mHeroNode = GameObject.Find("_AGENT/_HERO");
        mMonsterNode = GameObject.Find("_AGENT/_MONSTER");
    }

    public GameObject GetHeroNode()
    {
        return mHeroNode;
    }

    public GameObject GetMonsterNode()
    {
        return mMonsterNode;
    }

    public void LoadHero(uint heroId)
    {
        mHero = new Hero(heroId);
        mHero.Initialize();
    }

    public void RemoveHero()
    {
        mHero.Dispose();
        mHero = null;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(mHero != null)
        {
            mHero.OnUpdate(deltaTime);
        }

        foreach(Monster monster in mMonsters)
        {
            monster.OnUpdate(deltaTime);
        }
    }
}
