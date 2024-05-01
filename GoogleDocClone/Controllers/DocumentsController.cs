using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoogleDocClone.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GoogleDocClone.Entities;
using GoogleDocClone.Services;

namespace GoogleDocClone.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsController(IDocumentsRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var documents = await _documentsRepository.GetUsersDocuments(userID);
            return View(documents);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("Id,Title,Content,UserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                await _documentsRepository.AddDocument(document);
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentsRepository.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            if (document.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _documentsRepository.UpdateDocumentAsync(document);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_documentsRepository.DocumentExists(document.Id))
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
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var document = await _documentsRepository.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }
            if (document.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _documentsRepository.FindAsync(id);
            await _documentsRepository.DeleteDocumentAsync(document);
            return RedirectToAction(nameof(Index));
        }
    }
}
