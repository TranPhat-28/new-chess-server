using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.GameMoveDTO;

namespace new_chess_server.Services.PracticeMode
{
    public class PracticeModeService : IPracticeModeService
    {
        public Task<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto)
        {
            throw new NotImplementedException();
        }
    }
}