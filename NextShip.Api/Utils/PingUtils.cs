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
        string status;
        status = reply.Status switch
        {
            IPStatus.Success => "成功",
            IPStatus.TimedOut => "超时",
            _ => "失败"
        };
        stringB.AppendLine("状态：" + status);

        if (reply.Status == IPStatus.Success)
        {
            stringB.AppendLine(string.Format("Ip地址: {0} ", reply.Address));
            stringB.AppendLine(string.Format("ping时间: {0} ", reply.Options.Ttl));
            stringB.AppendLine(string.Format("ping包大小: {0} ", reply.Buffer.Length));
            stringB.AppendLine(string.Format("往返时间: {0} ", reply.RoundtripTime));
        }

        return new PingInfo(reply.Address.ToString(), reply.Options.Ttl, reply.Buffer.Length,
            reply.RoundtripTime, stringB);
    }
}

public class PingInfo
{
    public string ip;
    public int pingTime;
    public long roundTripTime;
    public int size;
    public StringBuilder stringB;

    public PingInfo
        (string ip, int pingTime, int size = -1, long roundTripTime = -1, StringBuilder stringB = null)
    {
        this.ip = ip;
        this.pingTime = pingTime;
        this.size = size;
        this.roundTripTime = roundTripTime;
        this.stringB = stringB;
    }
}