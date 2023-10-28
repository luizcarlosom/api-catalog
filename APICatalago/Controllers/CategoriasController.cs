using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try 
            {
                var categorias = _context.Categorias.AsNoTracking().ToList();
                if (categorias is null)
                {
                    //404
                    return NotFound("Categorias não encontradas...");
                }
                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um problema ao tratar a sua solicitação.");
            }       
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    //404
                    return NotFound($"Categoria com id= {id} não encontrado...");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um problema ao tratar a sua solicitação.");
            }   
        }


        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)
                {
                    //400
                    return BadRequest();
                }

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                //201
                return new CreatedAtRouteResult("ObterProduto",
                        new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
            
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                //Verificando se o Id passado no body da requisição é igual o Id do produto que ele quer atualizar se não for
                //400
                if (id != categoria.CategoriaId)
                {
                    return BadRequest("Dados inválidos");
                }

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
            
           
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                //Buscando produto no bd
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Produto não localizado");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }
    }
}
