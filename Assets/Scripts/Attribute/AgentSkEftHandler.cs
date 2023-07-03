using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect
{
    public class AgentSkEftHandler:IRecycle
    {
        private Agent _bindAgent;
        private List<IPortableEffect> _carrySkEfts;


        public void InitAgentSkEftHandler(Agent agt)
        {
            _bindAgent = agt;
            _carrySkEfts = new List<IPortableEffect>();
        }












        public void Dispose()
        {
            _bindAgent = null;
            _carrySkEfts = null;

        }

        public void Recycle()
        {
            Dispose();
            GamePoolCenter.Ins.AgtSkEftHandlerPool.Push(this);
        }

        public void RecycleReset()
        {
            Dispose();
        }

    }

}

