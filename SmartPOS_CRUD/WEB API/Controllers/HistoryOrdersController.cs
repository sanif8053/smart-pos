using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    public class HistoryOrdersController : ApiController
    {
        private Smart_POS2HistoryOrder db = new Smart_POS2HistoryOrder();

        // GET: api/HistoryOrders
        public IQueryable<HistoryOrder> GetHistoryOrders()
        {
            return db.HistoryOrders;
        }

        // GET: api/HistoryOrders/5
        [ResponseType(typeof(HistoryOrder))]
        public IHttpActionResult GetHistoryOrder(int id)
        {
            HistoryOrder historyOrder = db.HistoryOrders.Find(id);
            if (historyOrder == null)
            {
                return NotFound();
            }

            return Ok(historyOrder);
        }

        // PUT: api/HistoryOrders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHistoryOrder(int id, HistoryOrder historyOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != historyOrder.HistId)
            {
                return BadRequest();
            }

            db.Entry(historyOrder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryOrderExists(id))
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

        // POST: api/HistoryOrders
        [ResponseType(typeof(HistoryOrder))]
        public IHttpActionResult PostHistoryOrder(HistoryOrder historyOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HistoryOrders.Add(historyOrder);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = historyOrder.HistId }, historyOrder);
        }

        // DELETE: api/HistoryOrders/5
        [ResponseType(typeof(HistoryOrder))]
        public IHttpActionResult DeleteHistoryOrder(int id)
        {
            HistoryOrder historyOrder = db.HistoryOrders.Find(id);
            if (historyOrder == null)
            {
                return NotFound();
            }

            db.HistoryOrders.Remove(historyOrder);
            db.SaveChanges();

            return Ok(historyOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HistoryOrderExists(int id)
        {
            return db.HistoryOrders.Count(e => e.HistId == id) > 0;
        }
    }
}