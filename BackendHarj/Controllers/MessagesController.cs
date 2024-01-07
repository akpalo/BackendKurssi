using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendHarj.Models;
using Microsoft.AspNetCore.Authorization;
using BackendHarj.Services;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BackendHarj.Middleware;

namespace BackendHarj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserAuthenticationService _authService;

        public MessagesController(IMessageService service, IUserAuthenticationService authService)
        {
            _messageService = service;
            _authService = authService;
        }

        // GET: api/Messages

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return Ok(await _messageService.GetMessagesAsync());
        }


        // GET: api/search/searchtext
        [HttpGet("search/{searchtext}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Message>>> SearchMessagesAsync(string searchtext)
        {
            return Ok(await _messageService.SearchMessagesAsync(searchtext));
        }

        //GET: api/messages/sent/username
        [HttpGet("sent/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMySentMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == username)
            {
                IEnumerable<MessageDTO>? list = await _messageService.GetSentMessagesAsync(username);
                if (list == null)
                {
                    return BadRequest();
                }
                return Ok(list);
            }
            return BadRequest();
        }
        //GET: api/messages/received/username
        [HttpGet("received/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Message>>> GetMyReceivedMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == username)
            {
                IEnumerable<MessageDTO>? list = await _messageService.GetReceivedMessagesAsync(username);
                if (list == null)
                {
                    return BadRequest();
                }
                return Ok(list);
            }
            return BadRequest();
        }   
        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDTO>> GetMessage(long id)
        {
          MessageDTO message = await _messageService.GetMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMessage(long id, MessageDTO message)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == message.Sender)
            {
                if (id != message.Id)
                {
                    return BadRequest();
                }

                bool result = await _messageService.UpdateMessageAsync(message);

                if (!result)
                {
                    return NotFound(message);
                }

                return NoContent();
            }
            return BadRequest();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == message.Sender)
            {
                MessageDTO newMessage = await _messageService.NewMessageAsync(message);

                if (newMessage == null)
                {
                    return Problem();
                }

                return CreatedAtAction("GetMessage", new { id = message.Id }, message);
            }
            return BadRequest();
        }
        

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            if (await _authService.isMyMessage(this.User.FindFirst(ClaimTypes.Name).Value, id))
            {
                if (await _messageService.DeleteMessageAsync(id))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

    }
}
