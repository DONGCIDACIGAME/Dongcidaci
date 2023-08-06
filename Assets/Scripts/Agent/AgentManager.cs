using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class AgentManager : Singleton<AgentManager>
{
    private GameObject mHeroNode;
    private GameObject mMonsterNode;

    private Hero mHero;
    private HashSet<Monster> mMonsters;



    public void Initialize()
    {
        mHero = null;
        mMonsters = new HashSet<Monster>();
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

    public void LoadMonster(uint monsterId)
    {
        Monster monster = new Monster(monsterId);
        monster.Initialize();
        mMonsters.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        if(monster == null)
        {
            Log.Error(LogLevel.Normal, "Remove monster failed, monster is null!");
            return;
        }

        monster.Dispose();

        if (mMonsters.Contains(monster))
            mMonsters.Remove(monster);
    }

    public void RemoveAllMonsters()
    {
        foreach(Monster monster in mMonsters)
        {
            monster.Dispose();
        }

        mMonsters = null;
    }

    public void Dispose()
    {
        RemoveHero();
        RemoveAllMonsters();

        mHeroNode = null;
        mMonsterNode = null;
    }

    public void OnGameUpdate(float deltaTime)
    {
        if(mHero != null)
        {
            mHero.OnUpdate(deltaTime);
        }

        foreach(Monster monster in mMonsters)
        {
            monster.OnUpdate(deltaTime);
        }
    }

    public void OnLateUpdate(float deltaTime)
    {
        if (mHero != null)
        {
            mHero.OnLateUpdate(deltaTime);
        }

        foreach (Monster monster in mMonsters)
        {
            monster.OnLateUpdate(deltaTime);
        }
    }
}
