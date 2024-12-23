using System;

namespace VelozientComputers.Api.DTOs
{
    /// <summary>
    /// DTO para atualização de status do computador
    /// </summary>
    public class UpdateComputerStatusDto
    {
        /// <summary>
        /// ID do computador
        /// </summary>
        public int ComputerId { get; set; }

        /// <summary>
        /// Novo status do computador
        /// </summary>
        public string NewStatus { get; set; }
    }
}