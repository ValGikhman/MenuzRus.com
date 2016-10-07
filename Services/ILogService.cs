using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public interface ILogService {

        #region public

        void Log(Exception exception, Int32 userId, String sessionId);

        void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, params Object[] data);

        void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, String trace, params Object[] data);

        void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, String trace, String route, params Object[] data);

        #endregion public
    }
}