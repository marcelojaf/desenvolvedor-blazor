using System;

namespace VelozientComputers.Api.DTO
{
    /// <summary>
    /// DTO para atribuição de computador a um usuário
    /// </summary>
    public class ComputerAssignmentDto
    {
        /// <summary>
        /// ID do computador a ser atribuído
        /// </summary>
        public int ComputerId { get; set; }

        /// <summary>
        /// ID do usuário que receberá o computador
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Data de início da atribuição
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de término da atribuição (opcional)
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
