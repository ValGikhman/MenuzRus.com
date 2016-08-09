using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public class BaseService : IService {
        public readonly String devComputer = "VALS-PC";

        public String _connectionString;

        public BaseService() {
            //TODO: Connection String Change
            if (Environment.GetEnvironmentVariable("COMPUTERNAME") == devComputer) {
                this.connectionString = Services.Properties.Settings.Default.menuzrusDEV;
            }
            else {
                this.connectionString = Services.Properties.Settings.Default.menuzrusPROD;
            }
            // TODO: Remove
            this.connectionString = Services.Properties.Settings.Default.menuzrusPROD;
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