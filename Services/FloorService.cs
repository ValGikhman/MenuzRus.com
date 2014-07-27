﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class FloorService {

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
                SessionData.exeption = ex;
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
                        db.Tables.DeleteOnSubmit(query);
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

        public Floor GetFloor(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Floors.FirstOrDefault(m => m.id == id);
        }

        public List<Floor> GetFloors(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Floors.Where(m => m.CustomerId == id).ToList();
        }

        public Int32 GetTableOrderStatus(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            TableOrder retVal = db.TableOrders.FirstOrDefault(m => m.TableId == id);
            if (retVal != default(TableOrder)) {
                return retVal.Status;
            }
            return (Int32)Common.TableOrderStatus.Open;
        }

        public List<Table> GetTables(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Tables.Where(m => m.FloorId == id).ToList();
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
                SessionData.exeption = ex;
                return 0;
            }
            return query.id;
        }

        public Boolean SaveTables(List<Table> tables) {
            Table table;
            Int32 floorId;
            IEnumerable<Table> tablesToDelete;
            try {
                floorId = SessionData.floor.id;
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    tablesToDelete = db.Tables.Where(m => m.FloorId == floorId && !tables.Contains(m));
                    if (tablesToDelete.Any())
                        db.Tables.DeleteAllOnSubmit(tablesToDelete);
                    if (tables.Any()) {
                        foreach (Table t in tables) {
                            table = new Table();
                            table.Col = t.Col;
                            table.FloorId = t.FloorId;
                            table.Name = t.Name;
                            table.Row = t.Row;
                            table.Type = t.Type;
                            table.X = t.X;
                            table.Y = t.Y;
                            if (t.id == 0) {
                                db.Tables.InsertOnSubmit(table);
                            }
                        }
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }
    }
}