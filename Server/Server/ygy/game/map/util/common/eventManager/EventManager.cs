using Server.ygy.game.map.modules.login;
using Server.Ygy.Game.Map;
using Server.Ygy.Game.Map.Util.Common.EventManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common.eventManager
{
    public class EventManager
    {
        private class CalcuObj
        {
            private long endTime;

            private int eventType;

            private object[] paramList;

            public object[] ParamList
            {
                get
                {
                    return paramList;
                }
            }

            public long EndTime
            {
                get
                {
                    return endTime;
                }
            }

            public int EventType
            {
                get
                {
                    return eventType;
                }
            }

            public CalcuObj(int eventType, long endTime, params object[] param)
            {
                this.endTime = endTime;
                this.eventType = eventType;
                this.paramList = param;
            }
        }

        private static EventManager instance;

        private Dictionary<int, List<Action<object[]>>> funcDic;

        private List<CalcuObj> delayEventList;

        private Dictionary<int, Action<ClientPeer>>  autoEventDic;

        private EventManager()
        {
            funcDic = new Dictionary<int, List<Action<object[]>>>();
            delayEventList = new List<CalcuObj>();
            autoEventDic = new Dictionary<int, Action<ClientPeer>>();
        }

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="param">事件参数</param>
        private void ActiveEvent(int eventType, params object[] param)
        {
            if (funcDic == null)
            {
                Console.WriteLine("funcDic对象不存在");
                return;
            }
            funcDic.TryGetValue(eventType, out List<Action<object[]>> eventList);
            if (eventList == null)
            {
                Console.WriteLine($"{eventType}:eventList对象不存在");
                return;
            }
            for (int i = 0; i < eventList.Count; i++)
            {
                Action<object[]> func = eventList[i];
                if (func == null)
                {
                    continue;
                }
                func(param);
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">事件</param>
        public void RegisterEvent(int eventType, Action<object[]> action)
        {
            if (funcDic.ContainsKey(eventType) == true)
            {
                List<Action<object[]>> list = funcDic[eventType];
                if (list == null)
                {
                    return;
                }
                list.Add(action);
            }
            else
            {
                List<Action<object[]>> list = new List<Action<object[]>>();
                list.Add(action);
                funcDic.Add(eventType, list);
            }
        }

        /// <summary>
        /// 注册自动执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="serverEvent"></param>
        public void RegisterAutoEvent(int eventType, Action<ClientPeer> action)
        {
            if(autoEventDic == null)
            {
                return;
            }
            if(autoEventDic.ContainsKey(eventType) == true)
            {
                Action<ClientPeer> serEventList = autoEventDic[eventType];
                if(serEventList == null)
                {
                    return;
                }
                serEventList += action;
            }
            else
            {
                autoEventDic.Add(eventType, action);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="immediately">是否立即执行</param>
        /// <param name="delayTime">延迟时间:s</param>
        /// <param name="param">参数</param>
        public void PushEvent(int eventType, bool immediately, int delayTime, params object[] param)
        {
            if (immediately == true)
            {
                ActiveEvent(eventType, param);
                return;
            }
            long delayTimeL = delayTime * 10000000;
            CalcuObj o = new CalcuObj(eventType, DateTime.Now.Ticks + delayTimeL, param);
            delayEventList.Add(o);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public void PushEvent(int eventType)
        {
            ActiveEvent(eventType);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public void PushEvent(int eventType, bool immediately)
        {
            if(immediately == false)
            {
                return;
            }
            ActiveEvent(eventType);
        }

        /// <summary>
        /// 自动执行事件
        /// </summary>
        public void AutoPushEvent()
        {
            // 到达新一天事件
            NewDay();
            // ...

        }

        // 到达新一天事件
        public void NewDay()
        {
            autoEventDic.TryGetValue(EventDefine.Event_NewDay, out Action<ClientPeer> eventList);
            if(eventList == null)
            {
                return;
            }
            if(DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            {
                if(LoginModuleManager.Instance.ClientDic == null)
                {
                    return;
                }
                foreach (var item in LoginModuleManager.Instance.ClientDic)
                {
                    eventList(item.Value);
                }
            }
        }

        public void Update()
        {
            // 延时执行事件
            for (int i = delayEventList.Count - 1; i >= 0; i--)
            {
                if (DateTime.Now.Ticks >= delayEventList[i].EndTime)
                {
                    List<Action<object[]>> funcList = funcDic[delayEventList[i].EventType];
                    if (funcList == null)
                    {
                        delayEventList.RemoveAt(i);
                        continue;
                    }
                    foreach (var item in funcList)
                    {
                        item(delayEventList[i].ParamList);
                    }
                    delayEventList.RemoveAt(i);
                }
            }
            // 自动执行事件
            AutoPushEvent();
            // 按固定频率刷新事件
            PushEvent(EventDefine.Event_Update, true);
        }
    }
}
