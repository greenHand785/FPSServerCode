using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Pb;
using System.Collections;


public class GameRecordInfo : ICommonBean
{
    private long end_date;
    private int kill_num;
    private int head_shot_num;
    private int death_num;
    private int harm_num;
    private int rank;
    private string name;
    private string account;

    public void Save2DB(object dbMsg)
    {
        
    }

    public void Serialie2PB(object pbMsg)
    {
        if(pbMsg == null)
        {
            return;
        }
        PBMsgGameData data = pbMsg as PBMsgGameData;
        if(data == null)
        {
            return;
        }
        data.EndDate = end_date;
        data.KillNum = kill_num;
        data.HeadShotNum = head_shot_num;
        data.DeathNum = death_num;
        data.HarmNum = harm_num;
        data.Rank = rank;
        data.Name = name;
        data.Account = account;
    }

    public void SerializeFromPB(object pbMsg)
    {
        PBMsgGameData data = pbMsg as PBMsgGameData;
        if (data == null)
        {
            return;
        }
        account = data.Account;
        name = data.Name;
        rank = data.Rank;
        harm_num = data.HarmNum;
        death_num = data.DeathNum;
        head_shot_num = data.HeadShotNum;
        kill_num = data.KillNum;
        end_date = data.EndDate;
    }

    public void SerialieFromDB(object dbMsg)
    {
        DBGameData data = dbMsg as DBGameData;
        if(data == null)
        {
            return;
        }
        end_date = data.EndDate;
        kill_num = data.KillNum;
        head_shot_num = data.HeadShotNum;
        death_num = data.DeathNum;
        harm_num = data.HarmNum;
        rank = data.Rank;
        name = data.Name;
        account = data.Account;
    }
}
