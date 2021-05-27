using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaiVoaApi.Data;
using VaiVoaApi.Models;

namespace VaiVoaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pessoas/5
        [HttpGet("{email}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(string email)
        {
            var pessoa = await _context.Pessoas
                .Include(c => c.Cartoes.OrderBy(o => o.Id))
                .FirstOrDefaultAsync(b => b.Email == email);

            if (pessoa == null)
            {
                return NotFound();
            }
            return pessoa;
        }

        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            var p = await _context.Pessoas.FirstOrDefaultAsync(b => b.Email == pessoa.Email);
            if (p == null)
            {
                _context.Pessoas.Add(pessoa);
                await _context.SaveChangesAsync();
                p = await _context.Pessoas.FirstOrDefaultAsync(b => b.Email == pessoa.Email);
            }

            Random rand = new Random();
            Cartao c = new Cartao();
            c.NumberCard = _context.GenerateCardNumber();
            c.PessoaID = p.Id;
            _context.Cartoes.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPessoa", new { email = p.Email }, p);

        }

    }
}
