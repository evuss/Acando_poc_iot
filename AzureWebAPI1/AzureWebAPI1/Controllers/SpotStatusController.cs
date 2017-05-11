using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AzureWebAPI1.Models;


namespace AzureWebAPI1.Controllers
{
    public class SpotStatusController : ApiController
    {
        private SpotStateContext db = new SpotStateContext();

        public class TSQuery {
            public string TS { get; set; }
            public string TimeToDie { get; set; }
        }

        /*// GET: api/SpotStatus
        public IQueryable<SpotStatus> GetSpotStatus()
        {
            return db.SpotStatus;
        }*/

        // GET: api/SpotStatus/5
        /*[ResponseType(typeof(SpotStatus))]
        public async Task<IHttpActionResult> GetSpotStatus(string id)
        {
            SpotStatus spotStatus = await db.SpotStatus.FindAsync(id);
            if (spotStatus == null)
            {
                return NotFound();
            }

            return Ok(spotStatus);
        }*/

        // PUT: api/SpotStatus/5
        /*[ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpotStatus(string id, SpotStatus spotStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != spotStatus.TS)
            {
                return BadRequest();
            }

            db.Entry(spotStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/SpotStatus
        [ResponseType(typeof(SpotStatus))]
        public async Task<IHttpActionResult> PostSpotStatus(TSQuery tsQuery)
        {
            int iTemp = 0;
            string  inTS = "",
                    inTSdt,
                    outTS,
                    defaultTS = "19010101000000",
                    sTemp;
            DateTime dtTemp;

            SpotStatus spotStatus = new SpotStatus();

            // Comvert incoming TS data time string to the format "YYYY-MM-DD 00:00:00"
            if (tsQuery.TS == null || tsQuery.TS.Length != 14 || tsQuery.TS.CompareTo(defaultTS) < 0) {
                inTS = defaultTS;
            }
            else {
                inTS = tsQuery.TS;
            }
            inTSdt = inTS.Substring(0, 4) + "-" + inTS.Substring(4, 2) + "-" + inTS.Substring(6, 2) + " " +    //Date
                           inTS.Substring(8, 2) + ":" + inTS.Substring(10, 2) + ":" + inTS.Substring(12, 2);   //Time

            // Create Timestamp for fetch with 20s overlap (To handle potential lag)
            dtTemp = DateTime.UtcNow;
            //dtTemp = db.GetDbUtcTimestamp();
            outTS = dtTemp.AddSeconds(-20).ToString("yyyyMMddHHmmss");
            if (outTS.CompareTo(inTS) < 0) {    // Make sure Xs earlier is not before this fetch (should never happen if client updates are every 60s)
                outTS = inTS;
            }

            spotStatus.TS = outTS;
            spotStatus.Result = "Api reached";

            if(tsQuery.TimeToDie == null || tsQuery.TimeToDie == "") {
                spotStatus.SpotStates = db.GetSpotStateChangesSinceTS(inTSdt);
                //spotStatus.SpotStates = db.GetSpotStateChangesSinceTS("1900-01-01 00:00:00");
            }
            else {
                spotStatus.SpotStates = db.GetSpotStateChangesSinceTS(inTSdt, tsQuery.TimeToDie);
            }

            return CreatedAtRoute("DefaultApi", new { id = spotStatus.TS }, spotStatus);
        }

        /*// DELETE: api/SpotStatus/5
        [ResponseType(typeof(SpotStatus))]
        public async Task<IHttpActionResult> DeleteSpotStatus(string id)
        {
            SpotStatus spotStatus = await db.SpotStatus.FindAsync(id);
            if (spotStatus == null)
            {
                return NotFound();
            }

            db.SpotStatus.Remove(spotStatus);
            await db.SaveChangesAsync();

            return Ok(spotStatus);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       /* private bool SpotStatusExists(string id)
        {
            return db.SpotStatus.Count(e => e.TS == id) > 0;
        }*/
    }
}