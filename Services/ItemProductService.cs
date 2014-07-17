﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class ItemProductService {

        public Boolean DeleteItemProduct(Int32? id) {
            ItemProduct itemProduct = new ItemProduct();
            IEnumerable<ItemProductAssociation> itemProductAssocoation; ;
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    itemProductAssocoation = db.ItemProductAssociations.Where(m => m.ItemProductId == id);
                    if (itemProductAssocoation != default(ItemProductAssociation)) {
                        db.ItemProductAssociations.DeleteAllOnSubmit(itemProductAssocoation);
                    }

                    itemProduct = db.ItemProducts.Where(m => m.id == id).FirstOrDefault();
                    if (itemProduct != default(ItemProduct)) {
                        db.ItemProducts.DeleteOnSubmit(itemProduct);
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

        public Boolean DeleteItemProductAssociations(Int32 id) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    IEnumerable<ItemProductAssociation> query = db.ItemProductAssociations.Where(m => m.ItemProductId == id);
                    if (query != default(ItemProductAssociation)) {
                        db.ItemProductAssociations.DeleteAllOnSubmit(query);
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

        public Boolean SaveItemProduct(ItemProduct item) {
            ItemProduct itemQuery = new ItemProduct();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (item.id != 0)
                        itemQuery = db.ItemProducts.Where(m => m.id == item.id).FirstOrDefault();
                    if (itemQuery != default(ItemProduct)) {
                        itemQuery.id = item.id;
                        itemQuery.Type = item.Type;
                        itemQuery.ItemId = item.ItemId;
                    }
                    if (item.id == 0) {
                        db.ItemProducts.InsertOnSubmit(itemQuery);
                    }
                    db.SubmitChanges();
                    DeleteItemProductAssociations(itemQuery.id);
                    foreach (ItemProductAssociation ipa in item.ItemProductAssociations) {
                        ItemProductAssociation itemsQuery = new ItemProductAssociation();
                        itemsQuery.ItemProductId = itemQuery.id;
                        itemsQuery.ItemId = ipa.ItemId;
                        db.ItemProductAssociations.InsertOnSubmit(itemsQuery);
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