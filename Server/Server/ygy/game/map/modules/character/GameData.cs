using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.modules.character
{
    public class GameData : ICommonBean
    {
        private string start_date;
        private string end_date;
        private int kill_num;
        private int head_shot_num;
        private int death_num;
        private int harm_num;

        public string Start_date { get => start_date; set => start_date = value; }
        public string End_date { get => end_date; set => end_date = value; }
        public int Kill_num { get => kill_num; set => kill_num = value; }
        public int Head_shot_num { get => head_shot_num; set => head_shot_num = value; }
        public int Death_num { get => death_num; set => death_num = value; }
        public int Harm_num { get => harm_num; set => harm_num = value; }

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
            dBGameData.StartDate = start_date;
            dBGameData.EndDate = end_date;
            dBGameData.KillNum = kill_num;
            dBGameData.HeadShotNum = head_shot_num;
            dBGameData.DeathNum = death_num;
            dBGameData.HarmNum = harm_num;
        }

        public void Serialie2PB(object pbMsg)
        {
            throw new NotImplementedException();
        }

        public void SerialieFromDB(object dbMsg)
        {
            DBGameData dBGameData = dbMsg as DBGameData;
            if (dBGameData == null)
            {
                return;
            }
            start_date = dBGameData.StartDate;
            end_date = dBGameData.EndDate;
            kill_num = dBGameData.KillNum;
            head_shot_num = dBGameData.HeadShotNum;
            death_num = dBGameData.DeathNum;
            harm_num = dBGameData.HarmNum;
        }
    }
}
