using AccessData;
using AutoMapper;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserClientService : IUserClientService
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public UserClientService(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

    }
}