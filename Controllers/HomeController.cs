﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Models;
using TheBlogProject.Services;
using TheBlogProject.ViewModels;
using X.PagedList;

namespace TheBlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, 
                              IBlogEmailSender emailSender, 
                              ApplicationDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var blogs = _context.Blogs
                .Include(b => b.BlogUser)
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(await blogs);

        }

        public async Task<IActionResult> BlogPostIndex(int? id)
        {
            if(id is null)
            {
                return NotFound();
            }

            var posts = _context.Posts.Where(p => p.BlogId == id).ToList();
            return View("Index", posts);
        }

        public IActionResult About()
        {
            return View();
        }

        //This refers to the ContactMe page
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Contact(ContactMe model)
        {
            //This is where the email is sent from
            model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
