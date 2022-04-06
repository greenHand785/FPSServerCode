using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class GameRecordData : ICommonBean
    {
        private long id;
        private long start_date;
        private List<GameRecordInfo> gameRecordInfos;

        public long Id { get => id; set => id = value; }
        public long Start_date { get => start_date; set => start_date = value; }
        public List<GameRecordInfo> GameRecordInfos { get => gameRecordInfos; set => gameRecordInfos = value; }

        public void Save2DB(object dbMsg)
        {
            if(dbMsg == null)
            {
                dbMsg = new DBGameData();
            }
            DBGameData dBGameData = dbMsg as DBGameData;
            if(dBGameData == null)
            {
                return;
            }
            
        }

        public void Serialie2PB(object pbMsg)
        {
            if(pbMsg == null)
            {
                return;
            }
            PBMsgGameRecordInfo info = pbMsg as PBMsgGameRecordInfo;
            if(info == null)
            {
                return;
            }
            info.Id = id;
            info.StartDate = start_date;
            info.Info = new List<PBMsgGameData>();
            if(gameRecordInfos == null)
            {
                return;
            }
            foreach (var item in gameRecordInfos)
            {
                PBMsgGameData data = new PBMsgGameData();
                item.Serialie2PB(data);
                info.Info.Add(data);
            }
        }

        public void SerialieFromDB(object dbMsg)
        {
            DBGameRecordData dBGameData = dbMsg as DBGameRecordData;
            if (dBGameData == null)
            {
                return;
            }
            if(gameRecordInfos == null)
            {
                gameRecordInfos = new List<GameRecordInfo>();
            }
            if(dBGameData.Info == null)
            {
                return;
            }
            id = dBGameData.Id;
            start_date = dBGameData.StartDate;
            gameRecordInfos.Clear();
            foreach (var item in dBGameData.Info)
            {
                GameRecordInfo info = new GameRecordInfo();
                info.SerialieFromDB(item);
                gameRecordInfos.Add(info);
            }
        }
    }
}
