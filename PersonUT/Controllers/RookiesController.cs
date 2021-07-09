using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonUT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using PersonUT.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace PersonUT.Controllers
{
    public class RookiesController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ILogger<RookiesController> _logger;
        public RookiesController(ILogger<RookiesController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }


        public IActionResult Index()
        {
            try
            {
                var listMember = _personService.GetList();
                return View(listMember);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _personService.Create(member);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string email)
        {
            var findMember = _personService.Detail(email);
            return View(findMember);
        }
        [HttpPost]
        public IActionResult Edit(Person member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _personService.Edit(member);
            return RedirectToAction("Index");
        }


        public IActionResult Detail(string email)
        {
            var detailMember = _personService.Detail(email);
            if (detailMember == null) return NotFound();
            return View(detailMember);
        }

        public IActionResult Delete(string email)
        {
            var existing = _personService.Detail(email);
            if (existing == null) return NotFound();
            _personService.Delete(email);
            return RedirectToAction("Index");

        }

    }

}