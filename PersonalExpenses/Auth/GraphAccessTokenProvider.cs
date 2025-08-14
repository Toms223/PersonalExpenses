using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

public class GraphAccessTokenProvider : IAuthenticationProvider
{
    private readonly string _accessToken;

    public GraphAccessTokenProvider(string accessToken)
    {
        _accessToken = accessToken;
    }

    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_accessToken))
            throw new InvalidOperationException("Access token is null or empty.");

        request.Headers["Authorization"] = [$"Bearer {_accessToken}"];
        return Task.CompletedTask;
    }
}