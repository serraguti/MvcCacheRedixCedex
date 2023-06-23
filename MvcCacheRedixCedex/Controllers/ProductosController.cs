using Microsoft.AspNetCore.Mvc;
using MvcCacheRedixCedex.Models;
using MvcCacheRedixCedex.Repositories;
using MvcCacheRedixCedex.Services;

namespace MvcCacheRedixCedex.Controllers
{
    public class ProductosController : Controller
    {
        private RepositoryProductos repo;
        private ServiceCacheRedis service;
        public ProductosController
            (RepositoryProductos repo, ServiceCacheRedis service)
        {
            this.service = service;
            this.repo = repo;
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Producto> favoritos = await this.service.GetProductosFavoritosAsync();
            return View(favoritos);
        }

        public async Task<IActionResult> SeleccionarFavorito(int idproducto)
        {
            //BUSCAMOS EL PRODUCTO A ASOCIAR DENTRO DEL REPO
            Producto producto = this.repo.FindProducto(idproducto);
            await this.service.AddProductoFavoritoAsync(producto);
            return RedirectToAction("Favoritos");
        }

        public async Task<IActionResult> DeleteFavorito(int idproducto)
        {
            await this.service.DeleteProductoFavorito(idproducto);
            return RedirectToAction("Favoritos");
        }
        
        public IActionResult Index()
        {
            List<Producto> productos = this.repo.GetProductos();
            return View(productos);
        }

        public IActionResult Details(int id)
        {
            Producto prod = this.repo.FindProducto(id);
            return View(prod);
        }
    }
}
