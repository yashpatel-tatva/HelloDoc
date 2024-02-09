using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelloDoc.DataContext;
using HelloDoc.DataModels;

namespace HelloDoc.Controllers
{
    public class RequestsController : Controller
    {
        private readonly HelloDocDbContext _context;

        public RequestsController(HelloDocDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var helloDocDbContext = _context.Requests.Include(r => r.Physician).Include(r => r.Requesttype).Include(r => r.User);
            return View(await helloDocDbContext.ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Physician)
                .Include(r => r.Requesttype)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Requestid == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["Physicianid"] = new SelectList(_context.Physicians, "Physicianid", "Businessname");
            ViewData["Requesttypeid"] = new SelectList(_context.Requesttypes, "Requesttypeid", "Name");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Aspnetuserid");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Requestid,Requesttypeid,Userid,Firstname,Lastname,Phonenumber,Email,Status,Physicianid,Confirmationnumber,Createddate,Isdeleted,Modifieddate,Declinedby,Isurgentemailsent,Lastwellnessdate,Ismobile,Calltype,Completedbyphysician,Lastreservationdate,Accepteddate,Relationname,Casenumber,Ip,Casetag,Casetagphysician,Patientaccountid,Createduserid")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Physicianid"] = new SelectList(_context.Physicians, "Physicianid", "Businessname", request.Physicianid);
            ViewData["Requesttypeid"] = new SelectList(_context.Requesttypes, "Requesttypeid", "Name", request.Requesttypeid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Aspnetuserid", request.Userid);
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewData["Physicianid"] = new SelectList(_context.Physicians, "Physicianid", "Businessname", request.Physicianid);
            ViewData["Requesttypeid"] = new SelectList(_context.Requesttypes, "Requesttypeid", "Name", request.Requesttypeid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Aspnetuserid", request.Userid);
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Requestid,Requesttypeid,Userid,Firstname,Lastname,Phonenumber,Email,Status,Physicianid,Confirmationnumber,Createddate,Isdeleted,Modifieddate,Declinedby,Isurgentemailsent,Lastwellnessdate,Ismobile,Calltype,Completedbyphysician,Lastreservationdate,Accepteddate,Relationname,Casenumber,Ip,Casetag,Casetagphysician,Patientaccountid,Createduserid")] Request request)
        {
            if (id != request.Requestid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Requestid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Physicianid"] = new SelectList(_context.Physicians, "Physicianid", "Businessname", request.Physicianid);
            ViewData["Requesttypeid"] = new SelectList(_context.Requesttypes, "Requesttypeid", "Name", request.Requesttypeid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Aspnetuserid", request.Userid);
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Physician)
                .Include(r => r.Requesttype)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Requestid == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'HelloDocDbContext.Requests'  is null.");
            }
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
          return (_context.Requests?.Any(e => e.Requestid == id)).GetValueOrDefault();
        }
    }
}
