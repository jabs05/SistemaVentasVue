using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Articulo;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ArticulosController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Articulos/Listar
        [HttpGet("[action]")]
        public async Task <IEnumerable<ArticuloViewModel>> Listar()
        {
            //se agrega la categoria en el include porque hay un forening key de referencia con la tabla categoria.
            var articulo = await _context.Articulos.Include(a => a.categoria).ToListAsync();

            return articulo.Select(c => new ArticuloViewModel
            {
                idarticulo = c.idarticulo,
                idcategoria = c.idcategoria,
                categoria = c.categoria.nombre,
                codigo = c.codigo,
                nombre = c.nombre,
                precio_venta = c.precio_venta,
                stock= c.stock,
                descripcion= c.descripcion,
                condicion= c.condicion
            });
        }

        // GET: api/Articulos/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria).
                SingleOrDefaultAsync(a => a.idarticulo==id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo = articulo.idarticulo,
                idcategoria = articulo.idcategoria,
                categoria = articulo.categoria.nombre,
                codigo = articulo.codigo,
                nombre = articulo.nombre,
                precio_venta = articulo.precio_venta,
                stock = articulo.stock,
                descripcion = articulo.descripcion,
                condicion = articulo.condicion
            });
        }

        // PUT: api/Articulos/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idarticulo <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == model.idarticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.idcategoria = model.idcategoria;
            articulo.codigo = model.codigo;
            articulo.nombre = model.nombre;
            articulo.precio_venta = model.precio_venta;
            articulo.stock = model.stock;
            articulo.descripcion = model.descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Articulo articulo = new Articulo
            {
                idcategoria = model.idcategoria,
                codigo = model.codigo,
                nombre = model.nombre,
                precio_venta= model.precio_venta,
                stock = model.stock,
                descripcion = model.descripcion,
                condicion = true
            };
            _context.Articulos.Add(articulo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Put: api/Categorias/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Put: api/Categorias/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(c => c.idarticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.condicion = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }


    }
}