using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cross.Proxy
{
    public interface IProxy
    {
        Task<TModel> Get<TModel>(Uri uri) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth,
            params KeyValuePair<string, string>[] headers) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, string token) where TModel : class;
        Task PostAsync(Uri url, object body, string endPoint = null);
        Task PutAsync(Uri url, object body, string endPoint = null);
        Task<TModel> PutAsync<TModel>(Uri url, object body, string endPoint = null) where TModel : class;
        Task<TModel> DeleteAsync<TModel>(Uri url, object body = null, string endPoint = null) where TModel : class;

    }
}
