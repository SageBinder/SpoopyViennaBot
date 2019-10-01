using System.Threading.Tasks;

namespace SpoopyViennaBot.Utils.CommandsMeta
{
    public interface IInvokableMessageActor
    {
         Task Invoke(MessageContext context);
    }
}