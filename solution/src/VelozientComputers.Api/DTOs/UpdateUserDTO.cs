using System;

namespace VelozientComputers.Api.DTOs
{
    /// <summary>
    /// DTO para atualização de um usuário existente
    /// </summary>
    public class UpdateUserDTO : CreateUserDTO
    {
        /// <summary>
        /// Identificador único do usuário
        /// </summary>
        public int Id { get; set; }
    }
}