using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public class BaseService : IService {
        public String _connectionString;

        public BaseService() {
            if (Environment.GetEnvironmentVariable("COMPUTERNAME") == "VALS-PC") {
                this.connectionString = Services.Properties.Settings.Default.menuzrusDEV;
            }
            else {
                this.connectionString = Services.Properties.Settings.Default.menuzrusPROD;
            }
        }

        public String connectionString {
            get {
                return _connectionString;
            }
            set {
                _connectionString = value;
            }
        }
    }
}