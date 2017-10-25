using log4net;
using ScriptPlugin.Theme;

namespace ScriptPlugin.Common
{
    public class Params
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Params));

        //消息通知
        public static Messenger Messenger = new Messenger();
    }
}
