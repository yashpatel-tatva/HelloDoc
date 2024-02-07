﻿using HelloDoc.DataContext;
using HelloDoc.Models;
using HelloDoc.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDoc.Controllers
{
    public class RequestFormsController : Controller
    {
        private readonly HelloDocDbContext _context;
        public RequestFormsController(HelloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult PatientRequest()
        {
            return View();
        }
        public IActionResult FamilyRequest()
        {
            return View();
        }
        public IActionResult ConciergeRequest()
        {
            return View();
        }
        public IActionResult BuisnessPartnerRequest()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Self(User user) 
        {
            _context.Add(user);
            _context.SaveChanges(); 
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
