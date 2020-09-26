using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangoServer.Data;
using MangoServer.Model;

namespace MangoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly MangoServerContext _context;

        public UtilisateursController(MangoServerContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateur()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        [HttpGet("login")]
        public async Task<ActionResult<Utilisateur>> Login()
        {

            try
            {
                string nom = Request.Headers["nom"].ToString();
                string mdp = Request.Headers["mdp"].ToString();
                var utilisateur = await _context.Utilisateurs.Where<Utilisateur>(u => u.Identifiant == nom).Include( u => u.Items ).FirstAsync<Utilisateur>();

                if (utilisateur == null)
                {
                    return NotFound();
                }
                else if (mdp == utilisateur.Mdp)
                {
                    return Ok(utilisateur);
                }
                else
                {
                    return Unauthorized("UnAuthorized");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("UtilisateurCotnroller, login()\n" + ex.ToString());
                return NotFound("catch clause");
            }

        }

        // GET: api/Utilisateurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, [FromBody]Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return BadRequest();
            }


            Utilisateur existingUser = await _context.Utilisateurs.Where<Utilisateur>(u => u.Id == id).Include(u => u.Items).FirstAsync<Utilisateur>();
    
            if(existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(utilisateur);


                foreach (Item existingItem in existingUser.Items)
                {
                    if (!utilisateur.Items.Any(c => c.Id == existingItem.Id))
                        _context.Items.Remove(existingItem);
                }
            }

            foreach (Item item in utilisateur.Items)
            {
                Item existingItem = existingUser.Items
                    .Where(c => c.Id == item.Id)
                    .SingleOrDefault();

                if (existingItem != null)
                    // Update child
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                else
                {
                    // Insert child
                    Item newItem = new Item
                    {
                        Titre = item.Titre,
                        Description = item.Description
                    };
                    existingUser.Items.Add(newItem);
                }
            }

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

           



            return NoContent();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Utilisateur>> PostNewItems(int id, [FromBody]Utilisateur utilisateur)
        {

            if (id != utilisateur.Id)
            {
                return BadRequest();
            }

            Utilisateur existingUser = await _context.Utilisateurs.Where<Utilisateur>(u => u.Id == id).Include(u => u.Items).FirstAsync<Utilisateur>();

            foreach (Item item in utilisateur.Items)
            {
                if(item.Id == 0){
                    // Insert child
                    Item newItem = new Item
                    {
                        Titre = item.Titre,
                        Description = item.Description
                    };
                    Console.WriteLine("newItem = " + newItem.Id + " " + newItem.Titre); 
                    existingUser.Items.Add(newItem);

                }

            }

                await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", utilisateur.Id, utilisateur);
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.Id }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Utilisateur>> DeleteUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return utilisateur;
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }
    }
}
