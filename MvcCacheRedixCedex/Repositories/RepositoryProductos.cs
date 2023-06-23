using MvcCacheRedixCedex.Helpers;
using MvcCacheRedixCedex.Models;
using System.Xml.Linq;

namespace MvcCacheRedixCedex.Repositories
{
    public class RepositoryProductos
    {
        private XDocument document;

        public RepositoryProductos(HelperPathProvider helperPath)
        {
            string path = helperPath.MapPath("productos.xml", Folders.Documents);
            this.document = XDocument.Load(path);
        }

        public List<Producto> GetProductos()
        {
            var consulta = from datos in this.document.Descendants("producto")
                           select datos;
            List<Producto> productosList = new List<Producto>();
            foreach (var tag in consulta)
            {
                Producto prod = new Producto();
                prod.IdProducto = int.Parse( tag.Element("idproducto").Value);
                prod.Nombre = tag.Element("nombre").Value;
                prod.Descripcion = tag.Element("descripcion").Value;
                prod.Precio = int.Parse(tag.Element("precio").Value);
                prod.Imagen = tag.Element("imagen").Value;
                productosList.Add(prod);
            }
            return productosList;
        }

        public Producto FindProducto(int id)
        {
            return this.GetProductos().FirstOrDefault(x => x.IdProducto == id);
        }
    }
}
