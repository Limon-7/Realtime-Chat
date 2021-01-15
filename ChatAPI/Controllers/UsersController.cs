using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAPI.Data;
using ChatAPI.Models;
using System.Security.Claims;
using ChatAPI.Services;
using AutoMapper;
using ChatAPI.Resourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IMessageRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<BroadcastHub> _hubContext;

        public UsersController(IMessageRepository repo, IMapper mapper, IHubContext<BroadcastHub> hubContext)
        {

            _repo = repo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     return Unauthorized();

            var usersFromRepo = await _repo.GetAllUsers();
            var users = _mapper.Map<IEnumerable<UserForReturnDto>>(usersFromRepo);
            await _hubContext.Clients.All.SendAsync("getUser", _mapper.Map<IEnumerable<UserForReturnDto>>(usersFromRepo));
            return Ok(users);
        }

    }
}