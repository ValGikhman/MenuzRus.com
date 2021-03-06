﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IFloorService {

        Boolean DeleteFloor(Int32? id);

        Boolean DeleteTable(Int32 id);

        Floor GetFloor(Int32 id);

        List<Floor> GetFloors(Int32 id);

        TableOrder GetTableOrder(Int32 tableId);

        String GetTableOrderDate(Int32 tableId);

        Int32 GetTableOrderStatus(Int32 tableId);

        List<Table> GetTables(Int32 id);

        Int32 SaveFloor(Floor floor);

        Boolean SaveTables(List<Table> tables, Int32 floorId);
    }
}