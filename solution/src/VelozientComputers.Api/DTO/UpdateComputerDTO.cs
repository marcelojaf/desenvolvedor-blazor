using System;

namespace VelozientComputers.Api.DTO
{

    /// <summary>
    /// DTO para atualização de um computador existente
    /// </summary>
    public class UpdateComputerDTO : CreateComputerDTO
    {
        /// <summary>
        /// Identificador único do computador
        /// </summary>
        public int Id { get; set; }
    }
}