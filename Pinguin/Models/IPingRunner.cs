using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Pinguin.Models;

public interface IPingRunner
{
    public Options Settings { get; set; }
    public ObservableCollection<PingObject> Pings { get; set; }

    public Dictionary<PingObject, CancellationTokenSource> Tasks { get; set; }
    Task Tracert(string host);
    Task AddPing(string host);
    Task AddPing(PingObject ping);
    Task RemovePing(PingObject ping);
    Task RunPing(PingObject ping, CancellationToken cancel);
}