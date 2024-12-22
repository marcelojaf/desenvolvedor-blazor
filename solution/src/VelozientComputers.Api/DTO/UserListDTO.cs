using System;

namespace VelozientComputers.Api.DTO
{
    /// <summary>
    /// DTO para listagem de usuários
    /// </summary>
    public class UserListDto : UpdateUserDto
    {
        /// <summary>
        /// Lista de computadores atualmente atribuídos ao usuário
        /// </summary>
        public ICollection<ComputerListDto> AssignedComputers { get; set; }
    }
}
