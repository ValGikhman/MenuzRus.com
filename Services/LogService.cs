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

        public void Log(Exception exception) {
            Log(Common.LogType.Exception, exception.Message, exception.StackTrace, exception.StackTrace);
        }

        public void Log(Common.LogType logType, String messsage, params Object[] data) {
            SendToLogger(logType, messsage, null, 0, null, null, data);
        }

        public void Log(Common.LogType logType, String messsage, String trace, params Object[] data) {
            SendToLogger(logType, messsage, trace, 0, null, null, data);
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
                }
            }
            return parameter;
        }

        private void SendToLogger(Common.LogType type, String message, String trace, Int32 userId, String sessionId, String route, params Object[] data) {
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
                return;
            }
        }

        #endregion private
    }
}