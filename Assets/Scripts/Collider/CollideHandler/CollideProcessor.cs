using GameEngine;
using System.Collections.Generic;
using System;

public static class CollideHandleCenter
{
    /// <summary>
    /// 处理两个实体的碰撞逻辑
    /// 对src进行类别判断，使用对应的处理机
    /// </summary>
    /// <param name="src"></param>
    /// <param name="tgt"></param>
    public static void HandleCollideFromSrcToTgt(Entity src,Entity tgt)
    {
        //此处通过src确定处理机
        if (src is Hero)
        {
            var srcHero = src as Hero;
            HeroCollideProcessor.Ins.HandleCollideFromSrcToTgt(srcHero,tgt);
        }else if (src is Monster)
        {
            var srcMonster = src as Monster;
            MonsterCollideProcessor.Ins.HandleCollideFromSrcToTgt(srcMonster,tgt);

        }else if (src is MapBlock)
        {
            var srcMapBlock = src as MapBlock;
            BlockCollideProcessor.Ins.HandleCollideFromSrcToTgt(srcMapBlock,tgt);
        }

    }


    
}


public abstract class CollideProcessor<T1,T2> : Singleton<T1> where T1 :new() where T2: Entity
{
    /// <summary>
    /// 已知处理者的情况下，对触碰的对象判断
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="tgtEntity"></param>
    public void HandleCollideFromSrcToTgt(T2 handler, Entity tgtEntity)
    {
        // 此处根据目标类型，确定调用处理机的哪个方法
        if (tgtEntity is Hero)
        {
            var tgtHero = tgtEntity as Hero;
            HandleColliderToHero(handler,tgtHero);
        }
        else if (tgtEntity is Monster)
        {
            var tgtMonster = tgtEntity as Monster;
            HandleColliderToMonster(handler, tgtMonster);

        }else if (tgtEntity is MapBlock)
        {
            var tgtBlock = tgtEntity as MapBlock;
            HandleColliderToBlock(handler,tgtBlock);
        }



    }



    public abstract void HandleColliderToHero(T2 handler, Hero tgtHero);
    public abstract void HandleColliderToMonster(T2 handler, Monster tgtMonster);
    public abstract void HandleColliderToBlock(T2 handler, MapBlock tgtBlock);





}
