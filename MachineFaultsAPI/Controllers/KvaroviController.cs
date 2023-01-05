using Dapper;
using MachineFaultsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MachineFaultsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KvaroviController : Controller
{
    // GET: api/kvarovi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Kvar>>> GetKvarovi(int pageSize = 10, int pageNumber = 1)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var offset = (pageNumber - 1) * pageSize;
            var kvarovi = await connection.QueryAsync<Kvar>(
                "SELECT * FROM kvarovi ORDER BY prioritet ASC, vrijeme_pocetka DESC LIMIT @PageSize OFFSET @Offset",
                new { PageSize = pageSize, Offset = offset }
            );
            return Ok(kvarovi);
        }
    }

    // GET: api/kvarovi/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Kvar>> GetKvar(int id)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var kvar = await connection.QueryFirstOrDefaultAsync<Kvar>("SELECT * FROM kvarovi WHERE id = @Id", new { Id = id });
            if (kvar == null)
            {
                return NotFound();
            }
            return Ok(kvar);
        }
    }
    
    // POST: api/kvarovi
    [HttpPost]
    public async Task<ActionResult<Kvar>> PostKvar(Kvar kvar)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            
            Console.WriteLine("Naziv stroja " + kvar.Naziv_Stroja);
            
            if (string.IsNullOrEmpty(kvar.Opis))
            {
                return BadRequest("Opis kvara je obavezan");
            }

            var existingActiveKvar = await connection.QueryFirstOrDefaultAsync<Kvar>(
                "SELECT * FROM kvarovi WHERE naziv_stroja = @NazivStroja AND status = 'neotklonjen'",
                new { NazivStroja = kvar.Naziv_Stroja }
            );
            if (existingActiveKvar != null)
            {
                return Conflict("Postoji aktivan kvar na stroju");
            }

            var rowsAffected = await connection.ExecuteAsync(
                "INSERT INTO kvarovi (id_stroja, naziv_stroja, naziv,prioritet, vrijeme_pocetka, vrijeme_zavrsetka, opis, status) " +
                "VALUES (@Id_Stroja, @NazivStroja, @Naziv, @Prioritet, @VrijemePocetka,@VrijemeZavršetka, @Opis, @Status)",
                new
                {
                    Id_Stroja = kvar.Id_Stroja,
                    NazivStroja = kvar.Naziv_Stroja,
                    Naziv = kvar.Naziv,
                    Prioritet = kvar.Prioritet,
                    VrijemePocetka = kvar.Vrijeme_Pocetka,
                    VrijemeZavršetka = kvar.Vrijeme_Zavrsetka,
                    Opis = kvar.Opis,
                    Status = kvar.Status
                }
            );
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to add kvar");
            }

            return CreatedAtAction(nameof(GetKvar), new { id = kvar.Id }, kvar);
        }
    }

    // DELETE: api/kvarovi/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Kvar>> DeleteKvar(int id)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var kvar = await connection.QueryFirstOrDefaultAsync<Kvar>("SELECT * FROM kvarovi WHERE id = @Id", new { Id = id });
            if (kvar == null)
            {
                return NotFound();
            }

            var rowsAffected = await connection.ExecuteAsync("DELETE FROM kvarovi WHERE id = @Id", new { Id = id });
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to delete kvar");
            }

            return Ok(kvar);
        }
    }

    // PUT: api/kvarovi/5
    [HttpPut("{id}/status")]
    public async Task<ActionResult<Kvar>> PutKvarStatus(int id,
        [FromBody] Kvar kvar)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            // Dohvatite postojeći kvar iz baze
            var existingKvar =
                await connection.QueryFirstOrDefaultAsync<Kvar>(
                    "SELECT * FROM kvarovi WHERE id = @Id", new { Id = id });
            if (existingKvar == null)
            {
                return NotFound();
            }

            // Mapirajte samo potrebne podatke (status i vrijeme završetka)
            existingKvar.Status = kvar.Status;
            existingKvar.Vrijeme_Zavrsetka = kvar.Vrijeme_Zavrsetka;

            // Izvršite ažuriranje kvara u bazi
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE kvarovi SET status = @Status, vrijeme_zavrsetka = @VrijemeZavršetka WHERE id = @Id",
                new
                {
                    Status = existingKvar.Status,
                    VrijemeZavršetka = existingKvar.Vrijeme_Zavrsetka,
                    Id = id
                });
            if (rowsAffected == 0)
            {
                return NotFound();
            }
            return existingKvar;
        }
    }


    // PATCH: api/kvarovi/5
    [HttpPatch("{id}")]
    public async Task<ActionResult<Kvar>> PatchKvar(int id, Kvar kvar)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var existingKvar = await connection.QueryFirstOrDefaultAsync<Kvar>("SELECT * FROM kvarovi WHERE id = @Id", new { Id = id });
            if (existingKvar == null)
            {
                return NotFound();
            }
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE kvarovi SET status = @Status WHERE id = @Id",
                new { Status = kvar.Status, Id = id }
            );
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to update kvar");
            }

            return Ok(kvar);
        }
    }
}