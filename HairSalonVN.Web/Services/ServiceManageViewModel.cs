using System.ComponentModel.DataAnnotations;

namespace HairSalonVN.Web.Models.Service
{
    public class ServiceManageViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ không được trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên dịch vụ phải từ 2-100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá không được trống")]
        [Range(0.01, 100000000, ErrorMessage = "Giá phải từ 0.01 đến 100.000.000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Thời lượng không được trống")]
        [Range(1, 1440, ErrorMessage = "Thời lượng phải từ 1 đến 1440 phút")]
        public int DurationMinutes { get; set; }

        public bool IsActive { get; set; } = true;
    }
}