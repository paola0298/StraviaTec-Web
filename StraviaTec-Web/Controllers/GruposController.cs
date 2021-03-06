using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StraviaTec_Web.Helpers;
using StraviaTec_Web.Models;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GruposController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Grupos
        [HttpGet]
        public async Task<ActionResult> GetGrupo()
        {
            var grupos = await _context.Grupo.FromSqlInterpolated(
                $@"SELECT ""Id"", ""Nombre"", ""Id_admin"" FROM ""GRUPO""").ToListAsync();
            return Ok(grupos);
            // return await _context.Grupo.ToListAsync();
        }

        // GET: api/Grupos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grupo>> GetGrupo(int id)
        {
            // var grupo = await _context.Grupo.FindAsync(id);
            var grupo = await _context.Grupo.FromSqlInterpolated(
                $@"SELECT ""Id"", ""Nombre"", ""Id_admin"" 
                FROM ""GRUPO"" WHERE ""Id"" = {id}").FirstOrDefaultAsync();


            if (grupo == null)
            {
                return NotFound();
            }

            return grupo;
        }

        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<Grupo>> GetUserGrupo(string idUser)
        {
            var grupos = await _context.Grupo.FromSqlInterpolated(
                $@"SELECT ""Id"", ""Nombre"", ""Id_admin"" 
                FROM ""GRUPO""
                WHERE ""Id_admin"" = {idUser}
                ").ToListAsync();
            
            return Ok(grupos);

        }

        // PUT: api/Grupos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(int id, Grupo grupo)
        {
            if (id != grupo.Id)
            {
                return BadRequest();
            }

            // _context.Entry(grupo).State = EntityState.Modified;
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""GRUPO""
                SET ""Nombre"" = {grupo.Nombre}, ""Id_admin"" = {grupo.IdAdmin}
                WHERE ""Id"" = {grupo.Id}");

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
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

        // POST: api/Grupos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Grupo>> PostGrupo(Grupo grupo)
        {
            if (grupo == null || string.IsNullOrWhiteSpace(grupo.IdAdmin) || string.IsNullOrWhiteSpace(grupo.Nombre)) {
                 return BadRequest(new ErrorInfo(ErrorCode.BadRequest, "El id del admin y nombre del grupo no pueden ser null."));
            }

            // _context.Grupo.FromSqlInterpolated($"INSERT INTO \"GRUPO\" (\"Nombre\", \"Id_admin\") VALUES ({grupo.Nombre}, {grupo.IdAdmin});");
            await _context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO \"GRUPO\" (\"Nombre\", \"Id_admin\") VALUES ({grupo.Nombre}, {grupo.IdAdmin})");
            var grupoA = await _context.Grupo.FromSqlInterpolated(
                $@"SELECT ""Id"", ""Nombre"", ""Id_admin"" FROM ""GRUPO"" WHERE ""Nombre"" = {grupo.Nombre} AND ""Id_admin"" = {grupo.IdAdmin}").FirstOrDefaultAsync();
            await _context.Database.ExecuteSqlInterpolatedAsync($@"INSERT INTO ""USUARIO_GRUPO"" (""Id_grupo"", ""Id_usuario"") VALUES ({grupoA.Id}, {grupo.IdAdmin})");
            // _context.Grupo.Add(grupo);
            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                return StatusCode(500);
            }

            return Ok(grupoA);
        }

        // DELETE: api/Grupos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Grupo>> DeleteGrupo(int id)
        {
            // var grupo = await _context.Grupo.FindAsync(id);
            var grupo = await _context.Grupo.FromSqlInterpolated(
                $@"SELECT ""Id"", ""Nombre"", ""Id_admin"" FROM ""GRUPO"" WHERE ""Id"" = {id}").FirstOrDefaultAsync();
            if (grupo == null)
            {
                return NotFound();
            }

            // _context.Grupo.Remove(grupo);
            await _context.Database.ExecuteSqlInterpolatedAsync($@"DELETE FROM ""GRUPO"" WHERE ""Id"" = {id}");
            await _context.SaveChangesAsync();

            return grupo;
        }

        private bool GrupoExists(int id)
        {
            return _context.Grupo.FromSqlInterpolated($@"SELECT ""Id"", ""Nombre"", ""Id_admin"" FROM ""GRUPO"" WHERE ""Id"" = {id}").Any();
            // return _context.Grupo.Any(e => e.Id == id);
        }
    }
}
