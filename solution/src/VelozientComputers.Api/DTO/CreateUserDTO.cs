using System;

namespace VelozientComputers.Api.DTO
{
    /// <summary>
    /// DTO para criação de um novo usuário
    /// </summary>
    public class CreateUserDTO
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Sobrenome do usuário
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string Email { get; set; }
    }
}