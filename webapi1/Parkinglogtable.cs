using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi1
{
    public class Parkinglogtable
    {
        public string ParkingLogId { get; set; }
        public int VehicleTypeId { get; set; }

        public string TransactionNumber { get; set; }

        public string VehicleNumber { get; set; }

        public string CardNo { get; set; }

        public string EPC { get; set; }

        public string TId { get; set; }

        public string Userdata { get; set; }

        public string TAGSignature { get; set; }

        public string FastTAGVehicleNo { get; set; }

        public int FastTAGVehicleClassCode { get; set; }

       

        public int InCreatedBy { get; set; }

        public string BarCodeId { get; set; }

        public int ReceiptId { get; set; }




    }
}
