using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Cross.Proxy
{
    public interface IProxy
    {
        public void SetDefaultConfig([NotNull] string clientName);
        Task<TModel> Get<TModel>(Uri uri= null) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, KeyValuePair<string, string> auth, params KeyValuePair<string, string>[] headers) where TModel : class;
        Task<TModel> Get<TModel>(Uri uri, string token) where TModel : class;
        Task PostAsync(Uri url, [NotNull] object body, string endPoint = null, string token = null);
        Task<TModel> PostAsync<TModel>([NotNull] object body, Uri url = null, string endPoint = null, string token = null) where TModel : class;
        Task PutAsync(Uri url, object body, string endPoint = null);
        Task<TModel> PutAsync<TModel>(Uri url, object body, string endPoint = null, string token = null) where TModel : class;        
        Task<TModel> DeleteAsync<TModel>(Uri url, object body = null, string endPoint = null) where TModel : class;

    }
}
