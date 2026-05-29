using System.ComponentModel.DataAnnotations;

namespace HairSalonVN.API.Services.DTOs.Services
{
    public class ServiceCreateDto
    {
        [Required(ErrorMessage = "Tên dịch vụ không được trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên dịch vụ phải từ 2-100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả không quá 500 ký tự")]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(0.01, 100000000, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Range(1, 1440, ErrorMessage = "Thời gian phải từ 1-1440 phút")]
        public int DurationMinutes { get; set; }
    }
}
