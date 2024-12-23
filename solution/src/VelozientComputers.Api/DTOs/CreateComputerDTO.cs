namespace VelozientComputers.Api.DTOs
{
    /// <summary>
    /// DTO para criação de um novo computador
    /// </summary>
    public class CreateComputerDTO
    {
        /// <summary>
        /// Fabricante do computador (Apple, Dell, HP, Lenovo)
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Número de série único do computador
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Status atual do computador
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Data de compra do computador
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Data de expiração da garantia
        /// </summary>
        public DateTime WarrantyExpiryDate { get; set; }

        /// <summary>
        /// Especificações técnicas do computador
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// URL da imagem do computador
        /// </summary>
        public string ImageUrl { get; set; }
    }
}