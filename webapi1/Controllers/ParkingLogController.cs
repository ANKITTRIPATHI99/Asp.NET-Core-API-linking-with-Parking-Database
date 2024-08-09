using Microsoft.AspNetCore.Mvc;


using System.Data;

using Microsoft.Data.SqlClient;
using System.Security.Cryptography;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

   
    public class ParkingLogController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ParkingLogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetParking()
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("INTELLI_PAVILION_LUDHIANA")))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from ParkingLog", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                if (dt.Rows.Count > 0)
                {
                    List<Parkinglogtable> parkinglist = new List<Parkinglogtable>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Parkinglogtable ParkingLog = new Parkinglogtable
                        {
                            VehicleTypeId = Convert.ToInt32(row["VehicleTypeId"]),
                            
                        };
                        parkinglist.Add(ParkingLog);
                    }
                    return Ok(parkinglist);
                }
                else
                {
                    return NotFound(new Response { StatusCode = 100, ErrorMessage = "No data found" });
                }
            }
        }

        [HttpGet]

        [Route("api/ParkingLog")]


        public IActionResult GetParking([FromQuery] string VehicleNumber = null, [FromQuery] string TId = null)
        {
            if (string.IsNullOrEmpty(VehicleNumber) && string.IsNullOrEmpty(TId))
            {
                return BadRequest(new Response { StatusCode = 400, ErrorMessage = "Either VehicleNumber or TransactionID must be provided" });
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("INTELLI_PAVILION_LUDHIANA")))  
                    {
                        con.Open();

                        string query = "SELECT * FROM ParkingLog WHERE 1=1";
                        if (!string.IsNullOrEmpty(VehicleNumber))
                        {
                            query += " AND VehicleNumber = @VehicleNumber";
                        }
                        if (!string.IsNullOrEmpty(TId))
                        {
                            query += " AND TId = @TId";
                        } 

                        SqlDataAdapter da = new SqlDataAdapter(query, con);
                        if (!string.IsNullOrEmpty(VehicleNumber))
                        {
                            da.SelectCommand.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                        }
                        if (!string.IsNullOrEmpty(TId))
                        {
                            da.SelectCommand.Parameters.AddWithValue("@TId", TId);
                        }

                        //[Route("{VehicleTypeId}")]
                        //public IActionResult GetParking(int VehicleTypeId)
                        //{
                        //    try
                        //    {
                        //        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("INTELLI_PAVILION_LUDHIANA")))
                        //        {
                        //            con.Open();  // Ensure connection is opened before executing query

                        //            string query = "SELECT * FROM ParkingLog WHERE VehicleTypeId = @VehicleTypeId";
                        //            SqlDataAdapter da = new SqlDataAdapter(query, con);
                        //            da.SelectCommand.Parameters.AddWithValue("@VehicleTypeId", VehicleTypeId);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            List<Parkinglogtable> parkingList = new List<Parkinglogtable>();
                            foreach (DataRow row in dt.Rows)
                            {
                                Parkinglogtable parkingLog = new Parkinglogtable
                                {

                                    ParkingLogId = row["ParkingLogId"].ToString(),

                                    VehicleTypeId = Convert.ToInt32(row["VehicleTypeId"]),

                                    VehicleNumber = row["VehicleNumber"].ToString(),

                                    TransactionNumber = row["TransactionNumber"].ToString(),

                                    CardNo = row["CardNo"].ToString(),
                                    EPC = row["EPC"].ToString(),

                                    TId = row["TId"].ToString(),

                                    Userdata = row["Userdata"].ToString(),

                                    TAGSignature = row["TAGSignature"].ToString(),

                                    FastTAGVehicleNo = row["FastTAGVehicleNo"].ToString(),

                                    FastTAGVehicleClassCode = Convert.ToInt32(row["FastTAGVehicleClassCode"]),

                                    BarCodeId = row["BarCodeId"].ToString(),

                                    ReceiptId = Convert.ToInt32(row["ReceiptId"]),

                                };
                                parkingList.Add(parkingLog);
                            }
                            return Ok(parkingList);
                        }
                        else
                        {
                            return NotFound(new Response { StatusCode = 100, ErrorMessage = "No data found" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) as needed for debugging or monitoring
                    return StatusCode(500, new Response { StatusCode = 500, ErrorMessage = "An error occurred while retrieving data." });
                }
            }
        }

        [HttpPost]
        public IActionResult AddEmployee(Parkinglogtable park)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("INTELLI_PAVILION_LUDHIANA")))
                {
                    con.Open();

                    // Construct the SQL command to insert the new parking log
                    string query = "INSERT INTO ParkingLog (ParkingLogId, VehicleTypeId, TransactionNumber, VehicleNumber, CardNo, EPC, TId, Userdata, TAGSignature, FastTAGVehicleNo, FastTAGVehicleClassCode, InCreatedBy, BarCodeId, ReceiptId) " +
                        "VALUES (@ParkingLogId, @VehicleTypeId, @TransactionNumber, @VehicleNumber, @CardNo, @EPC, @TId, @Userdata, @TAGSignature, @FastTAGVehicleNo, @FastTAGVehicleClassCode, @InCreatedBy, @BarCodeId, @ReceiptId)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ParkingLogId", park.ParkingLogId);
                    cmd.Parameters.AddWithValue("@VehicleTypeId", park.VehicleTypeId);
                    cmd.Parameters.AddWithValue("@TransactionNumber", park.TransactionNumber);
                    cmd.Parameters.AddWithValue("@VehicleNumber", park.VehicleNumber);
                    cmd.Parameters.AddWithValue("@CardNo", park.CardNo);

                    cmd.Parameters.AddWithValue("@EPC", park.EPC);
                    cmd.Parameters.AddWithValue("@TId", park.TId);
                    cmd.Parameters.AddWithValue("@Userdata", park.Userdata);
                    cmd.Parameters.AddWithValue("@TAGSignature", park.TAGSignature);
                    cmd.Parameters.AddWithValue("@FastTAGVehicleNo", park.FastTAGVehicleNo);
                    cmd.Parameters.AddWithValue("@FastTAGVehicleClassCode", park.FastTAGVehicleClassCode);
                   

                    cmd.Parameters.AddWithValue("@InCreatedBy", park.InCreatedBy);
                    cmd.Parameters.AddWithValue("@BarCodeId", park.BarCodeId);
                    cmd.Parameters.AddWithValue("@ReceiptId", park.ReceiptId);







                    // Execute the SQL command
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }



    }
    }



