    
using cromosmundial_proyecto_final.Data;
using cromosmundial_proyecto_final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CromoController : Controller
{
    private readonly AppDBContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CromoController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: CROMOS
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 10; // Número de elementos por página

        var cromos = _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Seleccion)
            .Include(c => c.Mundial) // Asegura que se carguen los datos relacionados de
            .AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            cromos = cromos.Where(c => c.Jugador.Nombre.Contains(searchString) ||
                                       c.Seleccion.Nombre.Contains(searchString) ||
                                       c.Mundial.Nombre.Contains(searchString));
        }

        int totalCromo = await cromos.CountAsync(); // Total de elementos después de aplicar el filtro

        var cromo2 = await cromos
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); // Aplica paginación

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalCromo / (double)pageSize);

        if (!cromo2.Any() && !string.IsNullOrWhiteSpace(searchString)) // Verifica si la lista está vacía después de aplicar el filtro
        {
            ViewBag.Mensaje = $"No se encontraron resultados para '{searchString}'. Puedes agregarlo."; // Mensaje para la vista si no se encuentran resultados
        }

        return View(cromo2); // Asegura que se devuelva la lista de cromos a la vista
    }

    // GET: CROMOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Seleccion)
            .Include(c => c.Mundial)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }

        return View(cromo);
    }

    // GET: CROMOS/Create
    public IActionResult Create()
    {
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");
        ViewData["SeleccionesId"] = new SelectList(_context.Selecciones, "Id", "Nombre");
        ViewData["MundialesId"] = new SelectList(_context.Mundiales, "Id", "Nombre");

        return View();
    }

    // POST: CROMOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cromo cromo)
    {
        if (ModelState.IsValid)
        {
            if (cromo.ArchivoFoto != null && cromo.ArchivoFoto.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Imagenes");

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(cromo.ArchivoFoto.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cromo.ArchivoFoto.CopyToAsync(stream);
                }

                cromo.RutaFoto = "/Imagenes/" + uniqueFileName;
            }

            _context.Add(cromo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(cromo);
    }

    // GET: CROMOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
           .Include(c => c.Jugador)
           .Include(c => c.Seleccion)
           .Include(c => c.Mundial)
           .FirstOrDefaultAsync(c => c.Id == id); 

        if (cromo == null)
        {
            return NotFound();
        }
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre", cromo.JugadorId);
        ViewData["SeleccionesId"] = new SelectList(_context.Selecciones, "Id", "Nombre", cromo.SeleccionId);
        ViewData["MundialesId"] = new SelectList(_context.Mundiales, "Id", "Nombre", cromo.MundialId);

        return View(cromo);
    }

    // POST: CROMOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cromo cromo)
    {
        if (id != cromo.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var cromoExistente = await _context.Cromos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                // Si sube nueva imagen
                if (cromo.ArchivoFoto != null && cromo.ArchivoFoto.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Imagenes");

                    string uniqueFileName =
                        Guid.NewGuid().ToString() + Path.GetExtension(cromo.ArchivoFoto.FileName);

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cromo.ArchivoFoto.CopyToAsync(stream);
                    }

                    cromo.RutaFoto = "/Imagenes/" + uniqueFileName;
                }
                else
                {
                    // mantener imagen anterior
                    cromo.RutaFoto = cromoExistente?.RutaFoto;
                }

                _context.Update(cromo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CromoExists(cromo.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre", cromo.JugadorId);
        ViewData["SeleccionesId"] = new SelectList(_context.Selecciones, "Id", "Nombre", cromo.SeleccionId);
        ViewData["MundialesId"] = new SelectList(_context.Mundiales, "Id", "Nombre", cromo.MundialId);

        return View(cromo);
    }

    // GET: CROMOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Seleccion)
            .Include(c => c.Mundial)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }


        return View(cromo);
    }

    // POST: CROMOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo != null)
        {
            _context.Cromos.Remove(cromo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CromoExists(int? id)
    {
        return _context.Cromos.Any(e => e.Id == id);
    }
}
