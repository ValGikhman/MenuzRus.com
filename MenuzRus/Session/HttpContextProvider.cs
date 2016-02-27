using System.Web;

namespace MenuzRus {

    /// <summary>
    /// A default HTTP context provider, returning a <see cref="HttpContextWrapper"/> from <see cref="HttpContext.Current"/>.
    /// </summary>
    public class HttpContextProvider : IHttpContextProvider {

        public HttpContextBase Current {
            get {
                return new HttpContextWrapper(HttpContext.Current);
            }
        }
    }
}