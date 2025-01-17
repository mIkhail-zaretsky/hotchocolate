using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Execution;

#if ASPNETCLASSIC
using Microsoft.Owin;
using HttpContext = Microsoft.Owin.IOwinContext;
#else
using Microsoft.AspNetCore.Http;
#endif

#if ASPNETCLASSIC
namespace HotChocolate.AspNetClassic
#else
namespace HotChocolate.AspNetCore
#endif
{
    public delegate Task<ConnectionStatus> OnConnectWebSocketAsync(
        IHttpContext context,
        IDictionary<string, object> properties,
        CancellationToken cancellationToken);

    public delegate Task OnCreateRequestAsync(
        IHttpContext context,
        IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken);

    public class QueryMiddlewareOptions
    {
        private PathString _path = new PathString("/");
        private PathString _subscriptionPath = new PathString("/ws");

        public int QueryCacheSize { get; set; } = 100;

        public PathString Path
        {
            get => _path;
            set
            {
                if (!value.HasValue)
                {
                    throw new ArgumentException(
                        "The path cannot be empty.");
                }

                _path = value;
                SubscriptionPath = value + new PathString("/ws");
            }
        }

        public PathString SubscriptionPath
        {
            get => _subscriptionPath;
            set
            {
                if (!value.HasValue)
                {
                    throw new ArgumentException(
                        "The subscription-path cannot be empty.");
                }

                _subscriptionPath = value;
            }
        }

        public OnConnectWebSocketAsync OnConnectWebSocket { get; set; }

        public OnCreateRequestAsync OnCreateRequest { get; set; }
    }
}
