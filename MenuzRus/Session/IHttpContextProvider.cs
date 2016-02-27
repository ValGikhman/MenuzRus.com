using System.Web;

namespace MenuzRus {

    public interface IHttpContextProvider {

        /// <summary>
        /// Gets the current HTTP context.
        /// </summary>
        /// <value>The current HTTP context.</value>
        HttpContextBase Current { get; }
    }
}