using BackendHarj.Models;

namespace BackendHarj.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();

        Task<IEnumerable<Message>> SearchMessagesAsync(string searchtext);

        Task<IEnumerable<Message>> GetSentMessagesAsync(User user);

        Task<IEnumerable<Message>> GetReceivedMessagesAsync(User user);

        Task<Message?> GetMessagesAsync(long id);

        Task<Message?> NewMessageAsync(Message message);

        Task<bool> UpdateMessageAsync(Message message);

        Task<bool> DeleteMessageAsync(Message message);
        
    }
}
