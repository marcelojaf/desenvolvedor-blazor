using System;

namespace VelozientComputers.Api.DTO
{
    /// <summary>
    /// DTO para listagem de computadores
    /// </summary>
    public class ComputerListDTO : UpdateComputerDTO
    {
        /// <summary>
        /// Status da garantia (RED, YELLOW, GREEN)
        /// </summary>
        public string WarrantyStatus { get; set; }

        /// <summary>
        /// Usuário atual atribuído ao computador (se houver)
        /// </summary>
        public UserListDto CurrentUser { get; set; }
    }
}
