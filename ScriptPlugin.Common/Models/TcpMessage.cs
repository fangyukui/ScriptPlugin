using System.Drawing;
using ScriptPlugin.Common.Enums;

namespace ScriptPlugin.Common.Models
{
    public class TcpMessage
    {
        public string Version { get; set; } = "1.0";
        public bool IsServer { get; set; }
        public MsgType MsgType { get; set; } = MsgType.Log;
        public string PcName { get; set; } = "";
        public string OsName { get; set; } = "";
        public Point Screen { get; set; } = new Point();
        public ActionType Action { get; set; } = ActionType.None;
        public string Msg { get; set; } = "";
        public int Value { get; set; }
        public string Ip { get; set; } = "";
        public RunState TaskState { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
