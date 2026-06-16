
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cromosmundial_proyecto_final.Models;
using cromosmundial_proyecto_final.Data;

public class MundialController : Controller
{
    private readonly AppDBContext _context;

    public MundialController(AppDBContext context)
    {
        _context = context;
    }

    // GET: MUNDIALS
    public async Task<IActionResult> Index(string searchString)    
    {

        var Mundiales = _context.Mundiales
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            Mundiales = Mundiales.Where(j =>
                j.Nombre.Contains(searchString) ||
                j.Anio.ToString().Contains(searchString) ||
                j.PaisSede.Contains(searchString)

            );
        }

        ViewData["CurrentFilter"] = searchString;

        var listaMundial = await Mundiales.ToListAsync();

        if (!listaMundial.Any() && !string.IsNullOrWhiteSpace(searchString))
        {
            ViewBag.Mensaje = $"No se encuentra el mundial '{searchString}'. Puedes agregarlo.";
        }

       return View(listaMundial);

    }

    // GET: MUNDIALS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mundial = await _context.Mundiales
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mundial == null)
        {
            return NotFound();
        }

        return View(mundial);
    }

    // GET: MUNDIALS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MUNDIALS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Anio,PaisSede,Cromos")] Mundial mundial)
    {
        if (ModelState.IsValid)
        {
            _context.Add(mundial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(mundial);
    }

    // GET: MUNDIALS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mundial = await _context.Mundiales.FindAsync(id);
        if (mundial == null)
        {
            return NotFound();
        }
        return View(mundial);
    }

    // POST: MUNDIALS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Anio,PaisSede,Cromos")] Mundial mundial)
    {
        if (id != mundial.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mundial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MundialExists(mundial.Id))
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
        return View(mundial);
    }

    // GET: MUNDIALS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mundial = await _context.Mundiales
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mundial == null)
        {
            return NotFound();
        }

        return View(mundial);
    }

    // POST: MUNDIALS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var mundial = await _context.Mundiales.FindAsync(id);
        if (mundial != null)
        {
            _context.Mundiales.Remove(mundial);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MundialExists(int? id)
    {
        return _context.Mundiales.Any(e => e.Id == id);
    }
}
