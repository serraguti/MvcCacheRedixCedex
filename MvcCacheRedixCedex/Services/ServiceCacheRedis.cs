using MvcCacheRedixCedex.Helpers;
using MvcCacheRedixCedex.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCacheRedixCedex.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;

        public ServiceCacheRedis()
        {
            this.database =
                HelperCacheMultiplexer.Connection.GetDatabase();
        }

        //METODO PARA AÑADIR FAVORITOS
        public async Task AddProductoFavoritoAsync(Producto favorito)
        {
            //CACHE REDIS FUNCIONA CON KEY/VALUE
            //SI NO PERSONALIZO LAS KEYS, TODOS LOS USUARIOS
            //UTILIZAN LA MISMA
            //VAMOS A ALMACENAR UN JSON DE LA COLECCION DE PRODUCTOS
            string jsonProductos = await this.database.StringGetAsync("favoritos");
            List<Producto> productosFavoritos;
            if (jsonProductos == null)
            {
                productosFavoritos = new List<Producto>();
            }
            else
            {
                productosFavoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
            }
            productosFavoritos.Add(favorito);
            jsonProductos = JsonConvert.SerializeObject(productosFavoritos);
            await this.database.StringSetAsync("favoritos", jsonProductos);
        }

        //METODO PARA RECUPERAR TODOS LOS FAVORITOS
        public async Task<List<Producto>> GetProductosFavoritosAsync()
        {
            string jsonProductos = await this.database.StringGetAsync("favoritos");
            if (jsonProductos == null)
            {
                return null;
            }
            else
            {
                List<Producto> favoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
                return favoritos;
            }
        }

        //METODO PARA ELIMINAR FAVORITOS
        public async Task DeleteProductoFavorito(int idproducto)
        {
            List<Producto> favoritos = await this.GetProductosFavoritosAsync();
            if (favoritos != null)
            {
                //BUSCAMOS EL PRODUCTO A ELIMINAR
                Producto productoDelete =
                    favoritos.FirstOrDefault(x => x.IdProducto == idproducto);
                favoritos.Remove(productoDelete);
                //SI YA NO TENEMOS FAVORITOS, ELIMINAMOS LA KEY COMPLETA
                if (favoritos.Count == 0)
                {
                    await this.database.KeyDeleteAsync("favoritos");
                }else
                {
                    string jsonProductos =
                        JsonConvert.SerializeObject(favoritos);
                    //ALMACENAMOS DE NUEVO LOS FAVORITOS.
                    //SI NO PONEMOS TIEMPO, CACHE REDIS ALMACENA 24 HORAS
                    //LOS DATOS ANTES DE DESTRUIRLOS, PERO PUEDO PONER TIEMPO
                    //AL CREAR UN OBJETO/KEY
                    await this.database.StringSetAsync
                        ("favoritos", jsonProductos, TimeSpan.FromMinutes(60));
                }
            }
        }
    }
}
