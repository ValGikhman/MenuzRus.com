using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class FloorService {

        public Boolean DeleteFloor(Int32? id) {
            Floors query = new Floors();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Floors.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Floors)) {
                        //IEnumerable<Tables> tables = db.Tables.Where(m => m.FloorId == id);
                        //if (tables != null) {
                        //    db.Tables.DeleteAllOnSubmit(tables);
                        //}
                        db.Floors.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public List<Floors> GetFloors(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Floors.Where(m => m.CustomerId == id).ToList();
        }

        public Int32 SaveFloor(Floors floor) {
            Floors query = new Floors();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (floor.id != 0)
                        query = db.Floors.Where(m => m.id == floor.id).FirstOrDefault();
                    if (query != default(Floors)) {
                        query.CustomerId = floor.CustomerId;
                        query.Name = floor.Name;
                        query.Description = floor.Description;
                    }
                    if (floor.id == 0) {
                        db.Floors.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return 0;
            }
            return query.id;
        }
    }
}