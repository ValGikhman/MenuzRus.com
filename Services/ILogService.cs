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

        void Log(Common.LogType logType, String messsage, params Object[] data);

        void Log(Common.LogType logType, String messsage, String trace, params Object[] data);

        #endregion public
    }
}