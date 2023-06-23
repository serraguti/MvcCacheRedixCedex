namespace MvcCacheRedixCedex.Helpers
{
    public enum Folders { Images, Css, Temporal, Documents }

    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            else if (folder == Folders.Documents)
            {
                carpeta = "documents";
            }
            else if (folder == Folders.Css)
            {
                carpeta = "css";
            }
            else if (folder == Folders.Temporal)
            {
                carpeta = "temporal";
            }
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }
    }
}
