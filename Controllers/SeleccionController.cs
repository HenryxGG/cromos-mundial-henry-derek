
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cromosmundial_proyecto_final.Models;
using cromosmundial_proyecto_final.Data;

public class SeleccionController : Controller
{
    private readonly AppDBContext _context;

    public SeleccionController(AppDBContext context)
    {
        _context = context;
    }

    // GET: SELECCIONS
    public async Task<IActionResult> Index(string searchString, int page = 1)    
    {
        int pageSize = 10; // Número de elementos por página

        var Selecciones = _context.Selecciones
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            Selecciones = Selecciones.Where(S =>
                S.Nombre.Contains(searchString) ||
                S.Confederacion.Contains(searchString) ||
                S.DirectorTecnico.Contains(searchString)
            );
        }

        int totalSeleccion = await Selecciones.CountAsync(); // Total de elementos después de aplicar el filtro

        var seleccion2 = await Selecciones
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); // Aplica paginación

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalSeleccion / (double)pageSize);

        if (!seleccion2.Any() && !string.IsNullOrWhiteSpace(searchString)) // Verifica si la lista está vacía después de aplicar el filtro
        {
            ViewBag.Mensaje = $"No se encontraron resultados para '{searchString}'. Puedes agregarlo."; // Mensaje para la vista si no se encuentran resultados
        }

        return View(seleccion2); // Asegura que se devuelva la lista de cromos a la vista
    }

    // GET: SELECCIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seleccion = await _context.Selecciones
            .FirstOrDefaultAsync(m => m.Id == id);
        if (seleccion == null)
        {
            return NotFound();
        }

        return View(seleccion);
    }

    // GET: SELECCIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SELECCIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Confederacion,DirectorTecnico,Cromos")] Seleccion seleccion)
    {
        if (ModelState.IsValid)
        {
            _context.Add(seleccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(seleccion);
    }

    // GET: SELECCIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seleccion = await _context.Selecciones.FindAsync(id);
        if (seleccion == null)
        {
            return NotFound();
        }
        return View(seleccion);
    }

    // POST: SELECCIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Confederacion,DirectorTecnico,Cromos,RutaFoto,ArchivoFoto")] Seleccion seleccion)
    {
        if (id != seleccion.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                if (seleccion.ArchivoFoto != null && seleccion.ArchivoFoto.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagenes");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + seleccion.ArchivoFoto.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await seleccion.ArchivoFoto.CopyToAsync(fileStream);
                    }
                    seleccion.RutaFoto = "/Imagenes/" + uniqueFileName;
                }
                _context.Update(seleccion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeleccionExists(seleccion.Id))
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
        return View(seleccion);
    }

    // GET: SELECCIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var seleccion = await _context.Selecciones
            .FirstOrDefaultAsync(m => m.Id == id);
        if (seleccion == null)
        {
            return NotFound();
        }

        return View(seleccion);
    }

    // POST: SELECCIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var seleccion = await _context.Selecciones.FindAsync(id);
        if (seleccion != null)
        {
            _context.Selecciones.Remove(seleccion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Jugadores(int id)
    {
        var seleccion = await _context.Selecciones
            .Include(s => s.Cromos)
            .ThenInclude(c => c.Jugador)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (seleccion == null)
        {
            return NotFound();
        }

        return View(seleccion);
    }

    private bool SeleccionExists(int? id)
    {
        return _context.Selecciones.Any(e => e.Id == id);
    }
}
