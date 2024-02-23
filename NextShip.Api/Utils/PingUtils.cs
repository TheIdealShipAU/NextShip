#nullable enable
using System.Net.NetworkInformation;
using System.Text;

namespace NextShip.Api.Utils;

public static class PingUtils
{
    public static PingInfo Ping(string url)
    {
        Msg($"ping {url}", MethodUtils.GetVoidName(), MethodUtils.GetClassName());

        var ping = new Ping();
        var reply = ping.Send(url);

        var stringB = new StringBuilder();
        var status = reply.Status switch
        {
            IPStatus.Success => "成功",
            IPStatus.TimedOut => "超时",
            _ => "失败"
        };

        stringB.AppendLine("状态：" + status);

        if (reply.Status != IPStatus.Success)
            return new PingInfo(reply.Address.ToString(), reply.Options!.Ttl, reply.Buffer.Length,
                reply.RoundtripTime, stringB);

        stringB.AppendLine($"Ip地址: {reply.Address} ");
        stringB.AppendLine($"ping时间: {reply.Options!.Ttl} ");
        stringB.AppendLine($"ping包大小: {reply.Buffer.Length} ");
        stringB.AppendLine($"往返时间: {reply.RoundtripTime} ");

        return new PingInfo(reply.Address.ToString(), reply.Options!.Ttl, reply.Buffer.Length,
            reply.RoundtripTime, stringB);
    }
}

public class PingInfo(string ip, int pingTime, int size = -1, long roundTripTime = -1, StringBuilder? stringB = null)
{
    public readonly int pingTime = pingTime;
    public string ip = ip;
    public long roundTripTime = roundTripTime;
    public int size = size;
    public StringBuilder? stringB = stringB;
}