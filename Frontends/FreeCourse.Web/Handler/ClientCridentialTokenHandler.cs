using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Web.Handler
{
    public class ClientCridentialTokenHandler : DelegatingHandler
    {
        private readonly IClientCridentialTokenService _clientCridentialTokenService;

        public ClientCridentialTokenHandler(IClientCridentialTokenService clientCridentialTokenService)
        {
            _clientCridentialTokenService = clientCridentialTokenService;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _clientCridentialTokenService.GetToken());

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizeException();

            return response;
        }
    }
}
