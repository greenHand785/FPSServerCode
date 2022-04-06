using Server.ygy.game.map.util.common.interfaceDefine;
using Server.Ygy.Game.Db;
using Server.Ygy.Game.Pb;
using System.Collections;

public class FriendInfo : ICommonBean
{
    private string account;
    private string name;
    private string user_img;

    public string Account { get => account; set => account = value; }
    public string Name { get => name; set => name = value; }
    public string User_img { get => user_img; set => user_img = value; }

    public void Save2DB(object dbMsg)
    {
        
    }

    public void Serialie2PB(object pbMsg)
    {
        if(pbMsg == null)
        {
            return;
        }
        PBMsgFriendInfo info = pbMsg as PBMsgFriendInfo;
        info.Account = account;
        info.Name = name;
        info.UserImg = user_img;
    }

    public void SerialieFromDB(object dbMsg)
    {
        DBFriendInfo dbInfo = dbMsg as DBFriendInfo;
        if(dbInfo == null)
        {
            return;
        }
        account = dbInfo.Account;
        name = dbInfo.Name;
        user_img = dbInfo.UserImg;
    }
}
