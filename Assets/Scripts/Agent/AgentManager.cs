using System.Collections.Generic;

public class AgentManager : IMeterHandler
{
    private Hero mHero;
    private HashSet<Monster> mMonsters;

    public void Initialize()
    {
        mHero = null;
        mMonsters = new HashSet<Monster>();
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

    public void OnMeterEnter(int meterIndex)
    {
        if (mHero != null)
        {
            mHero.OnMeterEnter(meterIndex);
        }

        foreach (Monster monster in mMonsters)
        {
            monster.OnMeterEnter(meterIndex);
        }
    }

    public void OnMeterEnd(int meterIndex)
    {
        if (mHero != null)
        {
            mHero.OnMeterEnd(meterIndex);
        }

        foreach (Monster monster in mMonsters)
        {
            monster.OnMeterEnd(meterIndex);
        }
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        if (mHero != null)
        {
            mHero.OnDisplayPointBeforeMeterEnter(meterIndex);
        }

        foreach (Monster monster in mMonsters)
        {
            monster.OnDisplayPointBeforeMeterEnter(meterIndex);
        }
    }
}
