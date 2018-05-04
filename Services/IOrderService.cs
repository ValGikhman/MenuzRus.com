using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IOrderService {

        #region Public Methods

        Int32 AddNewTableOrder(Int32 tableId);

        Boolean DeleteCheck(Int32 id);

        Boolean DeleteMenu(Int32 id);

        Boolean DeletePayment(Int32 id);

        Check GetCheck(Int32 checkId);

        List<TableOrder> GetKitchenOrders();

        ChecksMenu GetMenuItem(Int32 id);

        List<ChecksMenu> GetMenuItems(Int32 checkId);

        List<Payment> GetPayments(Int32 checkId);

        Printout GetPrintKitchenOrder(Int32 id);

        List<Printout> GetPrintouts(DateTime dateFrom, DateTime dateTo);

        List<ChecksMenuProduct> GetProducts(Int32 menuId);

        List<Printout> GetQueued4PrintKitchenOrders();

        Table GetTable(Int32 tableId);

        Tuple<String, Int32, String> GetTableAttributes(Int32 tableId, Boolean showPaidChecks);

        TableOrder GetTableOrder(Int32 tableId);

        List<TableOrder> GetTableOrders(Int32 tableId);

        List<TableOrder> GetTableOrdersByFloorId(Int32 floorId);

        Decimal LatestInventory();

        Decimal LatestSale();

        void SaveItem(Int32 productId, Int32 knopaId, CommonUnit.ProductType type);

        ChecksMenu SaveMenuItem(Item menuItem, Int32 tableId, Int32 orderId, Int32 userId);

        Int32 SavePayment(Payment payment, PaymentCC paymentCC, Int32 UserId);

        Boolean UpdateCheckStatus(Int32 checkId, CommonUnit.CheckStatus status);

        Boolean UpdateCheckStatusPaid(Int32 checkId, Decimal price, Decimal tax, Decimal adjustment);

        Boolean UpdateCheckType(Int32 checkId, CommonUnit.CheckType type);

        Boolean UpdateKitchenOrderPrintStatus(Int32 id);

        void UpdateMenuItemStatus(Int32 id, CommonUnit.MenuItemStatus status);

        Boolean UpdateTableStatus(Int32 tableOrderId, CommonUnit.TableOrderStatus status);

        #endregion Public Methods
    }
}