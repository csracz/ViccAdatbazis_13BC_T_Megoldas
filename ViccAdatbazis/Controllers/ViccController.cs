using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViccController : ControllerBase
    {
        private readonly ViccDbContext _context;
        public ViccController(ViccDbContext context)
        {
            _context = context;
        }

        //Viccek lekérdezése
        [HttpGet]
        public async Task<ActionResult<List<Vicc>>> GetViccek()
        {
            return await _context.Viccek.Where(x => x.Aktiv).ToListAsync();
        }

        //Vicc lekérdezése
        [HttpGet("{id}")]
        public async Task<ActionResult<Vicc>> GetVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            return vicc == null ? NotFound() : vicc;
        }

        //Új vicc feltöltése
        [HttpPost]
        public async Task<ActionResult> PostVicc(Vicc ujVicc)
        {
            _context.Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*public async Task<ActionResult<Vicc>> PostVicc(Vicc ujVicc)
        {
            _context.Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVicc", new {id = ujVicc.Id}, ujVicc);
        }*/

        //Vicc módosítása
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVicc(int id, Vicc modositottVicc)
        {
            if (id != modositottVicc.Id)
            {
                return BadRequest();
            }
            _context.Entry(modositottVicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Vicc törlése
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            var torlendoVicc = _context.Viccek.Find(id);
            if(torlendoVicc == null)
                return NotFound();
            if (torlendoVicc.Aktiv)
            {
                torlendoVicc.Aktiv = false;
                _context.Entry(torlendoVicc).State = EntityState.Modified;
            }
            else
                _context.Viccek.Remove(torlendoVicc);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Lájkolás
        [Route("{id}/like")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> LikeVicc(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null) return NotFound();
            vicc.Tetszik++;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Dislájkolás
        [Route("{id}/dislike")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> DisLikeVicc(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null) return NotFound();
            vicc.NemTetszik++;
            _context.Entry(vicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
