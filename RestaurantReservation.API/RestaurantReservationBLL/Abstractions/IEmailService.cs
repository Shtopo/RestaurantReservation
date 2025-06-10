namespace RestaurantReservation.API.RestaurantReservationBLL.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}