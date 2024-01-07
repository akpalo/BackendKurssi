using BackendHarj.Models;
using System.Threading.Tasks;

namespace BackendHarj.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
        
        Task<IEnumerable<MessageDTO>> SearchMessagesAsync(string searchtext);

        Task<IEnumerable<MessageDTO>> GetReceivedMessagesAsync(string username);

        Task<IEnumerable<MessageDTO>> GetSentMessagesAsync(string username);

        Task<MessageDTO> GetMessageAsync(long id);

        Task<MessageDTO> NewMessageAsync(MessageDTO message);

        Task<bool> UpdateMessageAsync(MessageDTO message);

        Task<bool> DeleteMessageAsync(long id);
        
    }
}
