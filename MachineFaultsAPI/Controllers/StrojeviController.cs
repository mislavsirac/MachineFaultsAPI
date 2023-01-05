using Dapper;
using MachineFaultsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MachineFaultsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StrojeviController : Controller
{
    // GET
    // GET: api/strojevi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stroj>>> GetStrojevi()
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var strojevi = await connection.QueryAsync<Stroj>("SELECT * FROM strojevi");
            return Ok(strojevi);
        }
    }

    // GET: api/strojevi/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Stroj>> GetStroj(int id)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var stroj = await connection.QueryFirstOrDefaultAsync<Stroj>("SELECT * FROM strojevi WHERE id = @Id", new { Id = id });
            if (stroj == null)
            {
                return NotFound();
            }
            return Ok(stroj);
        }
    }

    // POST: api/strojevi
    [HttpPost]
    public async Task<ActionResult<Stroj>> PostStroj(Stroj stroj)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var existingStroj = await connection.QueryFirstOrDefaultAsync<Stroj>("SELECT * FROM strojevi WHERE naziv = @Naziv", new { Naziv = stroj.Naziv });
            if (existingStroj != null)
            {
                return Conflict();
            }

            var rowsAffected = await connection.ExecuteAsync(
                "INSERT INTO strojevi (naziv, prosjecno_trajanje_kvarova) VALUES (@Naziv, @ProsjecnoTrajanjeKvarova)", 
                new { Naziv = stroj.Naziv, ProsjecnoTrajanjeKvarova = stroj.Prosjecno_Trajanje_Kvarova }
            );
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to add stroj");
            }

            return CreatedAtAction(nameof(GetStroj), new { id = stroj.Id }, stroj);
        }
    }
    
    // DELETE: api/strojevi/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Stroj>> DeleteStroj(int id)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var stroj = await connection.QueryFirstOrDefaultAsync<Stroj>("SELECT * FROM strojevi WHERE id = @Id", new { Id = id });
            if (stroj == null)
            {
                return NotFound();
            }

            var rowsAffected = await connection.ExecuteAsync("DELETE FROM strojevi WHERE id = @Id", new { Id = id });
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to delete stroj");
            }

            return Ok(stroj);
        }
    }

    // PUT: api/strojevi/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Stroj>> PutStroj(int id, Stroj stroj)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            var existingStroj = await connection.QueryFirstOrDefaultAsync<Stroj>("SELECT * FROM strojevi WHERE id = @Id", new { Id = id });
            if (existingStroj == null)
            {
                return NotFound();
            }

            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE strojevi SET naziv = @Naziv, prosjecno_trajanje_kvarova = @ProsjecnoTrajanjeKvarova WHERE id = @Id", 
                new { Naziv = stroj.Naziv, ProsjecnoTrajanjeKvarova = stroj
                .Prosjecno_Trajanje_Kvarova, Id = id }
            );
            if (rowsAffected == 0)
            {
                return StatusCode(500, "Unable to update stroj");
            }

            return Ok(stroj);
        }
    }
    
    // GET: api/strojevi/{id}/kvarovi
    [HttpGet("{id}/kvarovi")]
    public async Task<ActionResult<Stroj>> GetKvaroviByStroj(int id)
    {
        using (var connection = DbConnection.DbConnection.GetConnection())
        {
            // Dohvatite sve kvarove za određeni stroj iz baze
            var kvarovi = await connection.QueryAsync<Kvar>("SELECT * FROM kvarovi WHERE id_stroja = @Id", new { Id = id });

            // Izračunajte prosječno vrijeme trajanja kvarova
            TimeSpan totalVrijemeTrajanja = TimeSpan.Zero;
            foreach (var kvar in kvarovi)
            {
                TimeSpan vrijemeTrajanja = (TimeSpan)(kvar.Vrijeme_Zavrsetka - kvar.Vrijeme_Pocetka);
                totalVrijemeTrajanja += vrijemeTrajanja;
            }
            double prosjecnoVrijemeTrajanja = TimeSpan.FromTicks
            (totalVrijemeTrajanja.Ticks / kvarovi.Count()).TotalHours;

            // Spremite prosječno vrijeme trajanja kvarova u bazu podataka
            var sql = "UPDATE strojevi SET prosjecno_trajanje_kvarova = @ProsjekTrajanja WHERE id = @IdStroja";
            connection.Execute(sql, new { ProsjekTrajanja = prosjecnoVrijemeTrajanja, IdStroja = id });

            // Dohvatite podatke o stroju iz baze podataka
            sql = "SELECT * FROM strojevi WHERE id = @IdStroja";
            var stroj = connection.QuerySingleOrDefault<Stroj>(sql, new { IdStroja = id });

            // Vratite podatke o stroju
            return stroj;

        }
    }



}
