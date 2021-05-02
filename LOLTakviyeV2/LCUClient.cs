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
using System.Threading.Tasks;

namespace loltakviyev2
{
  internal class LCUClient
  {
    public readonly string appPort;
    public readonly string authToken;
    public readonly int processId;
    public readonly string clientUri;
    private HttpClient httpClient = (HttpClient) null;
    private HttpClientHandler clientHandler = new HttpClientHandler();

    public LCUClient(string appPort, string authToken, int processId)
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

    public void SetHeadless()
    {
    }

    public async Task<HttpResponseMessage> HttpGet(string apiUrl)
    {
      HttpResponseMessage async = await this.httpClient.GetAsync(this.clientUri + apiUrl);
      return async;
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

    public async Task<HttpResponseMessage> HttpPutJson(
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

    public async Task<HttpResponseMessage> HttpPostForm(
      string apiUrl,
      IEnumerable<KeyValuePair<string, string>> formData)
    {
      HttpResponseMessage result;
      using (FormUrlEncodedContent content = new FormUrlEncodedContent(formData))
      {
        HttpResponseMessage httpResponseMessage = await this.httpClient.PostAsync(this.clientUri + apiUrl, (HttpContent) content);
        result = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
      }
      HttpResponseMessage httpResponseMessage1 = result;
      result = (HttpResponseMessage) null;
      return httpResponseMessage1;
    }

    public async Task<HttpResponseMessage> HttpPatchForm(
      string apiUrl,
      Uri requestUri,
      HttpContent iContent)
    {
      HttpMethod method = new HttpMethod("PATCH");
      HttpRequestMessage request = new HttpRequestMessage(method, apiUrl)
      {
        Content = iContent
      };
      HttpResponseMessage response = new HttpResponseMessage();
      try
      {
        response = await this.httpClient.SendAsync(request);
      }
      catch (TaskCanceledException ex)
      {
        Debug.WriteLine("ERROR: " + ex.ToString());
      }
      HttpResponseMessage httpResponseMessage = response;
      method = (HttpMethod) null;
      request = (HttpRequestMessage) null;
      response = (HttpResponseMessage) null;
      return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> HttpPutForm(
      string apiUrl,
      IEnumerable<KeyValuePair<string, string>> formData)
    {
      HttpResponseMessage result;
      using (FormUrlEncodedContent content = new FormUrlEncodedContent(formData))
      {
        HttpResponseMessage httpResponseMessage = await this.httpClient.PutAsync(this.clientUri + apiUrl, (HttpContent) content);
        result = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
      }
      HttpResponseMessage httpResponseMessage1 = result;
      result = (HttpResponseMessage) null;
      return httpResponseMessage1;
    }

    public async Task<HttpResponseMessage> HttpPostForm(
      string apiUrl,
      KeyValuePair<string, string> formData)
    {
      HttpResponseMessage result;
      using (FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) new KeyValuePair<string, string>[1]
      {
        formData
      }))
      {
        HttpResponseMessage httpResponseMessage = await this.httpClient.PostAsync(this.clientUri + apiUrl, (HttpContent) content);
        result = httpResponseMessage;
        httpResponseMessage = (HttpResponseMessage) null;
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
  }
}
