using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public class LogService : BaseService, ILogService {

        #region public

        public void Log(Exception exception, Int32 userId, String sessionId) {
            Log(Common.LogType.Exception, userId, sessionId, exception.Message, exception.StackTrace);
        }

        public void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, params Object[] data) {
            SendToLogger(logType, userId, sessionId, messsage, null, null, data);
        }

        public void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, String trace, params Object[] data) {
            SendToLogger(logType, userId, sessionId, messsage, trace, null, data);
        }

        public void Log(Common.LogType logType, Int32 userId, String sessionId, String messsage, String trace, String route, params Object[] data) {
            SendToLogger(logType, userId, sessionId, messsage, trace, route, data);
        }

        #endregion public

        #region private

        private String BuildParameters(Object[] data) {
            Int32 i;
            String parameter = String.Empty;
            if (data != null) {
                try {
                    foreach (Object[] obj in data) {
                        if (obj != null) {
                            i = 1;
                            parameter = String.Empty;
                            foreach (Object o in obj) {
                                if (i % 2 != 0)
                                    parameter += o.ToString();
                                else
                                    parameter += String.Format(":{0}{1}", o.ToString(), Environment.NewLine);

                                i++;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
            return parameter;
        }

        private void SendToLogger(Common.LogType type, Int32 userId, String sessionId, String message, String trace, String route, params Object[] data) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    Log log = new Log();
                    log.IP = Common.GetIP();
                    log.LogType = (Int32)type;
                    log.UserId = userId;
                    log.SessionId = sessionId != null ? sessionId : "N/A";
                    log.Message = String.Format("{0} {1}{2}", message, Environment.NewLine, BuildParameters(data));
                    log.Trace = trace;
                    log.Route = route;
                    db.Logs.InsertOnSubmit(log);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        #endregion private
    }
}