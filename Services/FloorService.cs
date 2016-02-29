using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class FloorService : IFloorService {

        public Boolean DeleteFloor(Int32? id) {
            Floor query = new Floor();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Floors.FirstOrDefault(m => m.id == id);
                    if (query != default(Floor)) {
                        IEnumerable<Table> tables = db.Tables.Where(m => m.FloorId == id);
                        if (tables != null) {
                            db.Tables.DeleteAllOnSubmit(tables);
                        }
                        db.Floors.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean DeleteTable(Int32 id) {
            Table query = new Table();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Tables.FirstOrDefault(m => m.id == id);
                    if (query != default(Table)) {
                        query.Status = (Int32)Common.Status.NotActive;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Floor GetFloor(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Floors.FirstOrDefault(m => m.id == id);
        }

        public List<Floor> GetFloors(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Floors.Where(m => m.CustomerId == id).ToList();
        }

        public TableOrder GetTableOrder(Int32 tableId) {
            OrderService service = new OrderService();
            TableOrder tableOrder = service.GetTableOrder(tableId);
            if (tableOrder != default(TableOrder)) {
                return tableOrder;
            }
            return null;
        }

        public String GetTableOrderDate(Int32 tableId) {
            OrderService service = new OrderService();
            TableOrder tableOrder = service.GetTableOrder(tableId);
            if (tableOrder != default(TableOrder)) {
                return String.Format("{0:M/d/yyyy HH:mm:ss}", tableOrder.DateModified);
            }
            return String.Format("{0:M/d/yyyy HH:mm:ss}", DateTime.Now);
        }

        public Int32 GetTableOrderStatus(Int32 tableId) {
            OrderService service = new OrderService();
            TableOrder tableOrder = service.GetTableOrder(tableId);
            if (tableOrder != default(TableOrder)) {
                return tableOrder.Status;
            }
            return (Int32)Common.TableOrderStatus.Closed;
        }

        public List<Table> GetTables(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Tables.Where(m => m.FloorId == id && m.Status == (Int32)Common.Status.Active).ToList();
        }

        public Int32 SaveFloor(Floor floor) {
            Floor query = new Floor();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (floor.id != 0)
                        query = db.Floors.FirstOrDefault(m => m.id == floor.id);
                    if (query != default(Floor)) {
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
                return 0;
            }
            return query.id;
        }

        public Boolean SaveTables(List<Table> tables, Int32 floorId) {
            Table table;
            IEnumerable<Table> tablesToDelete;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    tablesToDelete = db.Tables.Where(m => m.FloorId == floorId && !tables.Contains(m));
                    if (tablesToDelete.Any()) {
                        db.Tables.DeleteAllOnSubmit(tablesToDelete);
                        db.SubmitChanges();
                    }
                    if (tables.Any()) {
                        foreach (Table t in tables) {
                            table = new Table();
                            if (t.id != 0) {
                                table = db.Tables.FirstOrDefault(m => m.id == t.id);
                            }

                            table.Top = t.Top;
                            table.FloorId = floorId;
                            table.Name = t.Name;
                            table.Left = t.Left;
                            table.Status = (Int32)Common.Status.Active;
                            table.Type = t.Type;
                            table.Width = t.Width;
                            table.Height = t.Height;
                            if (t.id == 0) {
                                db.Tables.InsertOnSubmit(table);
                            }
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }
    }
}