
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cromosmundial_proyecto_final.Models;
using cromosmundial_proyecto_final.Data;

public class JugadorController : Controller
{
    private readonly AppDBContext _context;

    public JugadorController(AppDBContext context)
    {
        _context = context;
    }

    // GET: JUGADORS
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        int pageSize = 10; // Número de elementos por página

        var jugador = _context.Jugadores
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            jugador = jugador.Where(j =>
                j.Nombre.Contains(searchString) ||
                j.FechaNacimiento.Year.ToString().Contains(searchString) ||
                j.Nacionalidad.Contains(searchString)
            );
        }

        int totalJugador = await jugador.CountAsync(); // Total de elementos después de aplicar el filtro

        var jugador2 = await jugador
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); // Aplica paginación

        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = (int)Math.Ceiling(totalJugador / (double)pageSize);

        if (!jugador2.Any() && !string.IsNullOrWhiteSpace(searchString)) // Verifica si la lista está vacía después de aplicar el filtro
        {
            ViewBag.Mensaje = $"No se encontraron resultados para '{searchString}'. Puedes agregarlo."; // Mensaje para la vista si no se encuentran resultados
        }

        return View(jugador2); // Asegura que se devuelva la lista de cromos a la vista
    }
    // GET: JUGADORS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Cargamos el jugador incluyendo su cromo asociado
        var jugador = await _context.Jugadores
            .Include(j => j.Cromos)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // GET: JUGADORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: JUGADORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,FechaNacimiento,Nacionalidad,Historia")] Jugador jugador)
    {
        if (ModelState.IsValid)
        {
            _context.Add(jugador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(jugador);
    }

    // GET: JUGADORS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores.FindAsync(id);
        if (jugador == null)
        {
            return NotFound();
        }
        return View(jugador);
    }

    // POST: JUGADORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,FechaNacimiento,Nacionalidad,Historia")] Jugador jugador)
    {
        if (id != jugador.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(jugador);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JugadorExists(jugador.Id))
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
        return View(jugador);
    }

    // GET: JUGADORS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        // Cargamos el jugador incluyendo su cromo asociado
        var jugador = await _context.Jugadores
            .Include(j => j.Cromos)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // POST: JUGADORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var jugador = await _context.Jugadores.FindAsync(id);
        if (jugador != null)
        {
            _context.Jugadores.Remove(jugador);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool JugadorExists(int? id)
    {
        return _context.Jugadores.Any(e => e.Id == id);
    }
}
