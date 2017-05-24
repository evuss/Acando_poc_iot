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
//using System.Web.Http.Cors;

namespace AzureWebAPI1.Controllers
{
    public class SpotStatesController : ApiController
    {
        private SpotStateContext db = new SpotStateContext();

        
        // GET: api/SpotStates
        public IQueryable<SpotState> GetSpotStates()
        {
            return db.SpotStates;
        }
        
        // GET: api/SpotStates/5
        [ResponseType(typeof(SpotState))]
       // [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetSpotState(string id)//
        {
            SpotState spotState = await db.SpotStates.Where(x => x.TS == db.SpotStates.Max(p => p.TS)).FirstAsync();
            if (spotState == null)
            {
                return NotFound();
            }

            return Ok(spotState);
            //return CreatedAtRoute("DefaultApi", new { id = spotState }, spotState);
        }
        /*
        // PUT: api/SpotStates/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpotState(string id, SpotState spotState)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            if (id != spotState.SpotID)
            {
                return BadRequest();
            }

            db.Entry(spotState).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotStateExists(id))
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

        // POST: api/SpotStates
        [ResponseType(typeof(SpotState))]
        public async Task<IHttpActionResult> PostSpotState(SpotState spotState)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpotStates.Add(spotState);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpotStateExists(spotState.SpotID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = spotState.SpotID }, spotState);
        }

        // DELETE: api/SpotStates/5
        [ResponseType(typeof(SpotState))]
        public async Task<IHttpActionResult> DeleteSpotState(string id)
        {
            SpotState spotState = await db.SpotStates.FindAsync(id);
            if (spotState == null)
            {
                return NotFound();
            }

            db.SpotStates.Remove(spotState);
            await db.SaveChangesAsync();

            return Ok(spotState);
        }

        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpotStateExists(string id)
        {
            return db.SpotStates.Count(e => e.ID == id) > 0;
        }
    }
}