using loltakviyev2;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

internal class LCUListener
{
  private Thread listeningThread;
  private bool listening = false;
  private ConcurrentDictionary<int, LCUClient> gatheredLCUs = new ConcurrentDictionary<int, LCUClient>();
  private ConcurrentDictionary<int, RiotClient> gatheredRiots = new ConcurrentDictionary<int, RiotClient>();

  public void StartListening()
  {
    this.listening = true;
    this.listeningThread = new Thread(new ThreadStart(this.ListenForAnyClients));
    this.listeningThread.Start();
  }

  public void StopListening()
  {
    this.listening = false;
    this.listeningThread.Join();
  }

  public void WaitForAnyClient()
  {
    while (this.GetGatheredLCUs().Count <= 0)
      Thread.Sleep(100);
  }

  public void WaitForAnyRiot()
  {
    while (this.GetGatheredRiots().Count <= 0)
      Thread.Sleep(100);
  }

  public List<LCUClient> GetGatheredLCUs()
  {
    foreach (KeyValuePair<int, LCUClient> gatheredLcU in this.gatheredLCUs)
    {
      int key = gatheredLcU.Key;
      if (!gatheredLcU.Value.IsAlive())
        this.gatheredLCUs.TryRemove(key, out LCUClient _);
    }
    return this.gatheredLCUs.Values.ToList<LCUClient>();
  }

  public List<RiotClient> GetGatheredRiots()
  {
    foreach (KeyValuePair<int, RiotClient> gatheredRiot in this.gatheredRiots)
    {
      int key = gatheredRiot.Key;
      if (!gatheredRiot.Value.IsAlive())
        this.gatheredRiots.TryRemove(key, out RiotClient _);
    }
    return this.gatheredRiots.Values.ToList<RiotClient>();
  }

  private void ListenForAnyClients()
  {
    while (this.listening)
    {
      foreach (Process process in Process.GetProcessesByName("LeagueClientUx"))
      {
        if (!this.IsAlreadyFound(process.Id))
        {
          string parameterValue;
          ProcessCommandLine.Retrieve(process, out parameterValue);
          string authToken = Regex.Match(parameterValue, "(\"--remoting-auth-token=)([^\"]*)(\")").Groups[2].Value;
          LCUClient lcuClient = new LCUClient(int.Parse(Regex.Match(parameterValue, "(\"--app-port=)([^\"]*)(\")").Groups[2].Value).ToString(), authToken, process.Id);
          this.gatheredLCUs.TryAdd(process.Id, lcuClient);
        }
      }
      foreach (Process process in Process.GetProcessesByName("RiotClientUx"))
      {
        if (!this.IsAlreadyFound(process.Id))
        {
          string parameterValue;
          ProcessCommandLine.Retrieve(process, out parameterValue);
          string authToken = Regex.Match(parameterValue, "(--remoting-auth-token=)([^\\s]+)").Groups[2].Value;
          RiotClient riotClient = new RiotClient(int.Parse(Regex.Match(parameterValue, "(--app-port=)([0-9]+)").Groups[2].Value).ToString(), authToken, process.Id);
          this.gatheredRiots.TryAdd(process.Id, riotClient);
        }
      }
      Thread.Sleep(100);
    }
  }

  private bool IsAlreadyFound(int pid)
  {
    foreach (KeyValuePair<int, LCUClient> gatheredLcU in this.gatheredLCUs)
    {
      if (gatheredLcU.Key == pid)
        return true;
    }
    foreach (KeyValuePair<int, RiotClient> gatheredRiot in this.gatheredRiots)
    {
      if (gatheredRiot.Key == pid)
        return true;
    }
    return false;
  }
}
