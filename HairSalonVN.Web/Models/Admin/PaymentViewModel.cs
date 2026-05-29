using HairSalonVN.Database.Constants;

namespace HairSalonVN.Web.Models.Admin;

public class PaymentViewModel
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
    public string AmountDisplay => Amount.ToString("N0") + "đ";
    public string MethodLabel => Method switch
    {
        PaymentMethods.Cash => "Tiền mặt",
        PaymentMethods.Card => "Thẻ",
        PaymentMethods.Transfer => "Chuyển khoản",
        PaymentMethods.MoMo => "MoMo",
        _ => Method
    };
}

public static class PaymentConstants
{
    public const string MoMoPhone = MoMoConstants.MoMoPhone;
    public const string MoMoName = MoMoConstants.MoMoName;
}
