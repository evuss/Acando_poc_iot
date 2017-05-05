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
    public class SpotsController : ApiController
    {
        public class SpotsAuth{ //Cannot inherit from "Spots" since EF will then use SpotsAuth.
            public string AuthToken { get; set; }
            public string SpotID { get; set; }
            public string SpotName { get; set; }
            public string SensorID { get; set; }
            public string ImageType { get; set; }
            public string XPos { get; set; }
            public string YPos { get; set; }
        }

        private SpotsContext db = new SpotsContext();

        // GET: api/Spots
        public IQueryable<Spots> GetSpots()
        {
            return db.Spots;
        }

        // GET: api/Spots/5
        [ResponseType(typeof(Spots))]
        public async Task<IHttpActionResult> GetSpots(string id)
        {
            Spots spots = await db.Spots.FindAsync(id);
            if (spots == null)
            {
                return NotFound();
            }

            return Ok(spots);
        }

        // PUT: api/Spots/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpots(string id, SpotsAuth spotsAuth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get Spot object from api object
            Spots spots = new Spots(spotsAuth.SpotID, spotsAuth.SpotName, spotsAuth.SensorID, spotsAuth.ImageType, spotsAuth.XPos, spotsAuth.YPos);

            if (id != spots.SpotID)
            {
                return BadRequest();
            }

            // Check for correct AuthToken
            if (CheckAuthToken(spotsAuth.AuthToken) == null) {
                return BadRequest("Incorrect Authtoken");
            }

            // Check if spot alreadt exist and use it's data for missing fields
            Spots oldSpot = await db.Spots.FindAsync(id);
            if(oldSpot != null) {
                if (spots.SpotName == null) {
                    spots.SpotName = oldSpot.SpotName;
                }
                if (spots.SensorID == null) {
                    spots.SensorID = oldSpot.SensorID;
                }
                if (spots.ImageType == null) {
                    spots.ImageType = oldSpot.ImageType;
                }
                if (spots.XPos == null) {
                    spots.XPos = oldSpot.XPos;
                }
                if (spots.YPos == null) {
                    spots.YPos = oldSpot.YPos;
                }
            }

            db.Entry(spots).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Spots
        [ResponseType(typeof(Spots))]
        public async Task<IHttpActionResult> PostSpots(SpotsAuth spotsAuth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get Spot object from api object
            Spots spots = new Spots(spotsAuth.SpotID, spotsAuth.SpotName, spotsAuth.SensorID, spotsAuth.ImageType, spotsAuth.XPos, spotsAuth.YPos);

            // Check for correct AuthToken
            if (CheckAuthToken(spotsAuth.AuthToken) == null) {
                return BadRequest("Incorrect Authtoken");
            }

            // Check any existing record of SpotID
            Spots oldSpot = await db.Spots.FindAsync(spots.SpotID);
            if (oldSpot == null) {  // This is a new spot. Add it.
                db.Spots.Add(spots);

                try {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException) {
                    if (SpotsExists(spots.SpotID)) {
                        return Conflict();
                    }
                    else {
                        throw;
                    }
                }
                return CreatedAtRoute("DefaultApi", new { id = spots.SpotID }, spots);
            }
            else {  // This spot alreay exists. Update with non null fields
                if (spots.SpotName != null) {
                    oldSpot.SpotName = spots.SpotName;
                }
                if (spots.SensorID != null) {
                    oldSpot.SensorID = spots.SensorID;
                }
                if (spots.ImageType != null) {
                    oldSpot.ImageType = spots.ImageType;
                }
                if (spots.XPos != null) {
                    oldSpot.XPos = spots.XPos;
                }
                if (spots.YPos != null) {
                    oldSpot.YPos = spots.YPos;
                }

                db.Entry(oldSpot).State = EntityState.Modified;

                try {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!SpotsExists(spots.SpotID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return CreatedAtRoute("DefaultApi", new { id = oldSpot.SpotID }, oldSpot);
            }

            
        }

        // DELETE: api/Spots/5
        /*[ResponseType(typeof(Spots))]
        public async Task<IHttpActionResult> DeleteSpots(string id)
        {
            Spots spots = await db.Spots.FindAsync(id);
            if (spots == null)
            {
                return NotFound();
            }

            // Check for correct AuthToken
            if (CheckAuthToken(spotsAuth.AuthToken) == null) {
                return BadRequest("Incorrect Authtoken");
            }

            db.Spots.Remove(spots);
            await db.SaveChangesAsync();

            return Ok(spots);
        }*/

        protected string CheckAuthToken(string authToken) {
            string AuthID = null;

            if (authToken != null) {
                AuthID = db.Database.SqlQuery<string>("SELECT AuthID FROM dbo.AuthTokens WHERE AuthToken = @p0", authToken).FirstOrDefault();
            }

            return AuthID;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpotsExists(string id)
        {
            return db.Spots.Count(e => e.SpotID == id) > 0;
        }
    }
}