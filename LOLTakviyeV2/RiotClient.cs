using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace loltakviyev2
{
  internal class RiotClient
  {
    public readonly string appPort;
    public readonly string authToken;
    public readonly int processId;
    public readonly string clientUri;
    private HttpClient httpClient = (HttpClient) null;
    private HttpClientHandler clientHandler = new HttpClientHandler();

    public RiotClient(string appPort, string authToken, int processId)
    {
      this.appPort = appPort;
      this.authToken = authToken;
      this.processId = processId;
      this.clientHandler.ServerCertificateCustomValidationCallback = (Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>) ((sender, cert, chain, sslPolicyErrors) => true);
      this.httpClient = new HttpClient((HttpMessageHandler) this.clientHandler);
      this.clientUri = "https://127.0.0.1:" + appPort;
      this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("riot:" + authToken)));
      ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
    }

    public bool IsAlive() => ((IEnumerable<Process>) Process.GetProcesses()).Any<Process>((Func<Process, bool>) (x => x.Id == this.processId));

    public async Task<bool> LoginAsync(string username, string password)
    {
      string body = "{\"clientId\":\"riot-client\",\"trustLevels\":[\"always_trusted\"]}";
      HttpResponseMessage httpResponseMessage1 = await this.HttpPostJson("/rso-auth/v2/authorizations", body);
      string body2 = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"persistLogin\":false}";
      HttpResponseMessage httpResponseMessage = await this.HttpPut("/rso-auth/v1/session/credentials", body2);
      HttpResponseMessage response = httpResponseMessage;
      httpResponseMessage = (HttpResponseMessage) null;
      bool result;
      if (!response.IsSuccessStatusCode)
      {
        result = false;
      }
      else
      {
        Thread.Sleep(1000);
        HttpResponseMessage httpResponseMessage2 = await this.HttpPostJson("/product-launcher/v1/products/league_of_legends/patchlines/live", (string) null);
        result = true;
        httpResponseMessage2 = (HttpResponseMessage) null;
      }
      bool flag = result;
      body = (string) null;
      body2 = (string) null;
      httpResponseMessage = (HttpResponseMessage) null;
      response = (HttpResponseMessage) null;
      return flag;
    }

    public async Task<HttpResponseMessage> HttpPostJson(
      string apiUrl,
      string body)
    {
      bool flag = body == "" || body == null;
      HttpResponseMessage result;
      if (flag)
      {
        HttpResponseMessage httpResponseMessage = await this.httpClient.PostAsync(this.clientUri + apiUrl, (HttpContent) null);
        result = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
      }
      else
      {
        using (StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json"))
        {
          HttpResponseMessage httpResponseMessage2 = await this.httpClient.PostAsync(this.clientUri + apiUrl, (HttpContent) stringContent);
          result = httpResponseMessage2;
          httpResponseMessage2 = (HttpResponseMessage) null;
        }
      }
      HttpResponseMessage httpResponseMessage1 = result;
      result = (HttpResponseMessage) null;
      return httpResponseMessage1;
    }

    public async Task<HttpResponseMessage> HttpDelete(string apiUrl)
    {
      HttpResponseMessage httpResponseMessage = await this.httpClient.DeleteAsync(this.clientUri + apiUrl);
      return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> HttpPut(
      string apiUrl,
      string body)
    {
      bool flag = body == "" || body == null;
      HttpResponseMessage result;
      if (flag)
      {
        HttpResponseMessage httpResponseMessage = await this.httpClient.PutAsync(this.clientUri + apiUrl, (HttpContent) null);
        result = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
      }
      else
      {
        using (StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json"))
        {
          HttpResponseMessage httpResponseMessage2 = await this.httpClient.PutAsync(this.clientUri + apiUrl, (HttpContent) stringContent);
          result = httpResponseMessage2;
          httpResponseMessage2 = (HttpResponseMessage) null;
        }
      }
      HttpResponseMessage httpResponseMessage1 = result;
      result = (HttpResponseMessage) null;
      return httpResponseMessage1;
    }
  }
}
