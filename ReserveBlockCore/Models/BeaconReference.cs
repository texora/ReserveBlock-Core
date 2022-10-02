﻿using ReserveBlockCore.Data;
using ReserveBlockCore.Utilities;

namespace ReserveBlockCore.Models
{
    public class BeaconReference
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime CreateDate { get; set; }

        public static LiteDB.ILiteCollection<BeaconReference>? GetBeaconReference()
        {
            try
            {
                var beaconRef = DbContext.DB_Beacon.GetCollection<BeaconReference>(DbContext.RSRV_BEACON_REF);
                return beaconRef;
            }
            catch (Exception ex)
            {
                DbContext.Rollback();
                ErrorLogUtility.LogError(ex.Message, "BeaconReference.GetBeaconReference()");
                return null;
            }

        }

        public static void SaveBeaconReference(BeaconReference beaconRefData, bool update = false)
        {
            var beaconRef = GetBeaconReference();
            if (beaconRef == null)
            {
                ErrorLogUtility.LogError("GetBeaconReference() returned a null value.", "BeaconReference.SaveBeaconData()");
            }
            else
            {
                var beaconDataRec = beaconRef.FindAll();
                if (beaconDataRec.Count() == 0)
                {
                    beaconRef.InsertSafe(beaconRefData);
                }
                else
                {
                    if(update)
                    {
                        var record = beaconDataRec.First();
                        record.Reference = beaconRefData.Reference;
                        beaconRef.Update(record);
                    }
                }
            }
        }

        public static async Task<string?> GetReference()
        {
            var beaconRef = GetBeaconReference();
            if (beaconRef == null)
            {
                ErrorLogUtility.LogError("GetBeaconReference() returned a null value.", "BeaconReference.SaveBeaconData()");
                return null;
            }
            else
            {
                var beaconDataRec = beaconRef.FindAll().FirstOrDefault();
                if(beaconDataRec == null)
                {
                    return null;
                }
                else
                {
                    return beaconDataRec.Reference;
                }
            }
        }
    }
}
