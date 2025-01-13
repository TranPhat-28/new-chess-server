using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.DTOs.GameMoveDTO;
using new_chess_server.DTOs.PracticeModeDTO;

namespace new_chess_server.Services.PracticeMode
{
    public interface IPracticeModeService
    {
        Task<ServiceResponse<ResponseMoveDto>> Move(RequestInputMoveDto requestInputMoveDto);
        Task<ServiceResponse<bool>> CheckIfSavedGameExist();
        Task<ServiceResponse<List<MoveHistoryItemDto>>> GetSavedGameHistory();
        Task<ServiceResponse<int>> DeleteSavedGameHistory();
        Task<ServiceResponse<int>> UpdateSavedGameHistory(UpdateGameHistoryDto updateGameHistoryDto);
    }
}